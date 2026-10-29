import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { toSignal } from '@angular/core/rxjs-interop';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { TranslatePipe } from '../../../core/i18n/translate.pipe';
import { TranslationService } from '../../../core/i18n/translation.service';
import { DialogService } from '../../../core/services/dialog.service';
import { NotificationService } from '../../../core/services/notification.service';
import { PhotoEntityType, PhotoService } from '../../../core/services/photo.service';
import { PhotoPurpose } from '../../enums';
import { PhotoReference } from '../../models/youth.model';
import { PhotoImage } from '../photo-image/photo-image';

export interface PhotoViewerData {
  photo: PhotoReference;
  alt: string;
  entityType: PhotoEntityType;
  entityId: string;
  canManage: boolean;
}

export type PhotoViewerResult =
  | { action: 'updated'; purpose: PhotoPurpose }
  | { action: 'removed' };

@Component({
  selector: 'app-photo-viewer-dialog',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatIconModule,
    MatSelectModule,
    TranslatePipe,
    PhotoImage,
  ],
  templateUrl: './photo-viewer-dialog.html',
  styleUrl: './photo-viewer-dialog.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PhotoViewerDialog {
  protected data = inject<PhotoViewerData>(MAT_DIALOG_DATA);
  private ref = inject<MatDialogRef<PhotoViewerDialog, PhotoViewerResult>>(MatDialogRef);
  private photos = inject(PhotoService);
  private dialogs = inject(DialogService);
  private notify = inject(NotificationService);
  private translate = inject(TranslationService);
  private fb = inject(FormBuilder);

  protected readonly purposes = Object.values(PhotoPurpose);
  protected form = this.fb.nonNullable.group({
    purpose: [this.data.photo.purpose, Validators.required],
  });
  protected busy = signal(false);
  private value = toSignal(this.form.valueChanges, { initialValue: this.form.getRawValue() });
  protected changed = computed(() => {
    const value = this.value();
    return value.purpose !== this.data.photo.purpose;
  });

  protected async save(): Promise<void> {
    if (!this.data.canManage || this.busy() || this.form.invalid || !this.changed()) return;
    const { purpose } = this.form.getRawValue();
    await this.run(async () => {
      await this.photos.updateSettings(this.data.entityType, this.data.entityId, this.data.photo.photoId, purpose);
      this.notify.success(this.translate.t('photo.updated'));
      this.ref.close({ action: 'updated', purpose });
    }, 'photo.updateFailed');
  }

  protected async remove(): Promise<void> {
    if (!this.data.canManage || this.busy()) return;
    this.ref.disableClose = true;
    const confirmed = await this.dialogs.confirmDelete(this.translate.t('photo.deleteConfirm'));
    this.ref.disableClose = false;
    if (!confirmed) return;
    await this.run(async () => {
      await this.photos.remove(this.data.entityType, this.data.entityId, this.data.photo.photoId);
      this.notify.success(this.translate.t('photo.deleted'));
      this.ref.close({ action: 'removed' });
    }, 'photo.deleteFailed');
  }

  private async run(action: () => Promise<void>, errorKey: string): Promise<void> {
    this.busy.set(true);
    this.form.disable({ emitEvent: false });
    this.ref.disableClose = true;
    try {
      await action();
    } catch {
      this.notify.error(this.translate.t(errorKey));
      this.busy.set(false);
      this.form.enable({ emitEvent: false });
      this.ref.disableClose = false;
    }
  }
}
