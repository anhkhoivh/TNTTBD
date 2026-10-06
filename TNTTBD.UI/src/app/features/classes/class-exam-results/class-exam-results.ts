import { Component, inject, signal, computed, effect, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormArray, Validators, FormGroup } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { NotificationService } from '../../../core/services/notification.service';
import { TranslatePipe } from '../../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../../shared/pipes/full-name.pipe';
import { FormErrorsDisplayComponent } from '../../../shared/components/form-errors-display.component';
import { TranslationService } from '../../../core/i18n/translation.service';
import { ClassExamResultsService } from './class-exam-results.service';

interface ResultValue {
  score: number | null;
  remark: string | null;
}

type SaveState = 'saved' | 'error';

@Component({
  selector: 'app-class-exam-results',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    ReactiveFormsModule,
    MatCardModule,
    MatTableModule,
    MatInputModule,
    MatFormFieldModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    TranslatePipe,
    FullNamePipe,
    FormErrorsDisplayComponent,
  ],
  templateUrl: './class-exam-results.html',
  styleUrl: './class-exam-results.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [ClassExamResultsService],
})
export class ClassExamResults {
  private route = inject(ActivatedRoute);
  private examResultsService = inject(ClassExamResultsService);
  private notify = inject(NotificationService);
  private fb = inject(FormBuilder);

  #translate = inject(TranslationService);

  protected loading = computed(() => this.examResultsService.examRes.isLoading());
  protected examTitle = computed(() => this.examResultsService.exam()?.title ?? '');
  protected writeStates = signal<Record<string, SaveState>>({});

  protected displayedColumns = ['name', 'score', 'remark'];

  protected resultsForm = this.fb.group({
    results: this.fb.array<FormGroup>([]),
  });

  get resultsArray() {
    return this.resultsForm.get('results') as FormArray;
  }

  private classId: string | null = null;
  private examId: string | null = null;
  private lastSaved = new Map<string, ResultValue>();
  private pendingValues = new Map<string, ResultValue>();
  private activeWrites = new Set<string>();

  constructor() {
    this.classId = this.route.snapshot.queryParamMap.get('classId');
    this.examId = this.route.snapshot.queryParamMap.get('examId');
    this.examResultsService.examId.set(this.examId);

    // Rebuild the results FormArray whenever a new exam loads (forms are imperative)
    effect(() => {
      const exam = this.examResultsService.exam();
      if (!exam) return;
      const resultsFA = this.fb.array<FormGroup>([]);
      this.lastSaved.clear();
      exam.examResults.forEach((r) => {
        this.lastSaved.set(r.catechismClassEucharisticYouthId, { score: r.score ?? null, remark: r.remark || null });
        resultsFA.push(
          this.fb.group({
            enrollmentId: this.fb.control(r.catechismClassEucharisticYouthId, { nonNullable: true }),
            fullName: this.fb.control(r.youthFullName),
            score: this.fb.control(r.score, [Validators.min(0), Validators.max(10)]),
            remark: this.fb.control(r.remark ?? '', [Validators.maxLength(500)]),
          }),
        );
      });
      this.resultsForm.setControl('results', resultsFA);
      this.writeStates.set({});
    });
  }

  protected onFieldBlur(row: FormGroup, event: FocusEvent): void {
    // Number inputs report value null for unparseable text; don't let that clear a saved score.
    if ((event.target as HTMLInputElement | null)?.validity?.badInput) {
      row.get('score')?.setErrors({ badInput: true });
      row.markAllAsTouched();
      return;
    }
    if (row.invalid) {
      row.markAllAsTouched();
      return;
    }

    const enrollmentId: string = row.get('enrollmentId')!.value;
    const value: ResultValue = {
      score: row.get('score')!.value ?? null,
      remark: row.get('remark')!.value || null,
    };
    const queued = this.pendingValues.get(enrollmentId) ?? this.lastSaved.get(enrollmentId);
    if (queued && queued.score === value.score && queued.remark === value.remark) return;

    this.pendingValues.set(enrollmentId, value);
    this.clearWriteState(enrollmentId);
    void this.flushEnrollment(enrollmentId);
  }

  protected saveState(enrollmentId: string): SaveState | undefined {
    return this.writeStates()[enrollmentId];
  }

  protected retry(row: FormGroup): void {
    const enrollmentId: string = row.get('enrollmentId')!.value;
    if (row.invalid || this.activeWrites.has(enrollmentId)) return;

    this.pendingValues.set(enrollmentId, {
      score: row.get('score')!.value ?? null,
      remark: row.get('remark')!.value || null,
    });
    this.clearWriteState(enrollmentId);
    void this.flushEnrollment(enrollmentId);
  }

  private async flushEnrollment(enrollmentId: string): Promise<void> {
    if (!this.examId || this.activeWrites.has(enrollmentId)) return;
    this.activeWrites.add(enrollmentId);

    try {
      while (this.pendingValues.has(enrollmentId)) {
        const value = this.pendingValues.get(enrollmentId)!;
        this.pendingValues.delete(enrollmentId);

        try {
          await this.examResultsService.saveOne(this.examId, {
            catechismClassEucharisticYouthId: enrollmentId,
            score: value.score,
            remark: value.remark,
          });
          this.lastSaved.set(enrollmentId, value);
        } catch {
          if (this.pendingValues.has(enrollmentId)) continue;

          this.setWriteState(enrollmentId, 'error');
          this.notify.error(this.#translate.t('exam.saveFailed'));
          return;
        }
      }

      this.setWriteState(enrollmentId, 'saved');
    } finally {
      this.activeWrites.delete(enrollmentId);
    }
  }

  private setWriteState(enrollmentId: string, state: SaveState): void {
    this.writeStates.update((states) => ({ ...states, [enrollmentId]: state }));
  }

  private clearWriteState(enrollmentId: string): void {
    this.writeStates.update((states) => {
      const next = { ...states };
      delete next[enrollmentId];
      return next;
    });
  }

  protected backParams() {
    return this.classId ? { id: this.classId } : {};
  }
}
