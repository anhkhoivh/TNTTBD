import { Injectable, computed, inject, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { catchError, of } from 'rxjs';
import { ApiService } from '../../core/services/api.service';
import { CatechismSessionAttendance } from '../../shared/models';

export interface AttendancePayload {
  sessionId: string | null;
  markedByCatechistId: string | null | undefined;
  attendanceItems: { catechismClassEucharisticYouthId: string; attendanceStatus: unknown }[];
}

@Injectable()
export class ClassAttendanceService {
  private api = inject(ApiService);

  readonly sessionId = signal<string | null>(null);
  readonly sheetRes = rxResource<CatechismSessionAttendance[], string>({
    params: () => this.sessionId() ?? '',
    stream: ({ params }) =>
      params
        ? this.api
            .get<CatechismSessionAttendance[]>('/sessions/attendance-sheet', { sessionId: params })
            .pipe(catchError(() => of([] as CatechismSessionAttendance[])))
        : of([] as CatechismSessionAttendance[]),
  });
  readonly sheet = computed(() => this.sheetRes.value() ?? []);

  async save(payload: AttendancePayload): Promise<void> {
    await this.api.postAsync('/sessions/attendances', payload);
  }
}
