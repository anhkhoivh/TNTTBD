import { Component, inject , ChangeDetectionStrategy} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatButtonModule } from '@angular/material/button';
import { CatechismYear } from '../../shared/models';
import { TranslatePipe } from '../../core/i18n/translate.pipe';

export interface YearFormData {
  year?: CatechismYear;
}

@Component({
  selector: 'app-year-form-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatButtonModule,
    TranslatePipe,
  ],
  template: `
    <h2 mat-dialog-title>{{ 'class.year' | t }}</h2>
    <form [formGroup]="form" (ngSubmit)="save()">
      <mat-dialog-content class="form-grid">
        <mat-form-field appearance="outline">
          <mat-label>{{ 'class.startDate' | t }}</mat-label>
          <input matInput [matDatepicker]="sp" formControlName="startDate" required />
          <mat-datepicker-toggle matIconSuffix [for]="sp"></mat-datepicker-toggle>
          <mat-datepicker #sp></mat-datepicker>
        </mat-form-field>
        <mat-form-field appearance="outline">
          <mat-label>{{ 'class.endDate' | t }}</mat-label>
          <input matInput [matDatepicker]="ep" formControlName="endDate" required />
          <mat-datepicker-toggle matIconSuffix [for]="ep"></mat-datepicker-toggle>
          <mat-datepicker #ep></mat-datepicker>
        </mat-form-field>
        <mat-form-field appearance="outline" class="full">
          <mat-label>{{ 'class.theme' | t }}</mat-label>
          <input matInput formControlName="theme" />
        </mat-form-field>
      </mat-dialog-content>
      <mat-dialog-actions align="end">
        <button mat-button type="button" (click)="ref.close()">
          {{ 'common.cancel' | t }}
        </button>
        <button mat-flat-button color="primary" type="submit" [disabled]="form.invalid">
          {{ 'common.save' | t }}
        </button>
      </mat-dialog-actions>
    </form>
  `,
  styles: [
    `
      .form-grid {
        display: grid;
        grid-template-columns: 1fr 1fr;
        gap: 1rem;
        min-width: 360px;
      }
      .full {
        grid-column: 1 / -1;
      }
    `,
  ],

  changeDetection: ChangeDetectionStrategy.OnPush
})
export class YearFormDialog {
  protected ref = inject(MatDialogRef<YearFormDialog>);
  private fb = inject(FormBuilder);
  protected data = inject<YearFormData>(MAT_DIALOG_DATA);

  protected form = this.fb.nonNullable.group({
    id: [this.data.year?.id ?? ''],
    startDate: [this.toDate(this.data.year?.startDate), Validators.required],
    endDate: [this.toDate(this.data.year?.endDate), Validators.required],
    theme: [this.data.year?.theme ?? ''],
  });

  private toDate(s: string | undefined): Date | null {
    return s ? new Date(s) : null;
  }

  private toIsoDate(d: Date | null): string | null {
    if (!d) return null;
    const y = d.getFullYear();
    const m = String(d.getMonth() + 1).padStart(2, '0');
    const day = String(d.getDate()).padStart(2, '0');
    return `${y}-${m}-${day}`;
  }

  protected save(): void {
    const v = this.form.getRawValue();
    this.ref.close({
      id: v.id || undefined,
      startDate: this.toIsoDate(v.startDate),
      endDate: this.toIsoDate(v.endDate),
      theme: v.theme || null,
    });
  }
}
