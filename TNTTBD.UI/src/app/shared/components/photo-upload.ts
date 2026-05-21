import { Component, inject, input, output, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ApiService } from '../../core/services/api.service';
import { NotificationService } from '../../core/services/notification.service';
import { TranslatePipe } from '../../core/i18n/translate.pipe';

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
        accept="image/*"
        hidden
        (change)="onFile($event)"
      />
      <button
        mat-stroked-button
        type="button"
        (click)="fileInput.click()"
        [disabled]="uploading()"
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
})
export class PhotoUpload {
  private api = inject(ApiService);
  private notify = inject(NotificationService);

  readonly endpoint = input.required<string>();
  readonly ownerKey = input<string>('ownerId');
  readonly ownerId = input.required<string>();
  readonly uploaded = output<{ id?: string }>();

  protected uploading = signal(false);

  protected onFile(ev: Event): void {
    const input = ev.target as HTMLInputElement;
    const file = input.files?.[0];
    if (!file) return;
    const form = new FormData();
    form.append(this.ownerKey(), this.ownerId());
    form.append('file', file);
    this.uploading.set(true);
    this.api.post<{ id?: string }>(this.endpoint(), form).subscribe({
      next: (res) => {
        this.uploaded.emit(res ?? {});
        this.notify.success('Uploaded');
      },
      complete: () => this.uploading.set(false),
      error: () => this.uploading.set(false),
    });
    input.value = '';
  }
}
