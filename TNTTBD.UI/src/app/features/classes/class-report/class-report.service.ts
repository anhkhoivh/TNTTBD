import { Injectable, computed, inject, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { catchError, of } from 'rxjs';
import { ApiService } from '../../../core/services/api.service';
import { ClassReport } from '../../../shared/models';

@Injectable()
export class ClassReportService {
  private api = inject(ApiService);

  readonly classId = signal<string | null>(null);
  readonly reportRes = rxResource<ClassReport | null, string>({
    params: () => this.classId() ?? '',
    stream: ({ params }) =>
      params ? this.api.get<ClassReport>(`/reports/classes/${params}`).pipe(catchError(() => of(null))) : of(null),
  });
  readonly report = computed(() => this.reportRes.value() ?? null);
}
