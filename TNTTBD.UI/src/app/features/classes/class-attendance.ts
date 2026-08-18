import { Component, inject, signal, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ApiService } from '../../core/services/api.service';
import { AuthService } from '../../core/auth/auth.service';
import { NotificationService } from '../../core/services/notification.service';
import { CatechismSessionAttendance } from '../../shared/models';
import { AttendanceStatus } from '../../shared/enums';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../shared/pipes/full-name.pipe';

interface Row {
  enrollmentId: string;
  youthFullName: any;
  status: AttendanceStatus;
}

@Component({
  selector: 'app-class-attendance',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink,
    MatCardModule,
    MatTableModule,
    MatButtonToggleModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    TranslatePipe,
    FullNamePipe,
  ],
  templateUrl: './class-attendance.html',
  styleUrl: './class-attendance.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ClassAttendance {
  private route = inject(ActivatedRoute);
  private api = inject(ApiService);
  private auth = inject(AuthService);
  private notify = inject(NotificationService);
  private cdr = inject(ChangeDetectorRef);

  protected readonly statuses = [
    { v: AttendanceStatus.Present, icon: 'check_circle' },
    { v: AttendanceStatus.Absent, icon: 'cancel' },
    { v: AttendanceStatus.Excused, icon: 'event_busy' },
  ];

  protected loading = signal(true);
  protected saving = signal(false);
  protected rows = signal<Row[]>([]);

  private classId: string | null = null;
  private sessionId: string | null = null;

  constructor() {
    this.route.queryParamMap.subscribe((qp) => {
      this.classId = qp.get('classId');
      this.sessionId = qp.get('sessionId');
      if (!this.sessionId) {
        this.loading.set(false);
        return;
      }
      this.loadSheet();
    });
  }

  private loadSheet(): void {
    this.api
      .get<CatechismSessionAttendance[]>('/sessions/attendance-sheet', { sessionId: this.sessionId! })
      .subscribe((items) => {
        this.rows.set(
          (items ?? []).map((a) => ({
            enrollmentId: a.catechismClassEucharisticYouthId,
            youthFullName: a.youthFullName,
            status: a.attendanceStatus ?? AttendanceStatus.None,
          })),
        );
        this.loading.set(false);
        this.cdr.markForCheck();
      });
  }

  protected setStatus(index: number, status: AttendanceStatus): void {
    const updated = [...this.rows()];
    updated[index] = { ...updated[index], status };
    this.rows.set(updated);
  }

  protected save(): void {
    this.saving.set(true);
    const payload = {
      sessionId: this.sessionId,
      markedByCatechistId: this.auth.currentUser()?.catechistId,
      attendanceItems: this.rows()
        .filter((r) => r.status !== AttendanceStatus.None)
        .map((r) => ({
          catechismClassEucharisticYouthId: r.enrollmentId,
          attendanceStatus: r.status,
        })),
    };
    this.api.post('/sessions/attendances', payload).subscribe({
      next: () => {
        this.saving.set(false);
        this.notify.success('Saved');
      },
      error: () => {
        this.saving.set(false);
        this.notify.error('Save failed');
      },
    });
  }

  protected backParams() {
    return this.classId ? { id: this.classId } : {};
  }
}
