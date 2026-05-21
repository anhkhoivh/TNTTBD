import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { toSignal } from '@angular/core/rxjs-interop';
import { forkJoin } from 'rxjs';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ApiService } from '../../core/services/api.service';
import { NotificationService } from '../../core/services/notification.service';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../shared/pipes/full-name.pipe';
import { DateLocalePipe } from '../../shared/pipes/date-locale.pipe';
import {
  CatechismClass,
  CatechismClassEucharisticYouth,
  EucharisticYouth,
  Exam,
} from '../../shared/models';

interface ResultRow {
  enrollmentId: string;
  youth?: EucharisticYouth;
  score: number | null;
  isDone: boolean;
  remark: string;
}

@Component({
  selector: 'app-exams',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatTableModule,
    MatCheckboxModule,
    MatButtonModule,
    MatIconModule,
    TranslatePipe,
    FullNamePipe,
    DateLocalePipe,
  ],
  templateUrl: './exams.html',
  styleUrl: './exams.scss',
})
export class ExamsPage {
  private route = inject(ActivatedRoute);
  private api = inject(ApiService);
  private notify = inject(NotificationService);

  protected classes = toSignal(this.api.list<CatechismClass>('/classes'), {
    initialValue: { items: [], total: 0, page: 1, pageSize: 50 },
  });

  protected selectedClassId = signal<string | null>(null);
  protected selectedExamId = signal<string | null>(null);
  protected exams = signal<Exam[]>([]);
  protected rows = signal<ResultRow[]>([]);

  constructor() {
    this.route.queryParamMap.subscribe((qp) => {
      const classId = qp.get('classId');
      const examId = qp.get('examId');
      if (classId) {
        this.selectedClassId.set(classId);
        this.loadExams(classId, examId);
      }
    });
  }

  protected onClassChange(id: string): void {
    this.selectedClassId.set(id);
    this.selectedExamId.set(null);
    this.rows.set([]);
    this.loadExams(id, null);
  }

  protected onExamChange(id: string): void {
    this.selectedExamId.set(id);
    this.loadRows();
  }

  private loadExams(classId: string, preselectExamId: string | null): void {
    this.api.list<Exam>('/exams', { classId: classId }).subscribe((r) => {
      this.exams.set(r.items);
      if (preselectExamId) {
        this.selectedExamId.set(preselectExamId);
        this.loadRows();
      }
    });
  }

  private loadRows(): void {
    const classId = this.selectedClassId();
    const examId = this.selectedExamId();
    if (!classId || !examId) return;
    forkJoin({
      enrollments: this.api.list<CatechismClassEucharisticYouth>(
        '/class-youth',
        { classId: classId },
      ),
      exam: this.api.get<Exam>('/exams/detail', { id: examId }),
      youth: this.api.list<EucharisticYouth>('/youths'),
    }).subscribe(({ enrollments, exam, youth }) => {
      const rows: ResultRow[] = enrollments.items.map((e) => {
        const existing = exam.examResults?.find(
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
    });
  }

  protected updateRow(enrollmentId: string, patch: Partial<ResultRow>): void {
    this.rows.update((rows) =>
      rows.map((r) => (r.enrollmentId === enrollmentId ? { ...r, ...patch } : r)),
    );
  }

  protected saveAll(): void {
    const examId = this.selectedExamId();
    if (!examId) return;
    const payload = {
      examId,
      results: this.rows().map((r) => ({
        catechismClassEucharisticYouthId: r.enrollmentId,
        score: r.score ?? 0,
        isDone: r.isDone,
        remark: r.remark || null,
      })),
    };
    this.api.post('/exams/results', payload).subscribe({
      next: () => this.notify.success('Saved'),
      error: () => this.notify.error('Save failed'),
    });
  }
}

