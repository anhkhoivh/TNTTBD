import { Injectable, computed, inject, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { catchError, of } from 'rxjs';
import { ApiService } from '../../core/services/api.service';
import {
  IsoDate,
  MilestoneEventCandidate,
  MilestoneEventDetail,
  MilestoneEventSummary,
  PaginatedResult,
  SaveMilestoneEventPayload,
} from '../../shared/models';
import { MilestoneType } from '../../shared/enums';

export interface MilestoneEventListParams {
  search: string;
  type: MilestoneType | '';
  from: IsoDate | '';
  to: IsoDate | '';
  page: number;
  pageSize: number;
}

export interface CandidateQuery {
  type: MilestoneType;
  search?: string;
  classId?: string | null;
  catechismYearId?: string | null;
  /** Event date, used to check prerequisite dates. */
  date?: IsoDate | null;
  page: number;
  pageSize: number;
}

function emptyPage<T>(): PaginatedResult<T> {
  return { items: [], total: 0, page: 1, pageSize: 25 };
}

@Injectable()
export class MilestoneEventsService {
  private api = inject(ApiService);

  readonly listParams = signal<MilestoneEventListParams>({ search: '', type: '', from: '', to: '', page: 1, pageSize: 25 });
  readonly listRes = rxResource<PaginatedResult<MilestoneEventSummary>, MilestoneEventListParams>({
    params: () => this.listParams(),
    stream: ({ params }) =>
      this.api
        .list<MilestoneEventSummary>('/milestone-events', {
          search: params.search || undefined,
          type: params.type || undefined,
          from: params.from || undefined,
          to: params.to || undefined,
          page: params.page,
          pageSize: params.pageSize,
        })
        .pipe(catchError(() => of(emptyPage<MilestoneEventSummary>()))),
  });
  readonly list = computed(() => this.listRes.value() ?? emptyPage<MilestoneEventSummary>());
  readonly isListLoading = computed(() => this.listRes.isLoading());

  get(id: string): Promise<MilestoneEventDetail> {
    return this.api.getAsync<MilestoneEventDetail>(`/milestone-events/${id}`);
  }

  async save(payload: SaveMilestoneEventPayload): Promise<string> {
    const result = await this.api.postAsync<{ id: string }>('/milestone-events', payload);
    return result.id;
  }

  async remove(id: string): Promise<void> {
    await this.api.deleteAsync(`/milestone-events/${id}`, null);
    this.listRes.reload();
  }

  candidates(query: CandidateQuery): Promise<PaginatedResult<MilestoneEventCandidate>> {
    return this.api.getAsync<PaginatedResult<MilestoneEventCandidate>>('/milestone-events/candidates', {
      type: query.type,
      search: query.search || undefined,
      classId: query.classId || undefined,
      catechismYearId: query.catechismYearId || undefined,
      date: query.date || undefined,
      page: query.page,
      pageSize: query.pageSize,
    });
  }

  async generateCodes(template: string, startIndex: number, dates: IsoDate[]): Promise<string[]> {
    const result = await this.api.postAsync<{ codes: string[] }>('/milestone-events/generate-codes', {
      template,
      startIndex,
      dates,
    });
    return result.codes;
  }

  exportExcel(id: string, language: string): Promise<Blob> {
    return this.api.getBlobAsync(`/milestone-events/${id}/excel`, { language });
  }
}
