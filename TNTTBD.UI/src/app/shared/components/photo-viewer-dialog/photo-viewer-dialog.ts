import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { TranslatePipe } from '../../../core/i18n/translate.pipe';
import { PhotoImage } from '../photo-image/photo-image';

export interface PhotoViewerData {
  url: string;
  alt: string;
}

@Component({
  selector: 'app-photo-viewer-dialog',
  standalone: true,
  imports: [MatDialogModule, MatButtonModule, TranslatePipe, PhotoImage],
  templateUrl: './photo-viewer-dialog.html',
  styleUrl: './photo-viewer-dialog.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PhotoViewerDialog {
  protected data = inject<PhotoViewerData>(MAT_DIALOG_DATA);
}
