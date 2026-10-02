import { Component, inject, signal, computed, effect, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { TranslatePipe } from '../../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../../shared/pipes/full-name.pipe';
import { DateLocalePipe } from '../../../shared/pipes/date-locale.pipe';
import { DataTable, ColumnDef, ColumnConfig } from '../../../shared/components/data-table';
import { AttendanceStatus, EnrollmentStatus } from '../../../shared/enums';
import { ATTENDANCE_STATUS_DISPLAY } from '../../../shared/constants/attendance-status-display';
import { YouthReportService } from './youth-report.service';

const ALL_CLASSES = 'all';

@Component({
  selector: 'app-youth-report',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatCardModule,
    MatFormFieldModule,
    MatSelectModule,
    MatIconModule,
    MatButtonModule,
    MatTooltipModule,
    MatProgressSpinnerModule,
    TranslatePipe,
    FullNamePipe,
    DateLocalePipe,
    DataTable,
    ColumnDef,
  ],
  templateUrl: './youth-report.html',
  styleUrl: './youth-report.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [YouthReportService],
})
export class YouthReportPage {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private reportService = inject(YouthReportService);
  #location = inject(Location);

  protected readonly ALL_CLASSES = ALL_CLASSES;
  protected readonly statusDisplay = ATTENDANCE_STATUS_DISPLAY;
  protected display(status: AttendanceStatus) {
    return ATTENDANCE_STATUS_DISPLAY[status];
  }
  protected readonly attendanceStatuses = [
    AttendanceStatus.Present,
    AttendanceStatus.Excused,
    AttendanceStatus.Absent,
    AttendanceStatus.None,
  ];

  protected report = this.reportService.report;
  protected years = this.reportService.years;
  protected loading = computed(() => this.reportService.reportRes.isLoading());

  protected isAll = computed(() => !this.report()?.selectedEnrollmentId);
  protected selectedEnrollment = computed(() => {
    const r = this.report();
    return r?.enrollments.find((e) => e.enrollmentId === r.selectedEnrollmentId) ?? null;
  });

  protected attendanceFilter = signal<AttendanceStatus[]>([]);

  protected statusCounts = computed<Record<AttendanceStatus, number>>(() => {
    const a = this.report()?.attendance;
    return {
      [AttendanceStatus.Present]: a?.present ?? 0,
      [AttendanceStatus.Excused]: a?.excused ?? 0,
      [AttendanceStatus.Absent]: a?.absent ?? 0,
      [AttendanceStatus.None]: a?.unmarked ?? 0,
    };
  });

  protected attendanceRows = computed(() => {
    const statuses = this.attendanceFilter();
    return (this.report()?.attendanceDetails ?? [])
      .filter((a) => statuses.length === 0 || statuses.includes(a.status))
      .map((a) => ({ ...a, statusRank: ATTENDANCE_STATUS_DISPLAY[a.status].rank }));
  });

  protected examRows = computed(() => this.report()?.examDetails ?? []);

  protected attendanceColumns = computed<ColumnConfig[]>(() => [
    { key: 'sessionDateTime', labelKey: 'common.date' },
    { key: 'sessionTitle', labelKey: 'session.title' },
    ...(this.isAll() ? [{ key: 'className', labelKey: 'class.name' }] : []),
    { key: 'statusRank', labelKey: 'common.status' },
  ]);

  protected examColumns = computed<ColumnConfig[]>(() => [
    { key: 'examDate', labelKey: 'exam.date' },
    { key: 'examTitle', labelKey: 'exam.title' },
    ...(this.isAll() ? [{ key: 'className', labelKey: 'class.name' }] : []),
    { key: 'score', labelKey: 'exam.score' },
    { key: 'remark', labelKey: 'exam.remark', sortable: false },
  ]);

  constructor() {
    const queryParamMap = toSignal(this.route.queryParamMap, { initialValue: this.route.snapshot.queryParamMap });
    effect(() => {
      const q = queryParamMap();
      this.reportService.params.set({
        youthId: q.get('youthId'),
        enrollmentId: q.get('enrollmentId'),
        yearId: q.get('yearId'),
      });
    });
  }

  protected selectEnrollment(value: string): void {
    const isAll = value === ALL_CLASSES;
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {
        enrollmentId: isAll ? null : value,
        yearId: isAll ? (this.report()?.catechismYearId ?? null) : null,
      },
      queryParamsHandling: 'merge',
      replaceUrl: true,
    });
  }

  protected selectYear(yearId: string): void {
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { enrollmentId: null, yearId },
      queryParamsHandling: 'merge',
      replaceUrl: true,
    });
  }

  protected enrollmentStatusKey(status: EnrollmentStatus): string {
    return `enrollment.status.${status}`;
  }

  protected navigateBack(): void {
    this.#location.back();
  }
}
