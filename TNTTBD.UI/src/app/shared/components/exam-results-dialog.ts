import { Component, inject, signal, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormArray, Validators, FormGroup } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { forkJoin, catchError, of } from 'rxjs';
import { ApiService } from '../../core/services/api.service';
import { NotificationService } from '../../core/services/notification.service';
import {
  CatechismClassEucharisticYouth,
  EucharisticYouth,
  Exam,
  PaginatedResult,
} from '../models';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { FullNamePipe } from '../pipes/full-name.pipe';
import { FormErrorsDisplayComponent } from './form-errors-display.component';

export interface ExamResultsDialogData {
  classId: string;
  examId: string;
}

interface ResultRow {
  enrollmentId: string;
  youth?: EucharisticYouth;
  score: number | null;
  isDone: boolean;
  remark: string;
}

function emptyPage<T>(): PaginatedResult<T> {
  return { items: [], total: 0, page: 1, pageSize: 50 };
}

@Component({
  selector: 'app-exam-results-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatTableModule,
    MatInputModule,
    MatFormFieldModule,
    MatCheckboxModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    TranslatePipe,
    FullNamePipe,
    FormErrorsDisplayComponent,
  ],
  template: `
    <h2 mat-dialog-title>{{ examTitle() }}</h2>
    <mat-dialog-content>
      @if (loading()) {
        <div class="center"><mat-spinner diameter="40"></mat-spinner></div>
      } @else {
        <form [formGroup]="resultsForm">
        <table mat-table [dataSource]="resultsArray.controls" class="full-width">
          <ng-container matColumnDef="name">
            <th mat-header-cell *matHeaderCellDef>{{ 'youth.fullName' | t }}</th>
            <td mat-cell *matCellDef="let row; let i = index">
              @if (row.value.youth) { {{ row.value.youth.fullName | fullName }} }
            </td>
          </ng-container>
          <ng-container matColumnDef="score">
            <th mat-header-cell *matHeaderCellDef>{{ 'exam.score' | t }}</th>
            <td mat-cell *matCellDef="let row; let i = index">
              <mat-form-field appearance="outline" class="score-field">
                <input
                  matInput
                  type="number"
                  formControlName="score"
                  min="0"
                  max="10"
                  step="0.5"
                />
                <app-form-errors-display [control]="resultsArray.at(i).get('score')" fieldName="Score"></app-form-errors-display>
              </mat-form-field>
            </td>
          </ng-container>
          <ng-container matColumnDef="done">
            <th mat-header-cell *matHeaderCellDef>{{ 'exam.done' | t }}</th>
            <td mat-cell *matCellDef="let row; let i = index">
              <mat-checkbox formControlName="isDone"></mat-checkbox>
            </td>
          </ng-container>
          <ng-container matColumnDef="remark">
            <th mat-header-cell *matHeaderCellDef>{{ 'exam.remark' | t }}</th>
            <td mat-cell *matCellDef="let row; let i = index">
              <mat-form-field appearance="outline" class="remark-field">
                <input
                  matInput
                  formControlName="remark"
                  maxlength="500"
                />
                <app-form-errors-display [control]="resultsArray.at(i).get('remark')" fieldName="Remark"></app-form-errors-display>
              </mat-form-field>
            </td>
          </ng-container>
          <tr mat-header-row *matHeaderRowDef="['name', 'score', 'done', 'remark']"></tr>
          <tr mat-row *matRowDef="let row; columns: ['name', 'score', 'done', 'remark']; let i = index" [formGroupName]="i"></tr>
        </table>
        </form>
      }
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button (click)="ref.close()">{{ 'common.cancel' | t }}</button>
      <button mat-flat-button color="primary" (click)="save()" [disabled]="saving()">
        {{ 'common.save' | t }}
      </button>
    </mat-dialog-actions>
  `,
  styles: [
    `.center { display: flex; justify-content: center; padding: 2rem; }`,
    `.score-field { width: 80px; }`,
    `.remark-field { width: 160px; }`,
  ],

  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ExamResultsDialog implements OnInit {
  private api = inject(ApiService);
  private notify = inject(NotificationService);
  protected ref = inject(MatDialogRef<ExamResultsDialog>);
  private data: ExamResultsDialogData = inject(MAT_DIALOG_DATA);

  protected loading = signal(true);
  protected saving = signal(false);
  protected examTitle = signal('');
  private fb = inject(FormBuilder);
  protected resultsForm = this.fb.group({
    results: this.fb.array<FormGroup>([]),
  });
  get resultsArray() {
    return this.resultsForm.get('results') as FormArray;
  }

  ngOnInit(): void {
    forkJoin({
      enrollments: this.api
        .list<CatechismClassEucharisticYouth>('/class-youth', { classId: this.data.classId })
        .pipe(catchError(() => of(emptyPage<CatechismClassEucharisticYouth>()))),
      exam: this.api
        .get<Exam>('/exams/detail', { id: this.data.examId })
        .pipe(catchError(() => of(null))),
      youth: this.api
        .list<EucharisticYouth>('/youths')
        .pipe(catchError(() => of(emptyPage<EucharisticYouth>()))),
    }).subscribe(({ enrollments, exam, youth }) => {
      this.examTitle.set(exam?.title ?? '');
      const resultsFA = this.fb.array<FormGroup>([]);
      enrollments.items.forEach((e) => {
        const existing = exam?.examResults?.find(
          (r) => r.catechismClassEucharisticYouthId === e.id,
        );
        resultsFA.push(
          this.fb.group({
            enrollmentId: this.fb.control(e.id, { nonNullable: true }),
            youth: this.fb.control(
              youth.items.find(y => y.id === e.eucharisticYouthId)
            ),
            score: this.fb.control(existing?.score ?? null, [
              Validators.required,
              Validators.min(0),
              Validators.max(10)
            ]),
            isDone: this.fb.control(existing?.isDone ?? false, { nonNullable: true }),
            remark: this.fb.control(existing?.remark ?? '', [
              Validators.maxLength(500)
            ]),
          })
        );
      });
      this.resultsForm.setControl('results', resultsFA);
      this.loading.set(false);
    });
  }



  protected save(): void {
    if (this.resultsForm.invalid) {
      this.resultsForm.markAllAsTouched();
      return;
    }
    this.saving.set(true);
    const payload = {
      examId: this.data.examId,
      results: this.resultsArray.value.map((r: any) => ({
        catechismClassEucharisticYouthId: r.enrollmentId,
        score: r.score,
        isDone: r.isDone,
        remark: r.remark || null,
      })),
    };
    this.api.post('/exams/results', payload).subscribe({
      next: () => {
        this.saving.set(false);
        this.notify.success('Saved');
        this.ref.close(true);
      },
      error: () => {
        this.saving.set(false);
        this.notify.error('Save failed');
      },
    });
  }
}
