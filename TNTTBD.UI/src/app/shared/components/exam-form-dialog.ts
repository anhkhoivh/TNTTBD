import { Component, inject , ChangeDetectionStrategy} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatButtonModule } from '@angular/material/button';
import { Exam } from '../../shared/models';
import { TranslatePipe } from '../../core/i18n/translate.pipe';

export interface ExamFormData {
  classId: string;
  exam?: Exam;
}

@Component({
  selector: 'app-exam-form-dialog',
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
    <h2 mat-dialog-title>{{ 'exam.title' | t }}</h2>
    <form [formGroup]="form" (ngSubmit)="save()">
      <mat-dialog-content class="form">
        <mat-form-field appearance="outline">
          <mat-label>{{ 'exam.title' | t }}</mat-label>
          <input matInput formControlName="title" required />
        </mat-form-field>
        <mat-form-field appearance="outline">
          <mat-label>{{ 'exam.date' | t }}</mat-label>
          <input matInput [matDatepicker]="dp" formControlName="date" required />
          <mat-datepicker-toggle matIconSuffix [for]="dp"></mat-datepicker-toggle>
          <mat-datepicker #dp></mat-datepicker>
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

  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ExamFormDialog {
  protected ref = inject(MatDialogRef<ExamFormDialog>);
  private fb = inject(FormBuilder);
  protected data = inject<ExamFormData>(MAT_DIALOG_DATA);

  protected form = this.fb.nonNullable.group({
    id: [this.data.exam?.id ?? ''],
    title: [this.data.exam?.title ?? '', Validators.required],
    date: [this.data.exam?.examDate ? new Date(this.data.exam.examDate) : null, Validators.required],
  });

  protected save(): void {
    const v = this.form.getRawValue();
    const d = v.date as Date;
    const iso = `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}-${String(d.getDate()).padStart(2, '0')}`;
    this.ref.close({
      id: v.id || undefined,
      catechismClassId: this.data.classId,
      title: v.title,
      examDate: iso,
    });
  }
}
