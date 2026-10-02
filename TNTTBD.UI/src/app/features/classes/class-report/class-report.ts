import { Component, inject, signal, computed, effect, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule, Sort } from '@angular/material/sort';
import { MatTabsModule } from '@angular/material/tabs';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { TranslatePipe } from '../../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../../shared/pipes/full-name.pipe';
import { DateLocalePipe } from '../../../shared/pipes/date-locale.pipe';
import { AttendanceStatus, EnrollmentStatus } from '../../../shared/enums';
import { ClassReportRow } from '../../../shared/models';
import { ATTENDANCE_STATUS_DISPLAY } from '../../../shared/constants/attendance-status-display';
import { NotificationService } from '../../../core/services/notification.service';
import { TranslationService } from '../../../core/i18n/translation.service';
import { ClassReportService } from './class-report.service';

type SortValue = string | number | null | undefined;

@Component({
  selector: 'app-class-report',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatCardModule,
    MatTableModule,
    MatSortModule,
    MatTabsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatIconModule,
    MatButtonModule,
    MatTooltipModule,
    MatProgressSpinnerModule,
    TranslatePipe,
    FullNamePipe,
    DateLocalePipe,
  ],
  templateUrl: './class-report.html',
  styleUrl: './class-report.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [ClassReportService],
})
export class ClassReportPage {
  private route = inject(ActivatedRoute);
  private reportService = inject(ClassReportService);
  private fullName = new FullNamePipe();
  private notify = inject(NotificationService);
  private translation = inject(TranslationService);

  protected readonly statusDisplay = ATTENDANCE_STATUS_DISPLAY;
  protected display(status: AttendanceStatus) {
    return ATTENDANCE_STATUS_DISPLAY[status];
  }
  protected readonly EnrollmentStatus = EnrollmentStatus;
  protected readonly legend = [
    AttendanceStatus.Present,
    AttendanceStatus.Excused,
    AttendanceStatus.Absent,
    AttendanceStatus.None,
  ];
  protected readonly enrollmentStatuses = [
    EnrollmentStatus.Active,
    EnrollmentStatus.Pending,
    EnrollmentStatus.Suspended,
    EnrollmentStatus.Transferred,
    EnrollmentStatus.Completed,
  ];

  protected report = this.reportService.report;
  protected loading = computed(() => this.reportService.reportRes.isLoading());

  protected search = signal('');
  protected statusFilter = signal<EnrollmentStatus[]>([EnrollmentStatus.Active]);
  protected attendanceSort = signal<Sort>({ active: '', direction: '' });
  protected examSort = signal<Sort>({ active: '', direction: '' });

  private sessionIndex = computed(
    () => new Map((this.report()?.sessions ?? []).map((s, i) => [`s_${s.sessionId}`, i])),
  );
  private examIndex = computed(() => new Map((this.report()?.exams ?? []).map((e, i) => [`e_${e.examId}`, i])));

  protected attendanceColumns = computed(() => [
    'index',
    'name',
    ...(this.report()?.sessions ?? []).map((s) => `s_${s.sessionId}`),
    'present',
    'excused',
    'absent',
    'unmarked',
    'rate',
  ]);

  protected examColumns = computed(() => [
    'index',
    'name',
    ...(this.report()?.exams ?? []).map((e) => `e_${e.examId}`),
    'scored',
    'average',
  ]);

  protected filteredRows = computed(() => {
    const term = this.search().trim().toLowerCase();
    const statuses = this.statusFilter();
    return (this.report()?.rows ?? []).filter(
      (row) =>
        (statuses.length === 0 || statuses.includes(row.enrollmentStatus)) &&
        (!term ||
          this.nameOf(row).toLowerCase().includes(term) ||
          (row.youthCode ?? '').toLowerCase().includes(term)),
    );
  });

  protected attendanceRows = computed(() => this.sortRows(this.filteredRows(), this.attendanceSort()));
  protected examRows = computed(() => this.sortRows(this.filteredRows(), this.examSort()));

  protected sessionFooters = computed(() => {
    const rows = this.filteredRows();
    return (this.report()?.sessions ?? []).map((_, i) => {
      let present = 0;
      let counted = 0;
      for (const row of rows) {
        const cell = row.attendances[i];
        if (!cell?.isCounted) continue;
        counted++;
        if (cell.status === AttendanceStatus.Present) present++;
      }
      return { present, counted };
    });
  });

  protected attendanceTotals = computed(() => {
    const rows = this.filteredRows();
    const sum = (pick: (r: ClassReportRow) => number) => rows.reduce((acc, r) => acc + pick(r), 0);
    const counted = sum((r) => r.counted);
    const present = sum((r) => r.present);
    return {
      present,
      excused: sum((r) => r.excused),
      absent: sum((r) => r.absent),
      unmarked: sum((r) => r.unmarked),
      rate: counted === 0 ? null : (present / counted) * 100,
    };
  });

  protected examFooters = computed(() => {
    const rows = this.filteredRows();
    return (this.report()?.exams ?? []).map((_, i) => this.average(rows.map((r) => r.scores[i]?.score)));
  });

  protected examTotalAverage = computed(() => this.average(this.filteredRows().map((r) => r.averageScore)));
  protected exporting = signal(false);

  constructor() {
    const queryParamMap = toSignal(this.route.queryParamMap, { initialValue: this.route.snapshot.queryParamMap });
    effect(() => this.reportService.classId.set(queryParamMap().get('id')));
  }

  protected onSearch(event: Event): void {
    this.search.set((event.target as HTMLInputElement).value);
  }

  protected async exportExcel(): Promise<void> {
    const report = this.report();
    if (!report || this.exporting()) return;

    this.exporting.set(true);
    try {
      const blob = await this.reportService.exportExcel(
        report.classId,
        this.attendanceRows().map((row) => row.enrollmentId),
        this.examRows().map((row) => row.enrollmentId),
        this.translation.currentLang(),
      );
      const url = URL.createObjectURL(blob);
      const link = document.createElement('a');
      link.href = url;
      link.download = `${[report.classCode, report.className]
        .filter(Boolean)
        .join('-')
        .replace(/[<>:"/\\|?*]/g, '-')}.xlsx`;
      link.click();
      setTimeout(() => URL.revokeObjectURL(url), 1000);
    } catch {
      this.notify.error(this.translation.t('error.generic'));
    } finally {
      this.exporting.set(false);
    }
  }

  protected statusKey(status: EnrollmentStatus): string {
    return `enrollment.status.${status}`;
  }

  private nameOf(row: ClassReportRow): string {
    return this.fullName.transform(row.youthFullName);
  }

  private sortValue(row: ClassReportRow, key: string): SortValue {
    switch (key) {
      case 'name': {
        const n = row.youthFullName;
        return n ? [n.givenName, n.familyName, n.middleName].filter((p) => !!p).join(' ') : '';
      }
      case 'present':
      case 'excused':
      case 'absent':
      case 'unmarked':
      case 'scored':
        return row[key];
      case 'rate':
        return row.attendanceRate;
      case 'average':
        return row.averageScore;
    }
    const sessionIdx = this.sessionIndex().get(key);
    if (sessionIdx !== undefined) {
      const cell = row.attendances[sessionIdx];
      return cell ? ATTENDANCE_STATUS_DISPLAY[cell.status].rank : null;
    }
    const examIdx = this.examIndex().get(key);
    return examIdx !== undefined ? row.scores[examIdx]?.score : null;
  }

  private sortRows(rows: ClassReportRow[], sort: Sort): ClassReportRow[] {
    if (!sort.active || !sort.direction) return rows;
    const dir = sort.direction === 'asc' ? 1 : -1;
    return [...rows].sort((a, b) => {
      const av = this.sortValue(a, sort.active);
      const bv = this.sortValue(b, sort.active);
      const aEmpty = av == null || av === '';
      const bEmpty = bv == null || bv === '';
      // Empty values stay last in both directions.
      if (aEmpty || bEmpty) return aEmpty === bEmpty ? 0 : aEmpty ? 1 : -1;
      const cmp = typeof av === 'number' && typeof bv === 'number' ? av - bv : String(av).localeCompare(String(bv), 'vi');
      return cmp * dir;
    });
  }

  private average(values: (number | null | undefined)[]): number | null {
    const scored = values.filter((v): v is number => v != null);
    return scored.length === 0 ? null : scored.reduce((a, b) => a + b, 0) / scored.length;
  }
}
