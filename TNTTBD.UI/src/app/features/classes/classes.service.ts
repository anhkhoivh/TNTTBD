import { Injectable, computed, inject, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { catchError, map, of } from 'rxjs';
import { ApiService } from '../../core/services/api.service';
import {
  CatechismClass,
  CatechismClassCatechist,
  CatechismClassEucharisticYouth,
  CatechismYear,
} from '../../shared/models';
import { EnrollmentStatus } from '../../shared/enums';

@Injectable()
export class ClassesService {
  private api = inject(ApiService);

  readonly yearsRes = rxResource<CatechismYear[], void>({
    stream: () => this.api.list<CatechismYear>('/catechism-years').pipe(map((r) => r.items)),
  });
  readonly years = computed(() => this.yearsRes.value() ?? []);

  readonly classesRes = rxResource<CatechismClass[], void>({
    stream: () => this.api.list<CatechismClass>('/classes').pipe(map((r) => r.items)),
  });
  readonly classes = computed(() => this.classesRes.value() ?? []);

  readonly classCatechistsRes = rxResource<CatechismClassCatechist[], void>({
    stream: () => this.api.list<CatechismClassCatechist>('/class-catechist').pipe(map((r) => r.items)),
  });
  readonly classCatechists = computed(() => this.classCatechistsRes.value() ?? []);

  async createYear(payload: unknown): Promise<void> {
    await this.api.postAsync('/catechism-years', payload);
    this.yearsRes.reload();
  }

  async createClass(payload: unknown): Promise<void> {
    await this.api.postAsync('/classes', payload);
    this.classesRes.reload();
  }

  async updateClass(payload: unknown): Promise<void> {
    await this.api.patchAsync('/classes', payload);
    this.classesRes.reload();
  }

  async deleteClass(catechismClassId: string): Promise<void> {
    await this.api.deleteAsync('/classes', { catechismClassId });
    this.classesRes.reload();
  }

  // --- Migration-specific active-enrollment lookups, keyed by selected class id ---
  readonly srcClassId = signal<string | null>(null);
  readonly srcYouthRes = rxResource<CatechismClassEucharisticYouth[], string>({
    params: () => this.srcClassId() ?? '',
    stream: ({ params }) =>
      params
        ? this.api.list<CatechismClassEucharisticYouth>('/class-youth', { classId: params }).pipe(
            map((r) => r.items.filter((e) => e.status === EnrollmentStatus.Active)),
            catchError(() => of([] as CatechismClassEucharisticYouth[])),
          )
        : of([] as CatechismClassEucharisticYouth[]),
  });
  readonly srcYouth = computed(() => this.srcYouthRes.value() ?? []);

  readonly dstClassId = signal<string | null>(null);
  readonly dstYouthRes = rxResource<CatechismClassEucharisticYouth[], string>({
    params: () => this.dstClassId() ?? '',
    stream: ({ params }) =>
      params
        ? this.api.list<CatechismClassEucharisticYouth>('/class-youth', { classId: params }).pipe(
            map((r) => r.items.filter((e) => e.status === EnrollmentStatus.Active)),
            catchError(() => of([] as CatechismClassEucharisticYouth[])),
          )
        : of([] as CatechismClassEucharisticYouth[]),
  });
  readonly dstYouth = computed(() => this.dstYouthRes.value() ?? []);

  async migrateYouth(payload: {
    sourceClassId: string;
    destinationClassId: string;
    eucharisticYouthIds: string[];
  }): Promise<{ migrated: number }> {
    const res = await this.api.postAsync<{ migrated: number }>('/class-youth/migrate', payload);
    this.srcYouthRes.reload();
    this.dstYouthRes.reload();
    return res;
  }
}
