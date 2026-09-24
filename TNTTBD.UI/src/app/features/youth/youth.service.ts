import { Injectable, computed, inject, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { catchError, map, of } from 'rxjs';
import { ApiService } from '../../core/services/api.service';
import {
  CatechismClassEucharisticYouth,
  CatechistCurrentClass,
  EucharisticYouth,
  EucharisticYouthContact,
  PaginatedResult,
} from '../../shared/models';

function emptyPage<T>(): PaginatedResult<T> {
  return { items: [], total: 0, page: 1, pageSize: 50 };
}

export interface YouthListParams {
  search: string;
  page: number;
  pageSize: number;
  [key: string]: string | number;
}

export interface YouthPayload {
  eucharisticYouthId?: string | null;
  [key: string]: unknown;
}

@Injectable()
export class YouthService {
  private api = inject(ApiService);

  readonly listParams = signal<YouthListParams>({ search: '', page: 1, pageSize: 50 });
  readonly listRes = rxResource<PaginatedResult<EucharisticYouth>, YouthListParams>({
    params: () => this.listParams(),
    stream: ({ params }) =>
      this.api.list<EucharisticYouth>('/youths', params).pipe(catchError(() => of(emptyPage<EucharisticYouth>()))),
  });
  readonly list = computed(() => this.listRes.value() ?? emptyPage<EucharisticYouth>());

  readonly detailId = signal<string | null>(null);
  readonly detailRes = rxResource<EucharisticYouth | null, string>({
    params: () => this.detailId() ?? '',
    stream: ({ params }) =>
      params ? this.api.get<EucharisticYouth>('/youths/detail', { id: params }).pipe(catchError(() => of(null))) : of(null),
  });
  readonly detail = computed(() => this.detailRes.value() ?? null);

  readonly contactsActive = signal(false);
  readonly contactsRes = rxResource<PaginatedResult<EucharisticYouthContact>, string>({
    params: () => (this.contactsActive() && this.detailId() ? this.detailId()! : ''),
    stream: ({ params }) =>
      params
        ? this.api.get<EucharisticYouthContact[]>('/youths/contacts', { youthId: params }).pipe(
            map((items) => ({ items, total: items.length, page: 1, pageSize: items.length })),
            catchError(() => of(emptyPage<EucharisticYouthContact>())),
          )
        : of(emptyPage<EucharisticYouthContact>()),
  });
  readonly contacts = computed(() => this.contactsRes.value() ?? emptyPage<EucharisticYouthContact>());

  readonly historyActive = signal(false);
  readonly historyRes = rxResource<PaginatedResult<CatechismClassEucharisticYouth>, string>({
    params: () => (this.historyActive() && this.detailId() ? this.detailId()! : ''),
    stream: ({ params }) =>
      params
        ? this.api.get<CatechismClassEucharisticYouth[]>('/youths/enrollments', { youthId: params }).pipe(
            map((items) => ({ items, total: items.length, page: 1, pageSize: items.length })),
            catchError(() => of(emptyPage<CatechismClassEucharisticYouth>())),
          )
        : of(emptyPage<CatechismClassEucharisticYouth>()),
  });
  readonly history = computed(() => this.historyRes.value() ?? emptyPage<CatechismClassEucharisticYouth>());

  loadCurrentClass(): Promise<CatechistCurrentClass | null> {
    return this.api.getAsync<CatechistCurrentClass | null>('/catechists/current-class');
  }

  async save(payload: YouthPayload, isEdit: boolean): Promise<void> {
    await (isEdit ? this.api.patchAsync('/youths', payload) : this.api.postAsync('/youths', payload));
    this.listRes.reload();
    if (this.detailId()) this.detailRes.reload();
  }

  async linkContact(payload: { eucharisticYouthId: string; contactId: string; relationType: string }): Promise<void> {
    await this.api.postAsync('/youths/contacts', payload);
    this.contactsRes.reload();
  }

  async unlinkContact(id: string): Promise<void> {
    await this.api.deleteAsync('/youths/contacts', { id });
    this.contactsRes.reload();
  }
}
