import { ChangeDetectionStrategy, Component, computed, DestroyRef, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { firstValueFrom } from 'rxjs';
import { ApiService } from '../../../core/services/api.service';
import { AuthService } from '../../../core/auth/auth.service';
import { NotificationService } from '../../../core/services/notification.service';
import { DialogService } from '../../../core/services/dialog.service';
import { PhotoPurpose } from '../../enums';
import { TranslatePipe } from '../../../core/i18n/translate.pipe';
import { TranslationService } from '../../../core/i18n/translation.service';
import { PhotoEntityType, PhotoService } from '../../../core/services/photo.service';
import { injectHasCamera, injectIsTouch } from '../../utils/camera';
import { CameraCaptureDialog } from '../camera-capture-dialog/camera-capture-dialog';

const MAX_PHOTO_BYTES = 20 * 1024 * 1024;
const ALLOWED_PHOTO_TYPES = ['image/jpeg', 'image/png', 'image/gif', 'image/webp'];

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
    MatIconModule,
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
  private dialogs = inject(DialogService);
  private data = inject<PhotoUploadData>(MAT_DIALOG_DATA);
  private ref = inject<MatDialogRef<PhotoUpload, PhotoUploaded>>(MatDialogRef);
  private fb = inject(FormBuilder);
  private destroyRef = inject(DestroyRef);
  private touch = injectIsTouch();
  private hasCamera = injectHasCamera();

  protected readonly purposes = Object.values(PhotoPurpose);
  protected form = this.fb.nonNullable.group({
    purpose: [this.data.purpose, Validators.required],
  });
  protected file = signal<File | null>(null);
  protected previewUrl = signal<string | null>(null);
  protected cameraSource = signal(false);
  protected showCamera = computed(() => this.touch() || this.hasCamera());
  protected uploading = signal(false);

  constructor() {
    this.destroyRef.onDestroy(() => {
      const previewUrl = this.previewUrl();
      if (previewUrl) URL.revokeObjectURL(previewUrl);
    });
  }

  protected onFile(ev: Event): void {
    const input = ev.target as HTMLInputElement;
    const file = input.files?.[0] ?? null;
    input.value = '';
    this.clearFile();
    if (file) this.setFile(file, false);
  }

  protected onCameraFile(ev: Event): void {
    const input = ev.target as HTMLInputElement;
    const file = input.files?.[0] ?? null;
    input.value = '';
    if (file) this.setFile(file, true);
  }

  protected async takePhoto(cameraPicker: HTMLInputElement): Promise<void> {
    if (this.touch()) {
      cameraPicker.click();
      return;
    }

    const file = await this.dialogs.open<CameraCaptureDialog, undefined, File | null>(
      CameraCaptureDialog,
      { width: 'min(720px, 95vw)', maxWidth: '95vw', maxHeight: '90vh' },
    );
    if (file) this.setFile(file, true);
  }

  private setFile(file: File, fromCamera: boolean): void {
    if (!/\.(jpe?g|png|gif|webp)$/i.test(file.name) ||
        !ALLOWED_PHOTO_TYPES.includes(file.type) ||
        file.size === 0 || file.size > MAX_PHOTO_BYTES) {
      this.notify.error(this.translate.t('photo.invalidFile'));
      return;
    }

    this.clearFile();
    this.file.set(file);
    this.previewUrl.set(URL.createObjectURL(file));
    this.cameraSource.set(fromCamera);
  }

  private clearFile(): void {
    const previewUrl = this.previewUrl();
    if (previewUrl) URL.revokeObjectURL(previewUrl);
    this.file.set(null);
    this.previewUrl.set(null);
    this.cameraSource.set(false);
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
    const { purpose } = this.form.getRawValue();
    const idKey = this.data.entityType === 'catechists' ? 'catechistId' : 'eucharisticYouthId';
    const requestBody = {
      [idKey]: this.data.entityId,
      fileName: file.name,
      contentType: file.type,
      purpose,
      uploadedBy: userId,
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
