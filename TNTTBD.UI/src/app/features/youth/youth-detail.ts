import { Component, inject, effect, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule, Location, NgOptimizedImage } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import { PhotoService } from '../../core/services/photo.service';
import { DialogService } from '../../core/services/dialog.service';
import { NotificationService } from '../../core/services/notification.service';
import { AuthService } from '../../core/auth/auth.service';
import { CatechismClass, EucharisticYouthContact } from '../../shared/models';
import { ContactRelationType, EnrollmentStatus, MilestoneType, UserRole } from '../../shared/enums';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { TranslationService } from '../../core/i18n/translation.service';
import {
  AddChoice,
  AddChoiceBottomSheet,
  AddChoiceData,
} from '../../shared/components/add-choice-bottom-sheet/add-choice-bottom-sheet';
import {
  LinkContactDialog,
  LinkContactData,
  LinkContactResult,
} from '../../shared/components/link-contact-dialog/link-contact-dialog';
import { FullNamePipe } from '../../shared/pipes/full-name.pipe';
import { DateLocalePipe } from '../../shared/pipes/date-locale.pipe';
import { YouthService } from './youth.service';

@Component({
  selector: 'app-youth-detail',
  standalone: true,
  imports: [
    NgOptimizedImage,
    CommonModule,
    RouterLink,
    MatCardModule,
    MatTabsModule,
    MatIconModule,
    MatChipsModule,
    MatButtonModule,
    MatListModule,
    TranslatePipe,
    FullNamePipe,
    DateLocalePipe,
  ],
  templateUrl: './youth-detail.html',
  styleUrl: './youth-detail.scss',

  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [YouthService],
})
export class YouthDetail {
  private route = inject(ActivatedRoute);
  private youthService = inject(YouthService);
  protected photoService = inject(PhotoService);
  private router = inject(Router);
  private auth = inject(AuthService);
  private dialogs = inject(DialogService);
  private notify = inject(NotificationService);
  private translate = inject(TranslationService);
  #location = inject(Location);

  constructor() {
    const queryParamMap = toSignal(this.route.queryParamMap, { initialValue: this.route.snapshot.queryParamMap });
    effect(() => this.youthService.detailId.set(queryParamMap().get('id')));
  }

  /** Youth profile for the active route id (milestones + photos embedded) */
  protected data = this.youthService.detail;

  /** Lazy-loaded contacts tab */
  protected contactsData = this.youthService.contacts;

  /** Lazy-loaded class history tab */
  protected historyData = this.youthService.history;

  protected onTabChange(index: number): void {
    if (index === 3) this.youthService.contactsActive.set(true);
    if (index === 4) this.youthService.historyActive.set(true);
  }

  protected classOf(id: string, list: CatechismClass[]): CatechismClass | undefined {
    return list.find((c) => c.id === id);
  }

  protected milestoneKey(type: MilestoneType): string {
    const map: Record<MilestoneType, string> = {
      [MilestoneType.Baptism]: 'milestone.baptism',
      [MilestoneType.FirstPenance]: 'milestone.firstPenance',
      [MilestoneType.FirstCommunion]: 'milestone.firstCommunion',
      [MilestoneType.Confirmation]: 'milestone.confirmation',
      [MilestoneType.SolemnCommunion]: 'milestone.solemnCommunion',
      [MilestoneType.Graduation]: 'milestone.graduation',
    };
    return map[type] ?? '';
  }

  protected relationKey(r: ContactRelationType): string {
    return `contact.relation.${r}`;
  }

  protected enrollmentStatusKey(s: EnrollmentStatus): string {
    return `enrollment.status.${s}`;
  }

  protected canAddContact(): boolean {
    return this.auth.hasRole([UserRole.Admin, UserRole.Catechist]);
  }

  /** Admin/Catechist, the contact themself, or the youth the contact is linked to. */
  protected canManageContact(yc: EucharisticYouthContact): boolean {
    if (this.auth.hasRole([UserRole.Admin, UserRole.Catechist])) return true;
    const user = this.auth.currentUser();
    if (!user) return false;
    if (user.userRole === UserRole.EucharisticYouthContact) return user.contactId === yc.contactId;
    if (user.userRole === UserRole.EucharisticYouth) return user.eucharisticYouthId === yc.eucharisticYouthId;
    return false;
  }

  protected async addContact(youthId: string): Promise<void> {
    const choice = await this.dialogs.openBottomSheet<AddChoiceBottomSheet, AddChoiceData, AddChoice>(
      AddChoiceBottomSheet,
      {
        data: {
          titleKey: 'contact.add',
          addNewLabelKey: 'contact.addNew',
          addNewIcon: 'person_add',
          linkExistingLabelKey: 'contact.linkExisting',
          linkExistingIcon: 'link',
        },
      },
    );

    if (choice === 'new') {
      this.router.navigate(['/app/contacts/edit'], { queryParams: { youthId } });
      return;
    }
    if (choice !== 'existing') return;

    const existingContactIds = new Set(this.contactsData().items.map((c) => c.contactId));
    const result = await this.dialogs.open<LinkContactDialog, LinkContactData, LinkContactResult>(LinkContactDialog, {
      data: { youthId, existingContactIds },
      width: '480px',
    });
    if (!result) return;

    try {
      await this.youthService.linkContact(result);
      this.notify.success(this.translate.t('contact.linked'));
    } catch {
      // error toast already shown by global http error handling
    }
  }

  protected async unlinkContact(yc: EucharisticYouthContact): Promise<void> {
    const ok = await this.dialogs.confirmDelete(this.translate.t('contact.unlinkConfirm'));
    if (!ok) return;
    try {
      await this.youthService.unlinkContact(yc.id);
      this.notify.success(this.translate.t('contact.unlinked'));
    } catch {
      // error toast already shown by global http error handling
    }
  }

  navigateBack(): void {
    this.#location.back();
  }
}
