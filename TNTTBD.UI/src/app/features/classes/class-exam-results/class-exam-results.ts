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
import { ClassExamResultsService } from './class-exam-results.service';

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

  protected loading = computed(() => this.examResultsService.examRes.isLoading());
  protected saving = signal(false);
  protected examTitle = computed(() => this.examResultsService.exam()?.title ?? '');

  protected displayedColumns = ['name', 'score', 'remark'];

  protected resultsForm = this.fb.group({
    results: this.fb.array<FormGroup>([]),
  });

  get resultsArray() {
    return this.resultsForm.get('results') as FormArray;
  }

  private classId: string | null = null;
  private examId: string | null = null;

  constructor() {
    this.classId = this.route.snapshot.queryParamMap.get('classId');
    this.examId = this.route.snapshot.queryParamMap.get('examId');
    this.examResultsService.examId.set(this.examId);

    // Rebuild the results FormArray whenever a new exam loads (forms are imperative)
    effect(() => {
      const exam = this.examResultsService.exam();
      if (!exam) return;
      const resultsFA = this.fb.array<FormGroup>([]);
      exam.examResults.forEach((r) => {
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
    });
  }

  protected async save(): Promise<void> {
    if (this.resultsForm.invalid) {
      this.resultsForm.markAllAsTouched();
      return;
    }
    this.saving.set(true);
    const payload = {
      examId: this.examId,
      results: this.resultsArray.value.map((r: any) => ({
        catechismClassEucharisticYouthId: r.enrollmentId,
        score: r.score,
        remark: r.remark || null,
      })),
    };
    try {
      await this.examResultsService.save(payload);
      this.notify.success('Saved');
    } catch {
      this.notify.error('Save failed');
    } finally {
      this.saving.set(false);
    }
  }

  protected backParams() {
    return this.classId ? { id: this.classId } : {};
  }
}
