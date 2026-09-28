import { Injectable, computed, inject, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { of } from 'rxjs';
import { ApiService } from '../../core/services/api.service';
import { CatechismSessionAttendance } from '../../shared/models';
import { AttendanceStatus } from '../../shared/enums';

export interface AttendancePayload {
  sessionId: string | null;
  markedByCatechistId: string | null | undefined;
  attendanceItem: {
    catechismClassEucharisticYouthId: string;
    attendanceStatus: AttendanceStatus;
  };
}

@Injectable()
export class ClassAttendanceService {
  private api = inject(ApiService);

  readonly sessionId = signal<string | null>(null);
  readonly sheetRes = rxResource<CatechismSessionAttendance[], string>({
    params: () => this.sessionId() ?? '',
    stream: ({ params }) =>
      params
        ? this.api.get<CatechismSessionAttendance[]>('/sessions/attendance-sheet', { sessionId: params })
        : of([] as CatechismSessionAttendance[]),
  });
  readonly sheet = computed(() => this.sheetRes.value() ?? []);

  async saveOne(payload: AttendancePayload): Promise<void> {
    await this.api.postAsync('/sessions/attendance', payload);
  }
}
