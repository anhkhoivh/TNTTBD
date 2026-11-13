import { Injectable, computed, inject, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { catchError, of } from 'rxjs';
import { ApiService } from '../../../core/services/api.service';
import {
  CatechismClass,
  CatechismClassCatechist,
  CatechismClassEucharisticYouth,
  CatechismSession,
  Exam,
  PaginatedResult,
} from '../../../shared/models';
import { EnrollmentStatus } from '../../../shared/enums';

function emptyPage<T>(): PaginatedResult<T> {
  return { items: [], total: 0, page: 1, pageSize: 50 };
}

@Injectable()
export class ClassDetailService {
  private api = inject(ApiService);

  readonly classId = signal<string | null>(null);
  readonly classRes = rxResource<CatechismClass | null, string>({
    params: () => this.classId() ?? '',
    stream: ({ params }) =>
      params ? this.api.get<CatechismClass>('/classes/detail', { id: params }).pipe(catchError(() => of(null))) : of(null),
  });
  readonly classDetail = computed(() => this.classRes.value() ?? null);

  readonly activeTabs = signal(new Set<number>([0]));

  readonly enrollmentsRes = rxResource<PaginatedResult<CatechismClassEucharisticYouth>, string>({
    // computed() so activeTabs changes don't restart (and abort) an identical request
    params: computed(() => (this.classId() && this.activeTabs().has(0) ? this.classId()! : '')),
    stream: ({ params }) =>
      params
        ? this.api
            .list<CatechismClassEucharisticYouth>('/class-youth', { classId: params })
            .pipe(catchError(() => of(emptyPage<CatechismClassEucharisticYouth>())))
        : of(emptyPage<CatechismClassEucharisticYouth>()),
  });
  readonly enrollments = computed(() => this.enrollmentsRes.value() ?? emptyPage<CatechismClassEucharisticYouth>());

  readonly catechistsRes = rxResource<PaginatedResult<CatechismClassCatechist>, string>({
    params: computed(() => (this.classId() && this.activeTabs().has(1) ? this.classId()! : '')),
    stream: ({ params }) =>
      params
        ? this.api
            .list<CatechismClassCatechist>('/class-catechist', { classId: params })
            .pipe(catchError(() => of(emptyPage<CatechismClassCatechist>())))
        : of(emptyPage<CatechismClassCatechist>()),
  });
  readonly catechists = computed(() => this.catechistsRes.value() ?? emptyPage<CatechismClassCatechist>());

  readonly sessionsRes = rxResource<PaginatedResult<CatechismSession>, string>({
    params: computed(() => (this.classId() && this.activeTabs().has(2) ? this.classId()! : '')),
    stream: ({ params }) =>
      params
        ? this.api
            .list<CatechismSession>('/sessions', { classId: params })
            .pipe(catchError(() => of(emptyPage<CatechismSession>())))
        : of(emptyPage<CatechismSession>()),
  });
  readonly sessions = computed(() => this.sessionsRes.value() ?? emptyPage<CatechismSession>());

  readonly examsRes = rxResource<PaginatedResult<Exam>, string>({
    params: computed(() => (this.classId() && this.activeTabs().has(3) ? this.classId()! : '')),
    stream: ({ params }) =>
      params ? this.api.list<Exam>('/exams', { classId: params }).pipe(catchError(() => of(emptyPage<Exam>()))) : of(emptyPage<Exam>()),
  });
  readonly exams = computed(() => this.examsRes.value() ?? emptyPage<Exam>());

  onTabChange(index: number): void {
    this.activeTabs.update((s) => {
      if (s.has(index)) return s;
      const ns = new Set(s);
      ns.add(index);
      return ns;
    });
  }

  async enrollYouth(payload: unknown): Promise<void> {
    await this.api.postAsync('/class-youth', payload);
    this.enrollmentsRes.reload();
  }

  async changeEnrollmentStatus(payload: unknown): Promise<void> {
    await this.api.patchAsync('/class-youth', payload);
    this.enrollmentsRes.reload();
    this.classRes.reload();
  }

  async removeEnrollment(enrollmentId: string): Promise<void> {
    await this.api.deleteAsync('/class-youth', { id: enrollmentId });
    this.enrollmentsRes.reload();
  }

  async assignCatechist(payload: unknown): Promise<void> {
    await this.api.postAsync('/class-catechist', payload);
    this.catechistsRes.reload();
  }

  async changeCatechistStatus(id: string, status: EnrollmentStatus): Promise<void> {
    await this.api.patchAsync('/class-catechist/status', { id, status });
    this.catechistsRes.reload();
  }

  async removeCatechist(id: string): Promise<void> {
    await this.api.deleteAsync('/class-catechist', { id });
    this.catechistsRes.reload();
  }

  async createSession(payload: unknown): Promise<void> {
    await this.api.postAsync('/sessions', payload);
    this.sessionsRes.reload();
  }

  async updateSession(payload: unknown): Promise<void> {
    await this.api.patchAsync('/sessions', payload);
    this.sessionsRes.reload();
  }

  async deleteSession(catechismSessionId: string): Promise<void> {
    await this.api.deleteAsync('/sessions', { catechismSessionId });
    this.sessionsRes.reload();
  }

  async createExam(payload: unknown): Promise<void> {
    await this.api.postAsync('/exams', payload);
    this.examsRes.reload();
  }

  async updateExam(payload: unknown): Promise<void> {
    await this.api.patchAsync('/exams', payload);
    this.examsRes.reload();
  }

  async deleteExam(id: string): Promise<void> {
    await this.api.deleteAsync('/exams', { id });
    this.examsRes.reload();
  }
}
