import { Component, inject , ChangeDetectionStrategy} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { CatechismSession } from '../../shared/models';
import { TranslatePipe } from '../../core/i18n/translate.pipe';

export interface SessionFormData {
  classId: string;
  session?: CatechismSession;
}

@Component({
  selector: 'app-session-form-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    TranslatePipe,
  ],
  template: `
    <h2 mat-dialog-title>{{ 'session.title' | t }}</h2>
    <form [formGroup]="form" (ngSubmit)="save()">
      <mat-dialog-content class="form">
        <mat-form-field appearance="outline">
          <mat-label>{{ 'session.title' | t }}</mat-label>
          <input matInput formControlName="title" required />
        </mat-form-field>
        <div class="row">
          <mat-form-field appearance="outline">
            <mat-label>{{ 'common.date' | t }}</mat-label>
            <input matInput [matDatepicker]="dp" formControlName="date" required />
            <mat-datepicker-toggle matIconSuffix [for]="dp"></mat-datepicker-toggle>
            <mat-datepicker #dp></mat-datepicker>
          </mat-form-field>
          <mat-form-field appearance="outline">
            <mat-label>{{ 'common.time' | t }}</mat-label>
            <input matInput type="time" formControlName="time" required />
          </mat-form-field>
        </div>
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
      .row {
        display: grid;
        grid-template-columns: 1fr 1fr;
        gap: 1rem;
      }
    `,
  ],

  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SessionFormDialog {
  protected ref = inject(MatDialogRef<SessionFormDialog>);
  private fb = inject(FormBuilder);
  protected data = inject<SessionFormData>(MAT_DIALOG_DATA);

  protected form = this.fb.nonNullable.group({
    id: [this.data.session?.id ?? ''],
    title: [this.data.session?.title ?? '', Validators.required],
    date: [this.initDate(), Validators.required],
    time: [this.initTime(), Validators.required],
  });

  private initDate(): Date | null {
    const dt = this.data.session?.dateTime;
    return dt ? new Date(dt) : null;
  }

  private initTime(): string {
    const dt = this.data.session?.dateTime;
    if (!dt) return '08:00';
    const d = new Date(dt);
    return `${String(d.getHours()).padStart(2, '0')}:${String(d.getMinutes()).padStart(2, '0')}`;
  }

  protected save(): void {
    const v = this.form.getRawValue();
    const d = v.date as Date;
    const [hh, mm] = v.time.split(':').map(Number);
    d.setHours(hh, mm, 0, 0);
    this.ref.close({
      ...(v.id ? { catechismSessionId: v.id } : {}),
      catechismClassId: this.data.classId,
      title: v.title,
      dateTime: d.toISOString(),
    });
  }
}
