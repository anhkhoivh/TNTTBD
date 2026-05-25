import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
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
    FormsModule,
    MatDialogModule,
    MatTableModule,
    MatInputModule,
    MatFormFieldModule,
    MatCheckboxModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    TranslatePipe,
    FullNamePipe,
  ],
  template: `
    <h2 mat-dialog-title>{{ examTitle() }}</h2>
    <mat-dialog-content>
      @if (loading()) {
        <div class="center"><mat-spinner diameter="40"></mat-spinner></div>
      } @else {
        <table mat-table [dataSource]="rows()" class="full-width">
          <ng-container matColumnDef="name">
            <th mat-header-cell *matHeaderCellDef>{{ 'youth.fullName' | t }}</th>
            <td mat-cell *matCellDef="let row">
              @if (row.youth) { {{ row.youth.fullName | fullName }} }
            </td>
          </ng-container>
          <ng-container matColumnDef="score">
            <th mat-header-cell *matHeaderCellDef>{{ 'exam.score' | t }}</th>
            <td mat-cell *matCellDef="let row; let i = index">
              <mat-form-field appearance="outline" class="score-field">
                <input
                  matInput
                  type="number"
                  [ngModel]="row.score"
                  (ngModelChange)="updateRow(i, 'score', $event)"
                  min="0"
                  max="10"
                  step="0.5"
                />
              </mat-form-field>
            </td>
          </ng-container>
          <ng-container matColumnDef="done">
            <th mat-header-cell *matHeaderCellDef>{{ 'exam.done' | t }}</th>
            <td mat-cell *matCellDef="let row; let i = index">
              <mat-checkbox
                [ngModel]="row.isDone"
                (ngModelChange)="updateRow(i, 'isDone', $event)"
              ></mat-checkbox>
            </td>
          </ng-container>
          <ng-container matColumnDef="remark">
            <th mat-header-cell *matHeaderCellDef>{{ 'exam.remark' | t }}</th>
            <td mat-cell *matCellDef="let row; let i = index">
              <mat-form-field appearance="outline" class="remark-field">
                <input
                  matInput
                  [ngModel]="row.remark"
                  (ngModelChange)="updateRow(i, 'remark', $event)"
                />
              </mat-form-field>
            </td>
          </ng-container>
          <tr mat-header-row *matHeaderRowDef="['name', 'score', 'done', 'remark']"></tr>
          <tr mat-row *matRowDef="let row; columns: ['name', 'score', 'done', 'remark']"></tr>
        </table>
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
})
export class ExamResultsDialog implements OnInit {
  private api = inject(ApiService);
  private notify = inject(NotificationService);
  protected ref = inject(MatDialogRef<ExamResultsDialog>);
  private data: ExamResultsDialogData = inject(MAT_DIALOG_DATA);

  protected loading = signal(true);
  protected saving = signal(false);
  protected rows = signal<ResultRow[]>([]);
  protected examTitle = signal('');

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
      const rows: ResultRow[] = enrollments.items.map((e) => {
        const existing = exam?.examResults?.find(
          (r) => r.catechismClassEucharisticYouthId === e.id,
        );
        return {
          enrollmentId: e.id,
          youth: youth.items.find((y) => y.id === e.eucharisticYouthId),
          score: existing?.score ?? null,
          isDone: existing?.isDone ?? false,
          remark: existing?.remark ?? '',
        };
      });
      this.rows.set(rows);
      this.loading.set(false);
    });
  }

  protected updateRow(index: number, field: keyof ResultRow, value: unknown): void {
    const current = [...this.rows()];
    current[index] = { ...current[index], [field]: value };
    this.rows.set(current);
  }

  protected save(): void {
    this.saving.set(true);
    const payload = {
      examId: this.data.examId,
      results: this.rows().map((r) => ({
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
