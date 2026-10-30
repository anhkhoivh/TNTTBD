import { Component, inject, ChangeDetectionStrategy } from '@angular/core';
import { MAT_BOTTOM_SHEET_DATA, MatBottomSheetRef } from '@angular/material/bottom-sheet';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { TranslatePipe } from '../../../core/i18n/translate.pipe';

export type RowAction = 'view' | 'edit' | 'delete';

export interface RowActionsData {
  canView: boolean;
  canEdit: boolean;
  canDelete: boolean;
  viewLabelKey?: string;
  viewIcon?: string;
  deleteLabelKey?: string;
  deleteIcon?: string;
}

@Component({
  selector: 'app-row-actions-bottom-sheet',
  standalone: true,
  imports: [MatListModule, MatIconModule, TranslatePipe],
  templateUrl: './row-actions-bottom-sheet.html',
  styleUrl: './row-actions-bottom-sheet.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RowActionsBottomSheet {
  private ref = inject<MatBottomSheetRef<RowActionsBottomSheet, RowAction>>(MatBottomSheetRef);
  protected data = inject<RowActionsData>(MAT_BOTTOM_SHEET_DATA);

  protected choose(action: RowAction): void {
    this.ref.dismiss(action);
  }
}
