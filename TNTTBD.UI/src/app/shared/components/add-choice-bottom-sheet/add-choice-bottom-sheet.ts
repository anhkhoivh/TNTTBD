import { Component, inject, ChangeDetectionStrategy } from '@angular/core';
import { MAT_BOTTOM_SHEET_DATA, MatBottomSheetRef } from '@angular/material/bottom-sheet';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { TranslatePipe } from '../../../core/i18n/translate.pipe';

export type AddChoice = 'new' | 'existing';

export interface AddChoiceData {
  titleKey: string;
  addNewLabelKey: string;
  addNewIcon: string;
  linkExistingLabelKey: string;
  linkExistingIcon: string;
}

@Component({
  selector: 'app-add-choice-bottom-sheet',
  standalone: true,
  imports: [MatListModule, MatIconModule, TranslatePipe],
  templateUrl: './add-choice-bottom-sheet.html',
  styleUrl: './add-choice-bottom-sheet.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AddChoiceBottomSheet {
  private ref = inject<MatBottomSheetRef<AddChoiceBottomSheet, AddChoice>>(MatBottomSheetRef);
  protected data = inject<AddChoiceData>(MAT_BOTTOM_SHEET_DATA);

  protected choose(choice: AddChoice): void {
    this.ref.dismiss(choice);
  }
}
