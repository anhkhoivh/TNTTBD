import { Component, inject, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { switchMap, of, forkJoin, BehaviorSubject } from 'rxjs';
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
} from '../../shared/models';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../shared/pipes/full-name.pipe';
import { DateLocalePipe } from '../../shared/pipes/date-locale.pipe';
import { EnrollmentStatus, CatechismClassCatechistRole, UserRole } from '../../shared/enums';
import { SessionFormDialog } from '../../shared/components/session-form-dialog';
import { ExamFormDialog } from '../../shared/components/exam-form-dialog';
import { ConfirmDialog } from '../../shared/components/confirm-dialog';

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

  private reload$ = new BehaviorSubject<void>(undefined);
  protected refresh(): void { this.reload$.next(); }

  protected canManage(): boolean {
    return this.auth.hasRole([UserRole.Admin, UserRole.Catechist]);
  }

  protected data = toSignal(
    this.route.queryParamMap.pipe(
      switchMap((qp) => {
        const id = qp.get('id');
        if (!id) return of(null);
        return this.reload$.pipe(
          switchMap(() =>
            forkJoin({
          klass: this.api.get<CatechismClass>('/classes/detail', { id }),
          sessions: this.api.list<CatechismSession>('/sessions', { classId: id }),
          enrollments: this.api.list<CatechismClassEucharisticYouth>('/class-youth', {
            classId: id,
          }),
          catechists: this.api.list<CatechismClassCatechist>('/class-catechist', {
            classId: id,
          }),
          exams: this.api.list<Exam>('/exams', { classId: id }),
          youthLookup: this.api.list<EucharisticYouth>('/youths'),
          catechistLookup: this.api.list<Catechist>('/catechists'),
            }),
          ),
        );
      }),
    ),
    { initialValue: null },
  );

  protected youthName(id: string): string {
    const list = this.data()?.youthLookup.items ?? [];
    const y = list.find((x) => x.id === id);
    return y?.code ? `${y.code}` : id;
  }

  protected catechistOf(id: string): Catechist | undefined {
    return this.data()?.catechistLookup.items.find((c) => c.id === id);
  }

  protected youthOfEnrollment(enrollmentId: string): EucharisticYouth | undefined {
    const e = this.data()?.enrollments.items.find((x) => x.id === enrollmentId);
    if (!e) return undefined;
    return this.data()?.youthLookup.items.find((y) => y.id === e.eucharisticYouthId);
  }

  protected statusKey(status: EnrollmentStatus): string {
    return 'enrollment.status.' + (EnrollmentStatus[status] ?? '');
  }

  protected roleKey(role: CatechismClassCatechistRole): string {
    return 'catechistRole.' + (CatechismClassCatechistRole[role] ?? '');
  }

  protected newSession(classId: string): void {
    this.dialog
      .open(SessionFormDialog, { data: { classId } })
      .afterClosed()
      .subscribe((v) => {
        if (!v) return;
        this.api.post('/sessions', v).subscribe(() => {
          this.notify.success('Saved');
          this.refresh();
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
          this.refresh();
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
          this.refresh();
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
          this.refresh();
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
          this.refresh();
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
          this.refresh();
        });
      });
  }
}
