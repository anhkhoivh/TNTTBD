import { Component, inject, signal, ChangeDetectionStrategy } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { TranslatePipe } from '../../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../../shared/pipes/full-name.pipe';
import { EmptyState } from '../../../shared/components/empty-state';
import { ContactService } from '../contact.service';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { DialogService } from '../../../core/services/dialog.service';
import { NotificationService } from '../../../core/services/notification.service';
import { AuthService } from '../../../core/auth/auth.service';
import { UserRole } from '../../../shared/enums';
import { RowActionsBottomSheet, RowActionsData, RowAction } from '../../../shared/components/row-actions-bottom-sheet/row-actions-bottom-sheet';
import { Contact } from '../../../shared/models';

@Component({
  selector: 'app-contact-list',
  standalone: true,
  imports: [
    RouterLink,
    FormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatPaginatorModule,
    TranslatePipe,
    FullNamePipe,
    EmptyState,
    MatProgressSpinner
],
  templateUrl: './contact-list.html',
  styleUrl: './contact-list.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [ContactService],
})
export class ContactList {
  private contactService = inject(ContactService);
  private auth = inject(AuthService);
  private router = inject(Router);
  private dialogs = inject(DialogService);
  private notify = inject(NotificationService);

  protected data = this.contactService.list;
  protected isLoading = this.contactService.isListLoading;

  protected displayedColumns = ['fullName', 'phoneNumber', 'email', 'actions'];
  protected searchQuery = signal('');

  onPageChange(event: PageEvent) {
    this.contactService.listParams.update((p) => ({
      ...p,
      page: event.pageIndex + 1,
      pageSize: event.pageSize,
    }));
  }

  onSearch(query: string) {
    this.searchQuery.set(query);
    this.contactService.listParams.update((p) => ({ ...p, search: query, page: 1 }));
  }

  private canDelete(): boolean {
    return this.auth.hasRole([UserRole.Admin]);
  }

  protected async openActions(row: Contact): Promise<void> {
    const data: RowActionsData = { canView: true, canEdit: true, canDelete: this.canDelete() };
    const action = await this.dialogs.openBottomSheet<RowActionsBottomSheet, RowActionsData, RowAction>(
      RowActionsBottomSheet,
      { data },
    );
    switch (action) {
      case 'view':
        await this.router.navigate(['/app/contacts/detail'], { queryParams: { id: row.id } });
        break;
      case 'edit':
        await this.router.navigate(['/app/contacts/edit'], { queryParams: { id: row.id } });
        break;
      case 'delete': {
        const ok = await this.dialogs.confirmDelete(`Delete contact "${row.fullName.givenName}"?`);
        if (!ok) return;
        await this.contactService.remove(row.id);
        this.notify.success('Deleted');
        break;
      }
    }
  }
}
