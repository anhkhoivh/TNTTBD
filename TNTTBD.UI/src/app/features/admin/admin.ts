import { Component, inject, signal, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { NotificationService } from '../../core/services/notification.service';
import { DialogService } from '../../core/services/dialog.service';
import { User } from '../../shared/models';
import { UserRole } from '../../shared/enums';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../shared/pipes/full-name.pipe';
import {
  RowActionsBottomSheet,
  RowActionsData,
  RowExtraAction,
} from '../../shared/components/row-actions-bottom-sheet/row-actions-bottom-sheet';
import { UserFormDialog, UserFormData, UserFormResult } from './user-form-dialog';
import { ResetPasswordDialog, ResetPasswordData } from './reset-password-dialog';
import { AdminService } from './admin.service';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatTableModule,
    MatChipsModule,
    MatIconModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatPaginatorModule,
    MatProgressSpinner,
    TranslatePipe,
    FullNamePipe,
  ],
  templateUrl: './admin.html',
  styleUrl: './admin.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [AdminService],
})
export class AdminPage {
  private adminService = inject(AdminService);
  private dialogs = inject(DialogService);
  private notify = inject(NotificationService);

  protected readonly roles = Object.values(UserRole);

  protected data = this.adminService.users;
  protected isLoading = this.adminService.isLoading;

  protected searchQuery = signal('');
  protected roleFilter = signal<UserRole>(UserRole.Admin);

  protected cols = ['userName', 'email', 'role', 'link', 'active', 'actions'];

  protected roleKey(role: UserRole): string {
    return `user.role.${role}`;
  }

  protected linkLabel(u: User): string {
    if (u.catechistId) return `Catechist · ${u.catechistId.slice(0, 8)}`;
    if (u.eucharisticYouthId) return `Youth · ${u.eucharisticYouthId.slice(0, 8)}`;
    if (u.contactId) return `Contact · ${u.contactId.slice(0, 8)}`;
    return '';
  }

  protected onSearch(query: string): void {
    this.searchQuery.set(query);
    this.adminService.listParams.update((p) => ({ ...p, search: query, page: 1 }));
  }

  protected onRoleChange(role: UserRole): void {
    this.roleFilter.set(role);
    this.adminService.listParams.update((p) => ({ ...p, role, page: 1 }));
  }

  protected onPageChange(event: PageEvent): void {
    this.adminService.listParams.update((p) => ({
      ...p,
      page: event.pageIndex + 1,
      pageSize: event.pageSize,
    }));
  }

  protected async openActions(user: User): Promise<void> {
    const extraActions: RowExtraAction[] = [
      { key: 'resetPassword', icon: 'lock_reset', labelKey: 'password.reset' },
      user.isActive
        ? { key: 'deactivate', icon: 'person_off', labelKey: 'user.deactivate', warn: true }
        : { key: 'activate', icon: 'how_to_reg', labelKey: 'user.activate' },
    ];
    const data: RowActionsData = { canView: false, canEdit: true, canDelete: false, extraActions };
    const action = await this.dialogs.openBottomSheet<RowActionsBottomSheet, RowActionsData, string>(
      RowActionsBottomSheet,
      { data },
    );
    switch (action) {
      case 'edit':
        await this.edit(user);
        break;
      case 'resetPassword':
        await this.resetPassword(user);
        break;
      case 'activate':
        await this.activate(user);
        break;
      case 'deactivate':
        await this.deactivate(user);
        break;
    }
  }

  protected async create(): Promise<void> {
    const result = await this.dialogs.open<UserFormDialog, UserFormData, UserFormResult>(
      UserFormDialog,
      { data: { mode: 'create' } },
    );
    if (!result?.create) return;
    try {
      await this.adminService.createUser(result.create);
      this.notify.info('user.created');
    } catch {
      this.notify.error('error.generic');
    }
  }

  private async edit(user: User): Promise<void> {
    const result = await this.dialogs.open<UserFormDialog, UserFormData, UserFormResult>(
      UserFormDialog,
      { data: { mode: 'edit', user } },
    );
    if (!result?.update) return;
    try {
      await this.adminService.updateUser(result.update);
      this.notify.info('user.updated');
    } catch {
      this.notify.error('error.generic');
    }
  }

  private async activate(user: User): Promise<void> {
    try {
      await this.adminService.toggleActive(user.id, true);
      this.notify.info('user.updated');
    } catch {
      this.notify.error('error.generic');
    }
  }

  private async deactivate(user: User): Promise<void> {
    const ok = await this.dialogs.confirm({
      message: `${user.userName} (${user.email})`,
      confirmKey: 'user.deactivate',
      destructive: true,
    });
    if (!ok) return;
    try {
      await this.adminService.deleteUser(user.id);
      this.notify.info('user.deactivated');
    } catch {
      this.notify.error('error.generic');
    }
  }

  private async resetPassword(user: User): Promise<void> {
    const result = await this.dialogs.open<ResetPasswordDialog, ResetPasswordData>(
      ResetPasswordDialog,
      { data: { userId: user.id, userName: user.userName } },
    );
    if (!result) return;
    try {
      await this.adminService.resetPassword(result);
      this.notify.success('password.resetSuccess');
    } catch {
      this.notify.error('error.generic');
    }
  }
}