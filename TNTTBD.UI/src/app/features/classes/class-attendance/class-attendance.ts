import { Component, inject, signal, computed, effect, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AuthService } from '../../../core/auth/auth.service';
import { NotificationService } from '../../../core/services/notification.service';
import { AttendanceStatus } from '../../../shared/enums';
import { TranslatePipe } from '../../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../../shared/pipes/full-name.pipe';
import { TranslationService } from '../../../core/i18n/translation.service';
import { ClassAttendanceService } from './class-attendance.service';

interface Row {
  enrollmentId: string;
  youthFullName: any;
  status: AttendanceStatus;
}

type SaveState = 'saved' | 'error';

@Component({
  selector: 'app-class-attendance',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
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
  providers: [ClassAttendanceService],
})
export class ClassAttendance {
  private route = inject(ActivatedRoute);
  private attendanceService = inject(ClassAttendanceService);
  private auth = inject(AuthService);
  private notify = inject(NotificationService);
  #location = inject(Location);

  protected readonly statuses = [
    { v: AttendanceStatus.Present, icon: 'check_circle' },
    { v: AttendanceStatus.Absent, icon: 'cancel' },
    { v: AttendanceStatus.Excused, icon: 'event_busy' },
  ];

  protected readonly statusColors: Record<AttendanceStatus, string> = {
    [AttendanceStatus.Present]: '#81C784',
    [AttendanceStatus.Absent]: '#f66271',
    [AttendanceStatus.Excused]: 'orange',
    [AttendanceStatus.None]: ''
  };

  protected loading = computed(() => this.attendanceService.sheetRes.isLoading());
  protected loadError = computed(() => this.attendanceService.sheetRes.error());
  protected rows = signal<Row[]>([]);
  protected writeStates = signal<Record<string, SaveState>>({});
  protected presentCount = computed(() => this.rows().filter((r) => r.status === AttendanceStatus.Present).length);
  protected absentCount = computed(() => this.rows().filter((r) => r.status === AttendanceStatus.Absent).length);
  protected excusedCount = computed(() => this.rows().filter((r) => r.status === AttendanceStatus.Excused).length);
  #translate = inject(TranslationService);

  private classId: string | null = null;
  private sessionId: string | null = null;
  private pendingStatuses = new Map<string, AttendanceStatus>();
  private activeWrites = new Set<string>();

  constructor() {
    this.classId = this.route.snapshot.queryParamMap.get('classId');
    this.sessionId = this.route.snapshot.queryParamMap.get('sessionId');
    this.attendanceService.sessionId.set(this.sessionId);

    // Reset local editable rows whenever the loaded attendance sheet changes
    effect(() => {
      const items = this.attendanceService.sheet();
      this.rows.set(
        items.map((a) => ({
          enrollmentId: a.catechismClassEucharisticYouthId,
          youthFullName: a.youthFullName,
          status: a.attendanceStatus ?? AttendanceStatus.None,
        })),
      );
    });
  }

  protected setStatus(index: number, status: AttendanceStatus | null | undefined): void {
    if (status == null || status === AttendanceStatus.None || this.loadError()) return;

    const updated = [...this.rows()];
    const row = updated[index];
    if (!row || row.status === status) return;
    updated[index] = { ...updated[index], status };
    this.rows.set(updated);
    this.pendingStatuses.set(row.enrollmentId, status);
    this.clearWriteState(row.enrollmentId);
    void this.flushEnrollment(row.enrollmentId);
  }

  protected saveState(enrollmentId: string): SaveState | undefined {
    return this.writeStates()[enrollmentId];
  }

  protected retry(enrollmentId: string): void {
    const row = this.rows().find((item) => item.enrollmentId === enrollmentId);
    if (!row || row.status === AttendanceStatus.None || this.activeWrites.has(enrollmentId)) return;

    this.pendingStatuses.set(enrollmentId, row.status);
    this.clearWriteState(enrollmentId);
    void this.flushEnrollment(enrollmentId);
  }

  protected retryLoad(): void {
    this.attendanceService.sheetRes.reload();
  }

  private async flushEnrollment(enrollmentId: string): Promise<void> {
    if (this.activeWrites.has(enrollmentId)) return;
    this.activeWrites.add(enrollmentId);

    try {
      while (this.pendingStatuses.has(enrollmentId)) {
        const status = this.pendingStatuses.get(enrollmentId)!;
        this.pendingStatuses.delete(enrollmentId);

        try {
          await this.attendanceService.saveOne({
            sessionId: this.sessionId,
            markedByCatechistId: this.auth.currentUser()?.catechistId,
            attendanceItem: {
              catechismClassEucharisticYouthId: enrollmentId,
              attendanceStatus: status,
            },
          });
        } catch {
          if (this.pendingStatuses.has(enrollmentId)) continue;

          this.setWriteState(enrollmentId, 'error');
          this.notify.error(this.#translate.t('attendance.saveFailed'));
          return;
        }
      }

      this.setWriteState(enrollmentId, 'saved');
    } finally {
      this.activeWrites.delete(enrollmentId);
    }
  }

  private setWriteState(enrollmentId: string, state: SaveState): void {
    this.writeStates.update((states) => ({ ...states, [enrollmentId]: state }));
  }

  private clearWriteState(enrollmentId: string): void {
    this.writeStates.update((states) => {
      const next = { ...states };
      delete next[enrollmentId];
      return next;
    });
  }

  navigateBack(): void {
    this.#location.back();
  }
}

