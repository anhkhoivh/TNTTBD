import { Injectable, computed, inject, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { catchError, map, of } from 'rxjs';
import { ApiService } from '../../../core/services/api.service';
import { CatechismYear, YouthReport } from '../../../shared/models';

export interface YouthReportParams {
  youthId: string | null;
  enrollmentId: string | null;
  yearId: string | null;
}

@Injectable()
export class YouthReportService {
  private api = inject(ApiService);

  readonly params = signal<YouthReportParams>({ youthId: null, enrollmentId: null, yearId: null });
  readonly reportRes = rxResource<YouthReport | null, YouthReportParams>({
    params: () => this.params(),
    stream: ({ params }) =>
      params.youthId
        ? this.api
            .get<YouthReport>(`/reports/youths/${params.youthId}`, {
              enrollmentId: params.enrollmentId ?? undefined,
              catechismYearId: params.enrollmentId ? undefined : (params.yearId ?? undefined),
            })
            .pipe(catchError(() => of(null)))
        : of(null),
  });
  readonly report = computed(() => this.reportRes.value() ?? null);

  readonly yearsRes = rxResource<CatechismYear[], void>({
    stream: () =>
      this.api.list<CatechismYear>('/catechism-years').pipe(
        map((page) => page.items),
        catchError(() => of([] as CatechismYear[])),
      ),
  });
  readonly years = computed(() => this.yearsRes.value() ?? []);
}
