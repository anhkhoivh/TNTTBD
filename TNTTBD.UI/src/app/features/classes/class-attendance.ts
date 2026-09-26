import { Component, inject, signal, computed, effect, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AuthService } from '../../core/auth/auth.service';
import { NotificationService } from '../../core/services/notification.service';
import { AttendanceStatus } from '../../shared/enums';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../shared/pipes/full-name.pipe';
import { TranslationService } from '../../core/i18n/translation.service';
import { ClassAttendanceService } from './class-attendance.service';

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
  providers: [ClassAttendanceService],
})
export class ClassAttendance {
  private route = inject(ActivatedRoute);
  private attendanceService = inject(ClassAttendanceService);
  private auth = inject(AuthService);
  private notify = inject(NotificationService);

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
  protected saving = signal(false);
  protected rows = signal<Row[]>([]);
  protected presentCount = computed(() => this.rows().filter((r) => r.status === AttendanceStatus.Present).length);
  protected absentCount = computed(() => this.rows().filter((r) => r.status === AttendanceStatus.Absent).length);
  protected excusedCount = computed(() => this.rows().filter((r) => r.status === AttendanceStatus.Excused).length);
  #translate = inject(TranslationService);

  private classId: string | null = null;
  private sessionId: string | null = null;

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

  protected setStatus(index: number, status: AttendanceStatus): void {
    const updated = [...this.rows()];
    updated[index] = { ...updated[index], status };
    this.rows.set(updated);
  }

  protected async save(): Promise<void> {
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
    try {
      await this.attendanceService.save(payload);
      this.notify.success(this.#translate.t('common.save'));
    } finally {
      this.saving.set(false);
    }
  }

  protected backParams() {
    return this.classId ? { id: this.classId } : {};
  }
}

