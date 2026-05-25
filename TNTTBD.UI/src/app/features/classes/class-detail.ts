import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
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
  protected enrollmentsData = signal<{
    enrollments: PaginatedResult<CatechismClassEucharisticYouth>;
    youthLookup: PaginatedResult<EucharisticYouth>;
  } | null>(null);

  protected catechistsData = signal<{
    catechists: PaginatedResult<CatechismClassCatechist>;
    catechistLookup: PaginatedResult<Catechist>;
  } | null>(null);

  protected sessionsData = signal<PaginatedResult<CatechismSession> | null>(null);
  protected examsData = signal<PaginatedResult<Exam> | null>(null);

  private tabsLoaded = { enrollments: false, catechists: false, sessions: false, exams: false };

  protected onTabChange(index: number): void {
    const classId = this.route.snapshot.queryParamMap.get('id');
    if (!classId) return;
    switch (index) {
      case 0:
        if (!this.tabsLoaded.enrollments) {
          this.tabsLoaded.enrollments = true;
          this.loadEnrollments(classId);
        }
        break;
      case 1:
        if (!this.tabsLoaded.catechists) {
          this.tabsLoaded.catechists = true;
          this.loadCatechists(classId);
        }
        break;
      case 2:
        if (!this.tabsLoaded.sessions) {
          this.tabsLoaded.sessions = true;
          this.loadSessions(classId);
        }
        break;
      case 3:
        if (!this.tabsLoaded.exams) {
          this.tabsLoaded.exams = true;
          this.loadExams(classId);
        }
        break;
    }
  }

  private loadEnrollments(classId: string): void {
    forkJoin({
      enrollments: this.api.list<CatechismClassEucharisticYouth>('/class-youth', { classId }).pipe(
        catchError(() => of(emptyPage<CatechismClassEucharisticYouth>())),
      ),
      youthLookup: this.api.list<EucharisticYouth>('/youths').pipe(
        catchError(() => of(emptyPage<EucharisticYouth>())),
      ),
    }).subscribe((d) => this.enrollmentsData.set(d));
  }

  private loadCatechists(classId: string): void {
    forkJoin({
      catechists: this.api.list<CatechismClassCatechist>('/class-catechist', { classId }).pipe(
        catchError(() => of(emptyPage<CatechismClassCatechist>())),
      ),
      catechistLookup: this.api.list<Catechist>('/catechists').pipe(
        catchError(() => of(emptyPage<Catechist>())),
      ),
    }).subscribe((d) => this.catechistsData.set(d));
  }

  private loadSessions(classId: string): void {
    this.api.list<CatechismSession>('/sessions', { classId }).pipe(
      catchError(() => of(emptyPage<CatechismSession>())),
    ).subscribe((d) => this.sessionsData.set(d));
  }

  private loadExams(classId: string): void {
    this.api.list<Exam>('/exams', { classId }).pipe(
      catchError(() => of(emptyPage<Exam>())),
    ).subscribe((d) => this.examsData.set(d));
  }

  /** Auto-load first tab on init */
  ngOnInit(): void {
    this.onTabChange(0);
  }

  protected youthOfEnrollment(enrollmentId: string): EucharisticYouth | undefined {
    const ed = this.enrollmentsData();
    if (!ed) return undefined;
    const e = ed.enrollments.items.find((x) => x.id === enrollmentId);
    if (!e) return undefined;
    return ed.youthLookup.items.find((y) => y.id === e.eucharisticYouthId);
  }

  protected catechistOf(id: string): Catechist | undefined {
    return this.catechistsData()?.catechistLookup.items.find((c) => c.id === id);
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
          this.loadSessions(classId);
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
          this.loadSessions(classId);
        });
      });
  }

  protected deleteSession(id: string): void {
    const classId = this.route.snapshot.queryParamMap.get('id')!;
    this.dialog
      .open(ConfirmDialog, { data: { destructive: true, confirmKey: 'common.delete', message: 'Delete this session?' } })
      .afterClosed()
      .subscribe((ok) => {
        if (!ok) return;
        this.api.delete('/sessions', { id }).subscribe(() => {
          this.notify.success('Deleted');
          this.loadSessions(classId);
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
          this.loadExams(classId);
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
          this.loadExams(classId);
        });
      });
  }

  protected deleteExam(id: string): void {
    const classId = this.route.snapshot.queryParamMap.get('id')!;
    this.dialog
      .open(ConfirmDialog, { data: { destructive: true, confirmKey: 'common.delete', message: 'Delete this exam?' } })
      .afterClosed()
      .subscribe((ok) => {
        if (!ok) return;
        this.api.delete('/exams', { id }).subscribe(() => {
          this.notify.success('Deleted');
          this.loadExams(classId);
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
