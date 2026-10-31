import { Component, inject, ChangeDetectionStrategy } from '@angular/core';
import { MAT_BOTTOM_SHEET_DATA, MatBottomSheetRef } from '@angular/material/bottom-sheet';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { TranslatePipe } from '../../../core/i18n/translate.pipe';

export type RowAction = 'view' | 'edit' | 'delete';

export interface RowExtraAction {
  /** Value returned when this action is chosen; must not collide with RowAction values. */
  key: string;
  icon: string;
  labelKey: string;
  warn?: boolean;
}

export interface RowActionsData {
  canView: boolean;
  canEdit: boolean;
  canDelete: boolean;
  viewLabelKey?: string;
  viewIcon?: string;
  deleteLabelKey?: string;
  deleteIcon?: string;
  /** Additional custom actions, rendered between Edit and Delete. */
  extraActions?: RowExtraAction[];
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
  private ref = inject<MatBottomSheetRef<RowActionsBottomSheet, string>>(MatBottomSheetRef);
  protected data = inject<RowActionsData>(MAT_BOTTOM_SHEET_DATA);

  protected choose(action: RowAction | string): void {
    this.ref.dismiss(action);
  }
}
