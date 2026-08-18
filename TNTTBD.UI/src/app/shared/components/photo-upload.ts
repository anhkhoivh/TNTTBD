import { Component, inject, input, output, signal, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { switchMap } from 'rxjs';
import { ApiService } from '../../core/services/api.service';
import { AuthService } from '../../core/auth/auth.service';
import { NotificationService } from '../../core/services/notification.service';
import { PhotoPurpose, PhotoVisibility } from '../../shared/enums';
import { TranslatePipe } from '../../core/i18n/translate.pipe';

export type PhotoEntityType = 'catechists' | 'youths';

export interface PhotoUploaded {
  photoId: string;
  url: string;
  contentType: string;
}

interface UploadUrlResponse {
  photoId: string;
  url: string;
  contentType: string;
  purpose: string;
  uploadedBy: string;
}

@Component({
  selector: 'app-photo-upload',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    TranslatePipe,
  ],
  template: `
    <div class="upload">
      <input
        #fileInput
        type="file"
        accept="image/jpeg,image/png,image/gif,image/webp"
        hidden
        (change)="onFile($event)"
      />
      <button
        mat-stroked-button
        type="button"
        (click)="fileInput.click()"
        [disabled]="uploading() || !entityId()"
      >
        @if (uploading()) {
          <mat-spinner diameter="18"></mat-spinner>
        } @else {
          <mat-icon>add_a_photo</mat-icon>
        }
        {{ 'photo.upload' | t }}
      </button>
    </div>
  `,
  styles: [
    `
      .upload {
        display: inline-flex;
        align-items: center;
      }
    `,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PhotoUpload {
  private api = inject(ApiService);
  private http = inject(HttpClient);
  private auth = inject(AuthService);
  private notify = inject(NotificationService);

  readonly entityType = input.required<PhotoEntityType>();
  readonly entityId = input.required<string>();
  readonly purpose = input<PhotoPurpose>(PhotoPurpose.Profile);
  readonly visibility = input<PhotoVisibility>(PhotoVisibility.Public);
  readonly uploaded = output<PhotoUploaded>();

  protected uploading = signal(false);

  protected onFile(ev: Event): void {
    const input = ev.target as HTMLInputElement;
    const file = input.files?.[0];
    input.value = '';
    if (!file) return;

    const userId = this.auth.currentUser()?.id;
    if (!userId) {
      this.notify.error('error.unauthorized');
      return;
    }

    const idKey = this.entityType() === 'catechists' ? 'catechistId' : 'eucharisticYouthId';
    const requestBody = {
      [idKey]: this.entityId(),
      fileName: file.name,
      contentType: file.type,
      purpose: this.purpose(),
      uploadedBy: userId,
      visibility: this.visibility(),
    };

    this.uploading.set(true);
    this.api
      .post<UploadUrlResponse>(`/${this.entityType()}/photos/upload-url`, requestBody)
      .pipe(
        switchMap((ref) => {
          const form = new FormData();
          form.append('file', file);
          return this.http.post(ref.url, form).pipe(
            switchMap(() => {
              this.uploaded.emit({ photoId: ref.photoId, url: ref.url, contentType: ref.contentType });
              return [];
            }),
          );
        }),
      )
      .subscribe({
        next: () => {},
        complete: () => {
          this.uploading.set(false);
          this.notify.success('photo.uploaded');
        },
        error: () => {
          this.uploading.set(false);
          this.notify.error('photo.uploadFailed');
        },
      });
  }
}
