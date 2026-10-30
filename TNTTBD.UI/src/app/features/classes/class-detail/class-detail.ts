import { Component, inject, effect, signal, ChangeDetectionStrategy } from '@angular/core';
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
import { MatTooltipModule } from '@angular/material/tooltip';
import { NotificationService } from '../../../core/services/notification.service';
import { DialogService } from '../../../core/services/dialog.service';
import { AuthService } from '../../../core/auth/auth.service';
import { CatechismClassEucharisticYouth, Exam, CatechismSession } from '../../../shared/models';
import { TranslatePipe } from '../../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../../shared/pipes/full-name.pipe';
import { DateLocalePipe } from '../../../shared/pipes/date-locale.pipe';
import { EnrollmentStatus, CatechismClassCatechistRole, UserRole } from '../../../shared/enums';
import { SessionFormDialog } from '../../../shared/components/session-form-dialog';
import { ExamFormDialog } from '../../../shared/components/exam-form-dialog';
import { CatechistAssignDialog, CatechistAssignData } from '../../../shared/components/catechist-assign-dialog';
import { EnrollYouthDialog, EnrollYouthData } from '../../../shared/components/enroll-youth-dialog';
import {
  AddChoice,
  AddChoiceBottomSheet,
  AddChoiceData,
} from '../../../shared/components/add-choice-bottom-sheet/add-choice-bottom-sheet';
import { ClassDetailService } from './class-detail.service';
import { TranslationService } from '../../../core/i18n/translation.service';
import { tabIndexFromKey, tabKeyForSelection } from '../../../shared/utils/tab-query-state';
import { ScrollableTabsDirective } from '../../../shared/directives/scrollable-tabs.directive';
import {
  RowAction,
  RowActionsBottomSheet,
  RowActionsData,
} from '../../../shared/components/row-actions-bottom-sheet/row-actions-bottom-sheet';

@Component({
  selector: 'app-class-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatCardModule,
    MatTabsModule,
    ScrollableTabsDirective,
    MatTableModule,
    MatIconModule,
    MatButtonModule,
    MatListModule,
    MatChipsModule,
    MatSelectModule,
    MatTooltipModule,
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
  protected selectedTabIndex = signal(0);

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
    effect(() => {
      const params = queryParamMap();
      this.classDetailService.classId.set(params.get('id'));
      const index = tabIndexFromKey(this.tabKeys(), params.get('tab'));
      this.selectedTabIndex.set(index);
      this.classDetailService.onTabChange(index);
    });
  }

  /** Class info for the active route id */
  protected data = this.classDetailService.classDetail;

  /** Lazy-loaded tab data */
  protected enrollmentsRes = this.classDetailService.enrollmentsRes;
  protected catechistsRes = this.classDetailService.catechistsRes;
  protected sessionsRes = this.classDetailService.sessionsRes;
  protected examsRes = this.classDetailService.examsRes;

  protected onTabChange(index: number): void {
    this.classDetailService.onTabChange(index);
    const currentTab = this.route.snapshot.queryParamMap.get('tab');
    const tab = tabKeyForSelection(this.tabKeys(), index, currentTab);
    if (!tab) return;
    void this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { tab },
      queryParamsHandling: 'merge',
    });
  }

  private tabKeys(): string[] {
    return this.isContact ? ['enrolled', 'catechists'] : ['enrolled', 'catechists', 'sessions', 'exams'];
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

  protected async openEnrollmentActions(enrollment: CatechismClassEucharisticYouth): Promise<void> {
    if (!this.canManage()) return;
    const action = await this.dialogs.openBottomSheet<RowActionsBottomSheet, RowActionsData, RowAction>(
      RowActionsBottomSheet,
      { data: {
        canView: true, canEdit: false, canDelete: true,
        viewLabelKey: 'report.open', viewIcon: 'assessment',
        deleteLabelKey: 'class.removeYouth', deleteIcon: 'link_off',
      } },
    );
    if (!this.canManage()) return;
    switch (action) {
      case 'view':
        await this.router.navigate(['/app/youth/report'], {
          queryParams: { youthId: enrollment.eucharisticYouthId, enrollmentId: enrollment.id },
        });
        break;
      case 'delete':
        await this.removeYouthFromClass(enrollment.id);
        break;
    }
  }

  protected async openSessionActions(classId: string, session: CatechismSession): Promise<void> {
    if (this.isContact) return;
    const canManage = this.canManage();
    const action = await this.dialogs.openBottomSheet<RowActionsBottomSheet, RowActionsData, RowAction>(
      RowActionsBottomSheet,
      { data: {
        canView: true, canEdit: canManage, canDelete: canManage,
        viewLabelKey: 'attendance.title', viewIcon: 'how_to_reg',
      } },
    );
    if (this.isContact) return;
    switch (action) {
      case 'view':
        await this.router.navigate(['/app/classes/attendance'], { queryParams: { classId, sessionId: session.id } });
        break;
      case 'edit':
        if (this.canManage()) await this.editSession(classId, session);
        break;
      case 'delete':
        if (this.canManage()) await this.deleteSession(session.id);
        break;
    }
  }

  protected async openExamActions(classId: string, exam: Exam): Promise<void> {
    if (this.isContact) return;
    const canManage = this.canManage();
    const action = await this.dialogs.openBottomSheet<RowActionsBottomSheet, RowActionsData, RowAction>(
      RowActionsBottomSheet,
      { data: {
        canView: true, canEdit: canManage, canDelete: canManage,
        viewLabelKey: 'exam.results', viewIcon: 'edit_note',
      } },
    );
    if (this.isContact) return;
    switch (action) {
      case 'view':
        await this.router.navigate(['/app/classes/exam-results'], { queryParams: { classId, examId: exam.id } });
        break;
      case 'edit':
        if (this.canManage()) await this.editExam(classId, exam);
        break;
      case 'delete':
        if (this.canManage()) await this.deleteExam(exam.id);
        break;
    }
  }

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

  protected async changeCatechistStatus(id: string, status: EnrollmentStatus): Promise<void> {
    try {
      await this.classDetailService.changeCatechistStatus(id, status);
      this.notify.success(this.#translate.t('common.saved'));
    } catch {
      // Revert the select to the persisted value; the interceptor already showed the error.
      this.catechistsRes.reload();
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
