import { Component, inject, effect, ChangeDetectionStrategy } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import { AuthService } from '../../../core/auth/auth.service';
import { DialogService } from '../../../core/services/dialog.service';
import { NotificationService } from '../../../core/services/notification.service';
import { ContactRelationType, UserRole } from '../../../shared/enums';
import { TranslatePipe } from '../../../core/i18n/translate.pipe';
import { TranslationService } from '../../../core/i18n/translation.service';
import { FullNamePipe } from '../../../shared/pipes/full-name.pipe';
import { MemberAvatar } from '../../../shared/components/member-avatar/member-avatar';
import { ContactService } from '../contact.service';

@Component({
  selector: 'app-contact-detail',
  standalone: true,
  imports: [RouterLink, MatCardModule, MatIconModule, MatButtonModule, MatListModule, TranslatePipe, FullNamePipe, MemberAvatar],
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
