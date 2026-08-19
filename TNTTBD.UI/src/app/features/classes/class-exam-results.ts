import { Component, inject, signal, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormArray, Validators, FormGroup, FormControl } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ApiService } from '../../core/services/api.service';
import { NotificationService } from '../../core/services/notification.service';
import { Exam } from '../../shared/models';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../shared/pipes/full-name.pipe';
import { FormErrorsDisplayComponent } from '../../shared/components/form-errors-display.component';

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
    MatCheckboxModule,
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
})
export class ClassExamResults {
  private route = inject(ActivatedRoute);
  private api = inject(ApiService);
  private notify = inject(NotificationService);
  private cdr = inject(ChangeDetectorRef);
  private fb = inject(FormBuilder);

  protected loading = signal(true);
  protected saving = signal(false);
  protected examTitle = signal('');

  protected resultsForm = this.fb.group({
    results: this.fb.array<FormGroup>([]),
  });

  get resultsArray() {
    return this.resultsForm.get('results') as FormArray;
  }

  private classId: string | null = null;
  private examId: string | null = null;

  constructor() {
    this.route.queryParamMap.subscribe((qp) => {
      this.classId = qp.get('classId');
      this.examId = qp.get('examId');
      if (!this.examId) {
        this.loading.set(false);
        return;
      }
      this.loadExam();
    });
  }

  getScoreControl(i: number): FormControl {
    return this.resultsArray.at(i).get('score') as FormControl;
  }

  private loadExam(): void {
    this.api.get<Exam>('/exams/detail', { id: this.examId! }).subscribe((exam) => {
      if (!exam) {
        this.loading.set(false);
        return;
      }
      this.examTitle.set(exam.title);
      const resultsFA = this.fb.array<FormGroup>([]);

      exam.examResults.forEach((r) => {
        resultsFA.push(
          this.fb.group({
            enrollmentId: this.fb.control(r.catechismClassEucharisticYouthId, { nonNullable: true }),
            fullName: this.fb.control(r.youthFullName),
            score: this.fb.control(r.score, [
              Validators.min(0),
              Validators.max(10),
            ]),
            remark: this.fb.control(r.remark ?? '', [Validators.maxLength(500)]),
          }),
        );
      });
      this.resultsForm.setControl('results', resultsFA);
      this.loading.set(false);
      this.cdr.detectChanges();
    });
  }

  protected save(): void {
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
    this.api.post('/exams/results', payload).subscribe({
      next: () => {
        this.saving.set(false);
        this.notify.success('Saved');
      },
      error: () => {
        this.saving.set(false);
        this.notify.error('Save failed');
      },
    });
  }

  protected backParams() {
    return this.classId ? { id: this.classId } : {};
  }
}
