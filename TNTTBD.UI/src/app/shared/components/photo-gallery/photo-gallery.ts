import { ChangeDetectionStrategy, Component, computed, inject, input, output, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { AuthService } from '../../../core/auth/auth.service';
import { DialogService } from '../../../core/services/dialog.service';
import { TranslationService } from '../../../core/i18n/translation.service';
import { TranslatePipe } from '../../../core/i18n/translate.pipe';
import { PhotoPurpose, UserRole } from '../../enums';
import { PhotoReference } from '../../models/youth.model';
import { PhotoImage } from '../photo-image/photo-image';
import { PhotoEntityType, PhotoUpload, PhotoUploadData, PhotoUploaded } from '../photo-upload/photo-upload';
import { PhotoViewerData, PhotoViewerDialog } from '../photo-viewer-dialog/photo-viewer-dialog';

@Component({
  selector: 'app-photo-gallery',
  standalone: true,
  imports: [MatButtonModule, MatIconModule, MatFormFieldModule, MatSelectModule, TranslatePipe, PhotoImage],
  templateUrl: './photo-gallery.html',
  styleUrl: './photo-gallery.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PhotoGallery {
  private auth = inject(AuthService);
  private dialogs = inject(DialogService);
  private translate = inject(TranslationService);
  readonly photos = input.required<PhotoReference[]>();
  readonly entityType = input.required<PhotoEntityType>();
  readonly entityId = input.required<string>();
  readonly memberName = input.required<string>();
  readonly uploaded = output<void>();
  protected readonly purposes = Object.values(PhotoPurpose);
  protected purpose = signal(PhotoPurpose.Profile);
  protected opening = signal(false);
  protected visiblePhotos = computed(() => this.photos().filter(photo => photo.purpose === this.purpose()));
  protected canUpload = computed(() => {
    const user = this.auth.currentUser();
    return !!user && (user.userRole === UserRole.Admin ||
      (this.entityType() === 'catechists'
        ? user.catechistId === this.entityId()
        : user.eucharisticYouthId === this.entityId()));
  });

  protected async upload(): Promise<void> {
    this.opening.set(true);
    try {
      const result = await this.dialogs.open<PhotoUpload, PhotoUploadData, PhotoUploaded>(PhotoUpload, {
        width: '480px',
        data: { entityType: this.entityType(), entityId: this.entityId(), purpose: this.purpose() },
      });
      if (result) {
        this.purpose.set(result.purpose);
        this.uploaded.emit();
      }
    } finally {
      this.opening.set(false);
    }
  }

  protected async view(photo: PhotoReference): Promise<void> {
    await this.dialogs.open<PhotoViewerDialog, PhotoViewerData>(PhotoViewerDialog, {
      width: '960px',
      maxWidth: '95vw',
      data: { url: photo.url, alt: `${this.memberName()} - ${this.translate.t('photo.purpose.' + photo.purpose)}` },
    });
  }
}
