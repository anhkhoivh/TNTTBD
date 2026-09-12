import { Component, inject, signal, computed, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { rxResource, toSignal } from '@angular/core/rxjs-interop';
import { switchMap, of, catchError } from 'rxjs';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import { MatChipsModule } from '@angular/material/chips';
import { MatSelectModule } from '@angular/material/select';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ApiService } from '../../core/services/api.service';
import { NotificationService } from '../../core/services/notification.service';
import { AuthService } from '../../core/auth/auth.service';
import {
  CatechismClass,
  CatechismClassCatechist,
  CatechismClassEucharisticYouth,
  CatechismSession,
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
import { CatechistAssignDialog, CatechistAssignData } from '../../shared/components/catechist-assign-dialog';
import { EnrollYouthDialog, EnrollYouthData } from '../../shared/components/enroll-youth-dialog';

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
    MatSelectModule,
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

  protected canAssignCatechists(): boolean {
    return this.auth.hasRole([UserRole.Admin]);
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

  protected enrollmentsRes = rxResource<PaginatedResult<CatechismClassEucharisticYouth>, string>({
    params: () => this.classId() && this.activeTabs().has(0) ? this.classId()! : '',
    stream: (loaderParams) =>
      this.api.list<CatechismClassEucharisticYouth>('/class-youth', { classId: loaderParams.params }).pipe(
        catchError(() => of(emptyPage<CatechismClassEucharisticYouth>())),
      )
  });


  protected catechistsRes = rxResource<PaginatedResult<CatechismClassCatechist>, string>({
    params: () => this.classId() && this.activeTabs().has(1) ? this.classId()! : '',
    stream: (loaderParams) =>
      this.api.list<CatechismClassCatechist>('/class-catechist', { classId: loaderParams.params }).pipe(
        catchError(() => of(emptyPage<CatechismClassCatechist>())),
      )
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

  protected youthOfEnrollment(enrollmentId: string): CatechismClassEucharisticYouth | undefined {
    const ed = this.enrollmentsRes.value();
    if (!ed) return undefined;
    const e = ed.items.find((x) => x.id === enrollmentId);
    if (!e) return undefined;
    return e;
  }

  protected statusKey(status: EnrollmentStatus): string {
    return `enrollment.status.${status}`;
  }

  protected readonly enrollmentStatuses = [
    EnrollmentStatus.Active,
    EnrollmentStatus.Pending,
    EnrollmentStatus.Suspended,
    EnrollmentStatus.Transferred,
    EnrollmentStatus.Completed,
  ];

  protected addExistingYouth(classId: string): void {
    const enrolledIds = new Set(
      (this.enrollmentsRes.value()?.items ?? []).map((e) => e.eucharisticYouthId),
    );
    this.dialog
      .open<EnrollYouthDialog, EnrollYouthData>(EnrollYouthDialog, {
        data: { classId, enrolledYouthIds: enrolledIds },
      })
      .afterClosed()
      .subscribe((result) => {
        if (!result) return;
        this.api.post('/class-youth', result).subscribe({
          next: () => {
            this.notify.success('class.youthEnrolled');
            this.enrollmentsRes.reload();
          },
          error: () => this.notify.error('error.generic'),
        });
      });
  }

  protected changeEnrollmentStatus(enrollmentId: string, status: EnrollmentStatus): void {
    this.api
      .patch('/class-youth', { id: enrollmentId, status, note: '' })
      .subscribe({
        next: () => {
          this.notify.success('Saved');
          this.enrollmentsRes.reload();
        },
        error: () => this.notify.error('error.generic'),
      });
  }

  protected removeYouthFromClass(enrollmentId: string): void {
    this.dialog
      .open(ConfirmDialog, {
        data: { destructive: true, confirmKey: 'common.delete', message: 'Remove this youth from class?' },
      })
      .afterClosed()
      .subscribe((ok) => {
        if (!ok) return;
        this.api.delete('/class-youth', { id: enrollmentId }).subscribe({
          next: () => {
            this.notify.success('Saved');
            this.enrollmentsRes.reload();
          },
          error: () => this.notify.error('error.generic'),
        });
      });
  }

  protected roleKey(role: CatechismClassCatechistRole): string {
    return `catechistRole.${role}`;
  }

  protected assignCatechist(classId: string): void {
    const existing = this.catechistsRes.value()?.items ?? [];
    this.dialog
      .open<CatechistAssignDialog, CatechistAssignData>(CatechistAssignDialog, {
        data: { classId, existingAssignments: existing },
      })
      .afterClosed()
      .subscribe((result) => {
        if (!result) return;
        this.api.post('/class-catechist', result).subscribe({
          next: () => {
            this.notify.success('catechist.assigned');
            this.catechistsRes.reload();
          },
          error: () => this.notify.error('error.generic'),
        });
      });
  }

  protected removeCatechist(id: string): void {
    this.dialog
      .open(ConfirmDialog, {
        data: { destructive: true, confirmKey: 'common.delete', message: 'Remove this catechist from class?' },
      })
      .afterClosed()
      .subscribe((ok) => {
        if (!ok) return;
        this.api.delete('/class-catechist', { id }).subscribe({
          next: () => {
            this.notify.success('catechist.removed');
            this.catechistsRes.reload();
          },
          error: () => this.notify.error('error.generic'),
        });
      });
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
        this.api.delete('/sessions', { catechismSessionId: id }).subscribe(() => {
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
}
