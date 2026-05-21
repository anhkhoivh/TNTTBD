import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { CatechismClass, CatechismYear } from '../../shared/models';
import { TranslatePipe } from '../../core/i18n/translate.pipe';

export interface ClassFormData {
  klass?: CatechismClass;
  years: CatechismYear[];
  defaultYearId?: string;
}

@Component({
  selector: 'app-class-form-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    TranslatePipe,
  ],
  template: `
    <h2 mat-dialog-title>{{ 'class.name' | t }}</h2>
    <form [formGroup]="form" (ngSubmit)="save()">
      <mat-dialog-content class="form">
        <mat-form-field appearance="outline">
          <mat-label>{{ 'class.year' | t }}</mat-label>
          <mat-select formControlName="catechismYearId" required>
            @for (y of data.years; track y.id) {
              <mat-option [value]="y.id">{{ y.startDate }} – {{ y.endDate }}</mat-option>
            }
          </mat-select>
        </mat-form-field>
        <mat-form-field appearance="outline">
          <mat-label>{{ 'class.name' | t }}</mat-label>
          <input matInput formControlName="className" required />
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
      .form {
        display: flex;
        flex-direction: column;
        gap: 0.5rem;
        min-width: 360px;
      }
    `,
  ],
})
export class ClassFormDialog {
  protected ref = inject(MatDialogRef<ClassFormDialog>);
  private fb = inject(FormBuilder);
  protected data = inject<ClassFormData>(MAT_DIALOG_DATA);

  protected form = this.fb.nonNullable.group({
    id: [this.data.klass?.id ?? ''],
    catechismYearId: [
      this.data.klass?.catechismYearId ?? this.data.defaultYearId ?? '',
      Validators.required,
    ],
    className: [this.data.klass?.className ?? '', Validators.required],
  });

  protected save(): void {
    const v = this.form.getRawValue();
    this.ref.close({ ...v, id: v.id || undefined });
  }
}
