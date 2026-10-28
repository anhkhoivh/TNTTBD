import { ChangeDetectionStrategy, Component, computed, inject, input, output, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { catchError, map, of } from 'rxjs';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { DialogService } from '../../../core/services/dialog.service';
import { PhotoService } from '../../../core/services/photo.service';
import { TranslationService } from '../../../core/i18n/translation.service';
import { TranslatePipe } from '../../../core/i18n/translate.pipe';
import { PhotoPurpose } from '../../enums';
import { PhotoReference } from '../../models/youth.model';
import { PhotoImage } from '../photo-image/photo-image';
import { PhotoEntityType, PhotoUpload, PhotoUploadData, PhotoUploaded } from '../photo-upload/photo-upload';
import { PhotoViewerData, PhotoViewerDialog, PhotoViewerResult } from '../photo-viewer-dialog/photo-viewer-dialog';

@Component({
  selector: 'app-photo-gallery',
  standalone: true,
  imports: [MatButtonModule, MatIconModule, MatFormFieldModule, MatSelectModule, TranslatePipe, PhotoImage],
  templateUrl: './photo-gallery.html',
  styleUrl: './photo-gallery.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PhotoGallery {
  private dialogs = inject(DialogService);
  private photoService = inject(PhotoService);
  private translate = inject(TranslationService);
  readonly photos = input.required<PhotoReference[]>();
  readonly entityType = input.required<PhotoEntityType>();
  readonly entityId = input.required<string>();
  readonly memberName = input.required<string>();
  readonly changed = output<void>();
  protected readonly purposes = Object.values(PhotoPurpose);
  protected purpose = signal(PhotoPurpose.Profile);
  protected opening = signal(false);
  protected visiblePhotos = computed(() => this.photos().filter(photo => photo.purpose === this.purpose()));
  private permissions = rxResource({
    params: () => ({ entityType: this.entityType(), entityId: this.entityId() }),
    stream: ({ params }) =>
      params.entityId
        ? this.photoService.permissions(params.entityType, params.entityId).pipe(
            map(result => result.canManage),
            catchError(() => of(false)),
          )
        : of(false),
  });
  protected canManage = computed(() => this.permissions.value() ?? false);

  protected async upload(): Promise<void> {
    this.opening.set(true);
    try {
      const result = await this.dialogs.open<PhotoUpload, PhotoUploadData, PhotoUploaded>(PhotoUpload, {
        width: '480px',
        data: { entityType: this.entityType(), entityId: this.entityId(), purpose: this.purpose() },
      });
      if (result) {
        this.purpose.set(result.purpose);
        this.changed.emit();
      }
    } finally {
      this.opening.set(false);
    }
  }

  protected async view(photo: PhotoReference): Promise<void> {
    const result = await this.dialogs.open<PhotoViewerDialog, PhotoViewerData, PhotoViewerResult>(PhotoViewerDialog, {
      width: '960px',
      maxWidth: '95vw',
      data: {
        photo,
        alt: `${this.memberName()} - ${this.translate.t('photo.purpose.' + photo.purpose)}`,
        entityType: this.entityType(),
        entityId: this.entityId(),
        canManage: this.canManage(),
      },
    });
    if (!result) return;
    if (result.action === 'updated') this.purpose.set(result.purpose);
    this.changed.emit();
  }
}
