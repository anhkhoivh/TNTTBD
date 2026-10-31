import { Component, inject, effect, ChangeDetectionStrategy } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import { MatTooltipModule } from '@angular/material/tooltip';
import { AuthService } from '../../../core/auth/auth.service';
import { DialogService } from '../../../core/services/dialog.service';
import { NotificationService } from '../../../core/services/notification.service';
import { ContactRelationType, UserRole } from '../../../shared/enums';
import { ContactDetail as ContactDetailModel, ContactYouth } from '../../../shared/models';
import { TranslatePipe } from '../../../core/i18n/translate.pipe';
import { TranslationService } from '../../../core/i18n/translation.service';
import { FullNamePipe } from '../../../shared/pipes/full-name.pipe';
import { MemberAvatar } from '../../../shared/components/member-avatar/member-avatar';
import {
  AddChoice,
  AddChoiceBottomSheet,
  AddChoiceData,
} from '../../../shared/components/add-choice-bottom-sheet/add-choice-bottom-sheet';
import {
  LinkYouthDialog,
  LinkYouthData,
  LinkYouthResult,
} from '../../../shared/components/link-youth-dialog/link-youth-dialog';
import { ContactService } from '../contact.service';

@Component({
  selector: 'app-contact-detail',
  standalone: true,
  imports: [RouterLink, MatCardModule, MatIconModule, MatButtonModule, MatListModule, MatTooltipModule, TranslatePipe, FullNamePipe, MemberAvatar],
  templateUrl: './contact-detail.html',
  styleUrl: './contact-detail.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [ContactService],
})
export class ContactDetail {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private contactService = inject(ContactService);
  private auth = inject(AuthService);
  private dialogs = inject(DialogService);
  private notify = inject(NotificationService);
  private translate = inject(TranslationService);
  #location = inject(Location);

  constructor() {
    const queryParamMap = toSignal(this.route.queryParamMap, { initialValue: this.route.snapshot.queryParamMap });
    effect(() => this.contactService.detailId.set(queryParamMap().get('id')));
  }

  protected data = this.contactService.detail;

  protected canDelete(): boolean {
    return this.auth.hasRole([UserRole.Admin, UserRole.Catechist]);
  }

  protected relationKey(r: ContactRelationType): string {
    return `contact.relation.${r}`;
  }

  /** Linking requires access to the youth (backend: Admin/Catechist). */
  protected canLinkYouth(): boolean {
    return this.auth.hasRole([UserRole.Admin, UserRole.Catechist]);
  }

  /** Admin/Catechist or the contact themself (mirrors UnlinkContactFromYouthHandler). */
  protected canUnlinkYouth(contactId: string): boolean {
    if (this.auth.hasRole([UserRole.Admin, UserRole.Catechist])) return true;
    const user = this.auth.currentUser();
    return user?.userRole === UserRole.EucharisticYouthContact && user.contactId === contactId;
  }

  protected async addYouth(contact: ContactDetailModel): Promise<void> {
    const choice = await this.dialogs.openBottomSheet<AddChoiceBottomSheet, AddChoiceData, AddChoice>(
      AddChoiceBottomSheet,
      {
        data: {
          titleKey: 'youth.add',
          addNewLabelKey: 'youth.addNew',
          addNewIcon: 'person_add',
          linkExistingLabelKey: 'youth.linkExisting',
          linkExistingIcon: 'link',
        },
      },
    );

    if (choice === 'new') {
      this.router.navigate(['/app/youth/edit'], { queryParams: { contactId: contact.id } });
      return;
    }
    if (choice !== 'existing') return;

    const existingYouthIds = new Set(contact.youths.map((y) => y.eucharisticYouthId));
    const result = await this.dialogs.open<LinkYouthDialog, LinkYouthData, LinkYouthResult>(LinkYouthDialog, {
      data: { contactId: contact.id, existingYouthIds },
      width: '480px',
    });
    if (!result) return;

    try {
      await this.contactService.linkYouth(result);
      this.notify.success(this.translate.t('contact.linked'));
    } catch {
      // error toast already shown by global http error handling
    }
  }

  protected async unlinkYouth(link: ContactYouth): Promise<void> {
    const ok = await this.dialogs.confirmDelete(this.translate.t('contact.unlinkYouthConfirm'));
    if (!ok) return;
    try {
      await this.contactService.unlinkYouth(link.id);
      this.notify.success(this.translate.t('contact.unlinked'));
    } catch {
      // error toast already shown by global http error handling
    }
  }

  protected async remove(id: string): Promise<void> {
    const ok = await this.dialogs.confirmDelete(this.translate.t('contact.deleteConfirm'));
    if (!ok) return;
    try {
      await this.contactService.remove(id);
      this.notify.success(this.translate.t('contact.deleted'));
      this.router.navigate(['/app/contacts']);
    } catch {
      // error toast already shown by global http error handling
    }
  }

  navigateBack(): void {
    this.#location.back();
  }
}
