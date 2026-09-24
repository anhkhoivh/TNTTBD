import { Injectable, computed, inject, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { catchError, of } from 'rxjs';
import { ApiService } from '../../core/services/api.service';
import { Contact, ContactDetail, PaginatedResult } from '../../shared/models';

function emptyPage<T>(): PaginatedResult<T> {
  return { items: [], total: 0, page: 1, pageSize: 50 };
}

export interface ContactListParams {
  search: string;
  page: number;
  pageSize: number;
  [key: string]: string | number;
}

export interface ContactPayload {
  id?: string | null;
  [key: string]: unknown;
}

@Injectable()
export class ContactService {
  private api = inject(ApiService);

  readonly listParams = signal<ContactListParams>({ search: '', page: 1, pageSize: 50 });
  readonly listRes = rxResource<PaginatedResult<Contact>, ContactListParams>({
    params: () => this.listParams(),
    stream: ({ params }) =>
      this.api.list<Contact>('/contacts', params).pipe(catchError(() => of(emptyPage<Contact>()))),
  });
  readonly list = computed(() => this.listRes.value() ?? emptyPage<Contact>());

  readonly detailId = signal<string | null>(null);
  readonly detailRes = rxResource<ContactDetail | null, string>({
    params: () => this.detailId() ?? '',
    stream: ({ params }) =>
      params ? this.api.get<ContactDetail>('/contacts/detail', { id: params }).pipe(catchError(() => of(null))) : of(null),
  });
  readonly detail = computed(() => this.detailRes.value() ?? null);

  async save(payload: ContactPayload, isEdit: boolean): Promise<void> {
    await (isEdit ? this.api.patchAsync('/contacts', payload) : this.api.postAsync('/contacts', payload));
    this.listRes.reload();
    if (this.detailId()) this.detailRes.reload();
  }

  async remove(id: string): Promise<void> {
    await this.api.deleteAsync('/contacts', { id });
    this.listRes.reload();
  }
}
