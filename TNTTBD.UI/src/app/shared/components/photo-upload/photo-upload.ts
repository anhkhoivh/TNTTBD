import { Component, inject, signal, ChangeDetectionStrategy } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { firstValueFrom } from 'rxjs';
import { ApiService } from '../../../core/services/api.service';
import { AuthService } from '../../../core/auth/auth.service';
import { NotificationService } from '../../../core/services/notification.service';
import { PhotoPurpose, PhotoVisibility } from '../../enums';
import { TranslatePipe } from '../../../core/i18n/translate.pipe';
import { TranslationService } from '../../../core/i18n/translation.service';
import { PhotoEntityType, PhotoService } from '../../../core/services/photo.service';

export type { PhotoEntityType };

export interface PhotoUploaded {
  photoId: string;
  url: string;
  contentType: string;
  purpose: PhotoPurpose;
}

export interface PhotoUploadData {
  entityType: PhotoEntityType;
  entityId: string;
  purpose: PhotoPurpose;
}

@Component({
  selector: 'app-photo-upload-dialog',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatSelectModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    TranslatePipe,
  ],
  templateUrl: './photo-upload.html',
  styleUrl: './photo-upload.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PhotoUpload {
  private api = inject(ApiService);
  private http = inject(HttpClient);
  private auth = inject(AuthService);
  private notify = inject(NotificationService);
  private translate = inject(TranslationService);
  private photos = inject(PhotoService);
  private data = inject<PhotoUploadData>(MAT_DIALOG_DATA);
  private ref = inject<MatDialogRef<PhotoUpload, PhotoUploaded>>(MatDialogRef);
  private fb = inject(FormBuilder);

  protected readonly purposes = Object.values(PhotoPurpose);
  protected readonly visibilities = Object.values(PhotoVisibility);
  protected form = this.fb.nonNullable.group({
    purpose: [this.data.purpose, Validators.required],
    visibility: [PhotoVisibility.Public, Validators.required],
  });
  protected file = signal<File | null>(null);
  protected uploading = signal(false);

  protected onFile(ev: Event): void {
    const input = ev.target as HTMLInputElement;
    const file = input.files?.[0];
    this.file.set(null);
    if (!file) return;
    if (!/\.(jpe?g|png|gif|webp)$/i.test(file.name) ||
        !['image/jpeg', 'image/png', 'image/gif', 'image/webp'].includes(file.type) ||
        file.size === 0 || file.size > 5 * 1024 * 1024) {
      input.value = '';
      this.notify.error(this.translate.t('photo.invalidFile'));
      return;
    }
    this.file.set(file);
  }

  protected async upload(): Promise<void> {
    if (this.uploading()) return;
    const file = this.file();
    if (!file || this.form.invalid) {
      this.form.markAllAsTouched();
      this.notify.error(this.translate.t('photo.invalidFile'));
      return;
    }
    const userId = this.auth.currentUser()?.id;
    if (!userId) {
      this.notify.error(this.translate.t('error.unauthorized'));
      return;
    }
    const { purpose, visibility } = this.form.getRawValue();
    const idKey = this.data.entityType === 'catechists' ? 'catechistId' : 'eucharisticYouthId';
    const requestBody = {
      [idKey]: this.data.entityId,
      fileName: file.name,
      contentType: file.type,
      purpose,
      uploadedBy: userId,
      visibility,
    };

    this.uploading.set(true);
    this.form.disable();
    this.ref.disableClose = true;
    try {
      const ref = await this.api.postAsync<PhotoUploaded>(
        `/${this.data.entityType}/photos/upload-url`,
        requestBody,
      );
      const form = new FormData();
      form.append('file', file);
      await firstValueFrom(this.http.post(this.photos.url(ref.url), form));
      this.notify.success(this.translate.t('photo.uploaded'));
      this.ref.close({ ...ref, purpose });
    } catch {
      this.notify.error(this.translate.t('photo.uploadFailed'));
    } finally {
      this.uploading.set(false);
      this.form.enable();
      this.ref.disableClose = false;
    }
  }
}
