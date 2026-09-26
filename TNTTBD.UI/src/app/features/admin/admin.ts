import { Component, inject, signal, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { NotificationService } from '../../core/services/notification.service';
import { DialogService } from '../../core/services/dialog.service';
import { User } from '../../shared/models';
import { UserRole } from '../../shared/enums';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { UserFormDialog, UserFormData, UserFormResult } from './user-form-dialog';
import { ResetPasswordDialog, ResetPasswordData } from './reset-password-dialog';
import { AdminService } from './admin.service';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatTableModule,
    MatChipsModule,
    MatIconModule,
    MatButtonModule,
    MatSlideToggleModule,
    TranslatePipe,
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

  protected readonly UserRole = UserRole;

  protected showInactive = signal(false);

  protected data = this.adminService.users;

  protected cols = ['userName', 'email', 'role', 'link', 'active', 'actions'];

  protected filtered(): User[] {
    const all = this.data().items;
    return this.showInactive() ? all : all.filter((u) => u.isActive);
  }

  protected roleKey(role: UserRole): string {
    return `user.role.${role}`;
  }

  protected linkLabel(u: User): string {
    if (u.catechistId) return `Catechist · ${u.catechistId.slice(0, 8)}`;
    if (u.eucharisticYouthId) return `Youth · ${u.eucharisticYouthId.slice(0, 8)}`;
    if (u.contactId) return `Contact · ${u.contactId.slice(0, 8)}`;
    return '';
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

  protected async edit(user: User): Promise<void> {
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

  protected async toggleActive(user: User): Promise<void> {
    try {
      await this.adminService.toggleActive(user.id, !user.isActive);
    } catch {
      this.notify.error('error.generic');
    }
  }

  protected async remove(user: User): Promise<void> {
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

  protected async resetPassword(user: User): Promise<void> {
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

