import { Component, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { toSignal } from '@angular/core/rxjs-interop';
import { switchMap, of, forkJoin } from 'rxjs';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ApiService } from '../../core/services/api.service';
import { NotificationService } from '../../core/services/notification.service';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../shared/pipes/full-name.pipe';
import { DateLocalePipe } from '../../shared/pipes/date-locale.pipe';
import {
  CatechismClass,
  CatechismClassEucharisticYouth,
  CatechismSession,
  CatechismSessionAttendance,
  EucharisticYouth,
} from '../../shared/models';
import { AttendanceStatus } from '../../shared/enums';

interface Row {
  enrollmentId: string;
  youth?: EucharisticYouth;
  status: AttendanceStatus;
}

@Component({
  selector: 'app-attendance',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatSelectModule,
    MatTableModule,
    MatButtonToggleModule,
    MatButtonModule,
    MatIconModule,
    TranslatePipe,
    FullNamePipe,
    DateLocalePipe,
  ],
  templateUrl: './attendance.html',
  styleUrl: './attendance.scss',
})
export class AttendancePage {
  private route = inject(ActivatedRoute);
  private api = inject(ApiService);
  private notify = inject(NotificationService);

  protected readonly AttendanceStatus = AttendanceStatus;
  protected readonly statuses = [
    { v: AttendanceStatus.Present, k: 'attendance.present', icon: 'check_circle' },
    { v: AttendanceStatus.Absent, k: 'attendance.absent', icon: 'cancel' },
    { v: AttendanceStatus.Excused, k: 'attendance.excused', icon: 'event_busy' },
  ];

  protected classes = toSignal(this.api.list<CatechismClass>('/classes'), {
    initialValue: { items: [], total: 0, page: 1, pageSize: 50 },
  });

  protected selectedClassId = signal<string | null>(null);
  protected selectedSessionId = signal<string | null>(null);

  protected sessions = signal<CatechismSession[]>([]);
  protected rows = signal<Row[]>([]);

  constructor() {
    this.route.queryParamMap.subscribe((qp) => {
      const classId = qp.get('classId');
      const sessionId = qp.get('sessionId');
      if (classId) {
        this.selectedClassId.set(classId);
        this.loadSessions(classId, sessionId);
      }
    });
  }

  protected onClassChange(id: string): void {
    this.selectedClassId.set(id);
    this.selectedSessionId.set(null);
    this.rows.set([]);
    this.loadSessions(id, null);
  }

  protected onSessionChange(id: string): void {
    this.selectedSessionId.set(id);
    this.loadRows();
  }

  private loadSessions(classId: string, preselectSessionId: string | null): void {
    this.api
      .list<CatechismSession>('/sessions', { classId: classId })
      .subscribe((sessions) => {
        this.sessions.set(sessions.items);
        if (preselectSessionId) {
          this.selectedSessionId.set(preselectSessionId);
          this.loadRows();
        }
      });
  }

  private loadRows(): void {
    const classId = this.selectedClassId();
    const sessionId = this.selectedSessionId();
    if (!classId || !sessionId) return;
    forkJoin({
      enrollments: this.api.list<CatechismClassEucharisticYouth>(
        '/class-youth',
        { classId: classId },
      ),
      attendance: this.api.list<CatechismSessionAttendance>('/sessions/attendances', {
        sessionId: sessionId,
      }),
      youth: this.api.list<EucharisticYouth>('/youths'),
    }).subscribe(({ enrollments, attendance, youth }) => {
      const rows: Row[] = enrollments.items.map((e) => {
        const existing = attendance.items.find(
          (a) => a.catechismClassEucharisticYouthId === e.id,
        );
        return {
          enrollmentId: e.id,
          youth: youth.items.find((y) => y.id === e.eucharisticYouthId),
          status: existing?.attendanceStatus ?? AttendanceStatus.None,
        };
      });
      this.rows.set(rows);
    });
  }

  protected setStatus(enrollmentId: string, status: AttendanceStatus): void {
    this.rows.update((rows) =>
      rows.map((r) => (r.enrollmentId === enrollmentId ? { ...r, status } : r)),
    );
  }

  protected saveAll(): void {
    const sessionId = this.selectedSessionId();
    if (!sessionId) return;
    const payload = {
      catechismSessionId: sessionId,
      entries: this.rows().map((r) => ({
        catechismClassEucharisticYouthId: r.enrollmentId,
        attendanceStatus: r.status,
      })),
    };
    this.api.post('/sessions/attendances', payload).subscribe({
      next: () => this.notify.success('Saved'),
      error: () => this.notify.error('Save failed'),
    });
  }
}

