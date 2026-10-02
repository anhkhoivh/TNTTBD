import { Injectable, computed, inject, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { catchError, of } from 'rxjs';
import { ApiService } from '../../../core/services/api.service';
import { Exam } from '../../../shared/models';

export interface ExamResultsPayload {
  examId: string | null;
  results: { catechismClassEucharisticYouthId: string; score: number | null; remark: string | null }[];
}

@Injectable()
export class ClassExamResultsService {
  private api = inject(ApiService);

  readonly examId = signal<string | null>(null);
  readonly examRes = rxResource<Exam | null, string>({
    params: () => this.examId() ?? '',
    stream: ({ params }) =>
      params ? this.api.get<Exam>('/exams/detail', { id: params }).pipe(catchError(() => of(null))) : of(null),
  });
  readonly exam = computed(() => this.examRes.value() ?? null);

  async save(payload: ExamResultsPayload): Promise<void> {
    await this.api.postAsync('/exams/results', payload);
  }
}
