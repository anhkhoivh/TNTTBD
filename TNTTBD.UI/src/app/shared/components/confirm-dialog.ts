import { Component, inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { TranslatePipe } from '../../core/i18n/translate.pipe';

export interface ConfirmData {
  title?: string;
  message?: string;
  confirmKey?: string;
  cancelKey?: string;
  destructive?: boolean;
}

@Component({
  selector: 'app-confirm-dialog',
  standalone: true,
  imports: [MatDialogModule, MatButtonModule, TranslatePipe],
  template: `
    <h2 mat-dialog-title>{{ data.title ?? ('common.confirm' | t) }}</h2>
    <mat-dialog-content>
      <p>{{ data.message }}</p>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button (click)="ref.close(false)">
        {{ (data.cancelKey ?? 'common.cancel') | t }}
      </button>
      <button
        mat-flat-button
        [color]="data.destructive ? 'warn' : 'primary'"
        (click)="ref.close(true)"
      >
        {{ (data.confirmKey ?? 'common.confirm') | t }}
      </button>
    </mat-dialog-actions>
  `,
})
export class ConfirmDialog {
  protected ref = inject(MatDialogRef<ConfirmDialog, boolean>);
  protected data = inject<ConfirmData>(MAT_DIALOG_DATA);
}
