import { Injectable, computed, inject, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { catchError, of } from 'rxjs';
import { ApiService } from '../../core/services/api.service';
import { Catechist, CatechismClassCatechist, PaginatedResult, PatronSaint } from '../../shared/models';

function emptyPage<T>(): PaginatedResult<T> {
  return { items: [], total: 0, page: 1, pageSize: 50 };
}

export interface CatechistPayload {
  catechistId?: string | null;
  givenName: string | null;
  middleName: string | null;
  familyName: string | null;
  christName: string | null;
  dateOfBirth: string | null;
  isMale: boolean | null;
  address: string | null;
  phoneNumber: string | null;
  patronSaints: unknown[] | null;
}

export interface CatechistListParams {
  search: string;
  page: number;
  pageSize: number;
}

@Injectable()
export class CatechistService {
  private api = inject(ApiService);

  readonly listParams = signal<CatechistListParams>({ search: '', page: 1, pageSize: 25 });
  readonly listRes = rxResource<PaginatedResult<Catechist>, CatechistListParams>({
    params: () => this.listParams(),
    stream: ({ params }) =>
      this.api
        .list<Catechist>('/catechists', {
          page: params.page,
          pageSize: params.pageSize,
          search: params.search.trim() || undefined,
        })
        .pipe(catchError(() => of(emptyPage<Catechist>()))),
  });
  readonly catechists = computed(() => this.listRes.value() ?? emptyPage<Catechist>());
  readonly isListLoading = computed(() => this.listRes.isLoading());

  /** Signal drives which catechist detail/edit-form record is loaded; null = no active record */
  readonly detailId = signal<string | null>(null);
  readonly detailRes = rxResource<Catechist | null, string>({
    params: () => this.detailId() ?? '',
    stream: ({ params }) =>
      params ? this.api.get<Catechist>('/catechists/detail', { id: params }).pipe(catchError(() => of(null))) : of(null),
  });
  readonly detail = computed(() => this.detailRes.value() ?? null);

  readonly assignmentsActive = signal(false);
  readonly assignmentsRes = rxResource<PaginatedResult<CatechismClassCatechist>, string>({
    params: () => (this.assignmentsActive() && this.detailId() ? this.detailId()! : ''),
    stream: ({ params }) =>
      params
        ? this.api
            .list<CatechismClassCatechist>('/class-catechist', { catechistId: params })
            .pipe(catchError(() => of(emptyPage<CatechismClassCatechist>())))
        : of(emptyPage<CatechismClassCatechist>()),
  });
  readonly assignments = computed(() => this.assignmentsRes.value() ?? emptyPage<CatechismClassCatechist>());

  async save(payload: CatechistPayload, isEdit: boolean): Promise<void> {
    await (isEdit
      ? this.api.patchAsync('/catechists', payload)
      : this.api.postAsync('/catechists', payload));
    this.listRes.reload();
    if (this.detailId()) this.detailRes.reload();
  }

  async remove(catechistId: string): Promise<void> {
    await this.api.deleteAsync('/catechists', { catechistId });
    this.listRes.reload();
  }
}
