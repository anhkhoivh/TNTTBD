import { Component, inject, effect, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import { MatChipsModule } from '@angular/material/chips';
import { MatSelectModule } from '@angular/material/select';
import { NotificationService } from '../../core/services/notification.service';
import { DialogService } from '../../core/services/dialog.service';
import { AuthService } from '../../core/auth/auth.service';
import { CatechismClassEucharisticYouth, Exam, CatechismSession } from '../../shared/models';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../shared/pipes/full-name.pipe';
import { DateLocalePipe } from '../../shared/pipes/date-locale.pipe';
import { EnrollmentStatus, CatechismClassCatechistRole, UserRole } from '../../shared/enums';
import { SessionFormDialog } from '../../shared/components/session-form-dialog';
import { ExamFormDialog } from '../../shared/components/exam-form-dialog';
import { CatechistAssignDialog, CatechistAssignData } from '../../shared/components/catechist-assign-dialog';
import { EnrollYouthDialog, EnrollYouthData } from '../../shared/components/enroll-youth-dialog';
import {
  AddChoice,
  AddChoiceBottomSheet,
  AddChoiceData,
} from '../../shared/components/add-choice-bottom-sheet/add-choice-bottom-sheet';
import { ClassDetailService } from './class-detail.service';
import { TranslationService } from '../../core/i18n/translation.service';

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
    TranslatePipe,
    FullNamePipe,
    DateLocalePipe,
  ],
  templateUrl: './class-detail.html',
  styleUrl: './class-detail.scss',

  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [ClassDetailService],
})
export class ClassDetail {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private classDetailService = inject(ClassDetailService);
  private dialogs = inject(DialogService);
  private notify = inject(NotificationService);
  protected auth = inject(AuthService);
  #translate = inject(TranslationService);

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

  constructor() {
    const queryParamMap = toSignal(this.route.queryParamMap, { initialValue: this.route.snapshot.queryParamMap });
    effect(() => this.classDetailService.classId.set(queryParamMap().get('id')));
  }

  /** Class info for the active route id */
  protected data = this.classDetailService.classDetail;

  /** Lazy-loaded tab data */
  protected enrollmentsRes = this.classDetailService.enrollmentsRes;
  protected catechistsRes = this.classDetailService.catechistsRes;
  protected sessionsRes = this.classDetailService.sessionsRes;
  protected examsRes = this.classDetailService.examsRes;
  protected report = this.classDetailService.report;

  protected onTabChange(index: number): void {
    this.classDetailService.onTabChange(index);
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

  protected async addYouth(classId: string): Promise<void> {
    const choice = await this.dialogs.openBottomSheet<AddChoiceBottomSheet, AddChoiceData, AddChoice>(
      AddChoiceBottomSheet,
      {
        data: {
          titleKey: 'class.addYouth',
          addNewLabelKey: 'class.addNewYouth',
          addNewIcon: 'person_add',
          linkExistingLabelKey: 'class.addExistingYouth',
          linkExistingIcon: 'person_search',
        },
      },
    );
    if (choice === 'new') {
      this.router.navigate(['/app/youth/edit'], { queryParams: { classId } });
    } else if (choice === 'existing') {
      await this.addExistingYouth(classId);
    }
  }

  private async addExistingYouth(classId: string): Promise<void> {
    const enrolledIds = new Set(
      (this.enrollmentsRes.value()?.items ?? []).map((e) => e.eucharisticYouthId),
    );
    const result = await this.dialogs.open<EnrollYouthDialog, EnrollYouthData>(EnrollYouthDialog, {
      data: { classId, enrolledYouthIds: enrolledIds },
    });
    if (!result) return;
    try {
      await this.classDetailService.enrollYouth(result);
      this.notify.success(this.#translate.t('class.youthEnrolled'));
    } catch {
      this.notify.error('error.generic');
    }
  }

  protected async changeEnrollmentStatus(enrollmentId: string, status: EnrollmentStatus): Promise<void> {
    try {
      await this.classDetailService.changeEnrollmentStatus({ id: enrollmentId, status, note: '' });
      this.notify.success(this.#translate.t('common.saved'));
    } catch {
      this.notify.error('error.generic');
    }
  }

  protected async removeYouthFromClass(enrollmentId: string): Promise<void> {
    const ok = await this.dialogs.confirmDelete('Remove this youth from class?');
    if (!ok) return;
    try {
      await this.classDetailService.removeEnrollment(enrollmentId);
      this.notify.success(this.#translate.t('common.saved'));
    } catch {
      this.notify.error('error.generic');
    }
  }

  protected roleKey(role: CatechismClassCatechistRole): string {
    return `catechistRole.${role}`;
  }

  protected async assignCatechist(classId: string): Promise<void> {
    const existing = this.catechistsRes.value()?.items ?? [];
    const result = await this.dialogs.open<CatechistAssignDialog, CatechistAssignData>(
      CatechistAssignDialog,
      { data: { classId, existingAssignments: existing } },
    );
    if (!result) return;
    try {
      await this.classDetailService.assignCatechist(result);
      this.notify.success(this.#translate.t('catechist.assigned'));
    } catch {
      this.notify.error('error.generic');
    }
  }

  protected async removeCatechist(id: string): Promise<void> {
    const ok = await this.dialogs.confirmDelete('Remove this catechist from class?');
    if (!ok) return;
    try {
      await this.classDetailService.removeCatechist(id);
      this.notify.success(this.#translate.t('catechist.removed'));
    } catch {
      this.notify.error('error.generic');
    }
  }

  protected async newSession(classId: string): Promise<void> {
    const v = await this.dialogs.open(SessionFormDialog, { data: { classId } });
    if (!v) return;
    await this.classDetailService.createSession(v);
    this.notify.success(this.#translate.t('common.saved'));
  }

  protected async editSession(classId: string, session: CatechismSession): Promise<void> {
    const v = await this.dialogs.open(SessionFormDialog, { data: { classId, session } });
    if (!v) return;
    await this.classDetailService.updateSession(v);
    this.notify.success(this.#translate.t('common.saved'));
  }

  protected async deleteSession(id: string): Promise<void> {
    const ok = await this.dialogs.confirmDelete('Delete this session?');
    if (!ok) return;
    await this.classDetailService.deleteSession(id);
    this.notify.success(this.#translate.t('common.deleted'));
  }

  protected async newExam(classId: string): Promise<void> {
    const v = await this.dialogs.open(ExamFormDialog, { data: { classId } });
    if (!v) return;
    await this.classDetailService.createExam(v);
    this.notify.success('Saved');
  }

  protected async editExam(classId: string, exam: Exam): Promise<void> {
    const v = await this.dialogs.open(ExamFormDialog, { data: { classId, exam } });
    if (!v) return;
    await this.classDetailService.updateExam(v);
    this.notify.success('Saved');
  }

  protected async deleteExam(id: string): Promise<void> {
    const ok = await this.dialogs.confirmDelete(this.#translate.t('exam.deleteConfirm'));
    if (!ok) return;
    await this.classDetailService.deleteExam(id);
    this.notify.success(this.#translate.t('common.deleted'));
  }
}
