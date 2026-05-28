import { Component, inject, signal, computed, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { rxResource, toSignal } from '@angular/core/rxjs-interop';
import { switchMap, of, forkJoin, catchError } from 'rxjs';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import { MatChipsModule } from '@angular/material/chips';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ApiService } from '../../core/services/api.service';
import { NotificationService } from '../../core/services/notification.service';
import { AuthService } from '../../core/auth/auth.service';
import {
  CatechismClass,
  CatechismClassCatechist,
  CatechismClassEucharisticYouth,
  CatechismSession,
  Catechist,
  EucharisticYouth,
  Exam,
  PaginatedResult,
} from '../../shared/models';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../shared/pipes/full-name.pipe';
import { DateLocalePipe } from '../../shared/pipes/date-locale.pipe';
import { EnrollmentStatus, CatechismClassCatechistRole, UserRole } from '../../shared/enums';
import { SessionFormDialog } from '../../shared/components/session-form-dialog';
import { ExamFormDialog } from '../../shared/components/exam-form-dialog';
import { ConfirmDialog } from '../../shared/components/confirm-dialog';
import { AttendanceDialog } from '../../shared/components/attendance-dialog';
import { ExamResultsDialog } from '../../shared/components/exam-results-dialog';

function emptyPage<T>(): PaginatedResult<T> {
  return { items: [], total: 0, page: 1, pageSize: 50 };
}

@Component({
  selector: 'app-class-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatCardModule,
    MatTabsModule,
    MatTableModule,
    MatIconModule,
    MatButtonModule,
    MatListModule,
    MatChipsModule,
    MatDialogModule,
    TranslatePipe,
    FullNamePipe,
    DateLocalePipe,
  ],
  templateUrl: './class-detail.html',
  styleUrl: './class-detail.scss',

  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ClassDetail {
  private route = inject(ActivatedRoute);
  private api = inject(ApiService);
  private dialog = inject(MatDialog);
  private notify = inject(NotificationService);
  protected auth = inject(AuthService);

  protected readonly EnrollmentStatus = EnrollmentStatus;
  protected readonly CatechismClassCatechistRole = CatechismClassCatechistRole;
  protected readonly UserRole = UserRole;

  protected canManage(): boolean {
    return this.auth.hasRole([UserRole.Admin, UserRole.Catechist]);
  }

  protected get isContact(): boolean {
    return this.auth.currentUser()?.userRole === UserRole.EucharisticYouthContact;
  }

  /** Initial load: class info only */
  protected data = toSignal(
    this.route.queryParamMap.pipe(
      switchMap((qp) => {
        const id = qp.get('id');
        if (!id) return of(null);
        return this.api.get<CatechismClass>('/classes/detail', { id }).pipe(
          catchError(() => of(null)),
        );
      }),
    ),
    { initialValue: null },
  );

  /** Lazy-loaded tab data signals */
  protected activeTabs = signal(new Set<number>([0]));
  protected classId = computed(() => this.data()?.id ?? this.route.snapshot.queryParamMap.get('id'));

  protected enrollmentsRes = rxResource<{
    enrollments: PaginatedResult<CatechismClassEucharisticYouth>;
    youthLookup: PaginatedResult<EucharisticYouth>;
  }, string>({
    params: () => this.classId() && this.activeTabs().has(0) ? this.classId()! : '',
    stream: (loaderParams) => forkJoin({
      enrollments: this.api.list<CatechismClassEucharisticYouth>('/class-youth', { classId: loaderParams.params }).pipe(
        catchError(() => of(emptyPage<CatechismClassEucharisticYouth>())),
      ),
      youthLookup: this.api.list<EucharisticYouth>('/youths').pipe(
        catchError(() => of(emptyPage<EucharisticYouth>())),
      ),
    })
  });

  protected catechistsRes = rxResource<{
    catechists: PaginatedResult<CatechismClassCatechist>;
    catechistLookup: PaginatedResult<Catechist>;
  }, string>({
    params: () => this.classId() && this.activeTabs().has(1) ? this.classId()! : '',
    stream: (loaderParams) => forkJoin({
      catechists: this.api.list<CatechismClassCatechist>('/class-catechist', { classId: loaderParams.params }).pipe(
        catchError(() => of(emptyPage<CatechismClassCatechist>())),
      ),
      catechistLookup: this.api.list<Catechist>('/catechists').pipe(
        catchError(() => of(emptyPage<Catechist>())),
      ),
    })
  });

  protected sessionsRes = rxResource<PaginatedResult<CatechismSession>, string>({
    params: () => this.classId() && this.activeTabs().has(2) ? this.classId()! : '',
    stream: (loaderParams) => this.api.list<CatechismSession>('/sessions', { classId: loaderParams.params }).pipe(
      catchError(() => of(emptyPage<CatechismSession>())),
    )
  });

  protected examsRes = rxResource<PaginatedResult<Exam>, string>({
    params: () => this.classId() && this.activeTabs().has(3) ? this.classId()! : '',
    stream: (loaderParams) => this.api.list<Exam>('/exams', { classId: loaderParams.params }).pipe(
      catchError(() => of(emptyPage<Exam>())),
    )
  });

  protected onTabChange(index: number): void {
    this.activeTabs.update(s => {
      const ns = new Set(s);
      ns.add(index);
      return ns;
    });
  }

  /** Auto-load first tab on init */
  ngOnInit(): void {
    this.onTabChange(0);
  }

  protected youthOfEnrollment(enrollmentId: string): EucharisticYouth | undefined {
    const ed = this.enrollmentsRes.value();
    if (!ed) return undefined;
    const e = ed.enrollments.items.find((x) => x.id === enrollmentId);
    if (!e) return undefined;
    return ed.youthLookup.items.find((y) => y.id === e.eucharisticYouthId);
  }

  protected catechistOf(id: string): Catechist | undefined {
    return this.catechistsRes.value()?.catechistLookup.items.find((c) => c.id === id);
  }

  protected statusKey(status: EnrollmentStatus): string {
    return `enrollment.status.${status}`;
  }

  protected roleKey(role: CatechismClassCatechistRole): string {
    return `catechistRole.${role}`;
  }

  protected newSession(classId: string): void {
    this.dialog
      .open(SessionFormDialog, { data: { classId } })
      .afterClosed()
      .subscribe((v) => {
        if (!v) return;
        this.api.post('/sessions', v).subscribe(() => {
          this.notify.success('Saved');
          this.sessionsRes.reload();
        });
      });
  }

  protected editSession(classId: string, session: CatechismSession): void {
    this.dialog
      .open(SessionFormDialog, { data: { classId, session } })
      .afterClosed()
      .subscribe((v) => {
        if (!v) return;
        this.api.patch('/sessions', v).subscribe(() => {
          this.notify.success('Saved');
          this.sessionsRes.reload();
        });
      });
  }

  protected deleteSession(id: string): void {
    this.dialog
      .open(ConfirmDialog, { data: { destructive: true, confirmKey: 'common.delete', message: 'Delete this session?' } })
      .afterClosed()
      .subscribe((ok) => {
        if (!ok) return;
        this.api.delete('/sessions', { id }).subscribe(() => {
          this.notify.success('Deleted');
          this.sessionsRes.reload();
        });
      });
  }

  protected newExam(classId: string): void {
    this.dialog
      .open(ExamFormDialog, { data: { classId } })
      .afterClosed()
      .subscribe((v) => {
        if (!v) return;
        this.api.post('/exams', v).subscribe(() => {
          this.notify.success('Saved');
          this.examsRes.reload();
        });
      });
  }

  protected editExam(classId: string, exam: Exam): void {
    this.dialog
      .open(ExamFormDialog, { data: { classId, exam } })
      .afterClosed()
      .subscribe((v) => {
        if (!v) return;
        this.api.patch('/exams', v).subscribe(() => {
          this.notify.success('Saved');
          this.examsRes.reload();
        });
      });
  }

  protected deleteExam(id: string): void {
    this.dialog
      .open(ConfirmDialog, { data: { destructive: true, confirmKey: 'common.delete', message: 'Delete this exam?' } })
      .afterClosed()
      .subscribe((ok) => {
        if (!ok) return;
        this.api.delete('/exams', { id }).subscribe(() => {
          this.notify.success('Deleted');
          this.examsRes.reload();
        });
      });
  }

  protected openAttendance(classId: string, sessionId: string): void {
    this.dialog.open(AttendanceDialog, {
      data: { classId, sessionId },
      width: '700px',
    });
  }

  protected openExamResults(classId: string, examId: string): void {
    this.dialog.open(ExamResultsDialog, {
      data: { classId, examId },
      width: '800px',
    });
  }
}
