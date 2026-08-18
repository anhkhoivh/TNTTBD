import { Component, inject, signal, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { toSignal } from '@angular/core/rxjs-interop';
import { BehaviorSubject, switchMap } from 'rxjs';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ApiService } from '../../core/services/api.service';
import { NotificationService } from '../../core/services/notification.service';
import { User } from '../../shared/models';
import { UserRole } from '../../shared/enums';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { ConfirmDialog, ConfirmData } from '../../shared/components/confirm-dialog';
import { UserFormDialog, UserFormData, UserFormResult } from './user-form-dialog';

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
    MatDialogModule,
    TranslatePipe,
  ],
  templateUrl: './admin.html',
  styleUrl: './admin.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AdminPage {
  private api = inject(ApiService);
  private dialog = inject(MatDialog);
  private notify = inject(NotificationService);

  protected readonly UserRole = UserRole;

  private reload$ = new BehaviorSubject<void>(undefined);
  protected showInactive = signal(false);

  protected data = toSignal(
    this.reload$.pipe(switchMap(() => this.api.list<User>('/users', { page: 1, pageSize: 100 }))),
    { initialValue: { items: [], total: 0, page: 1, pageSize: 100 } },
  );

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

  protected create(): void {
    const ref = this.dialog.open<UserFormDialog, UserFormData, UserFormResult>(UserFormDialog, {
      data: { mode: 'create' },
    });
    ref.afterClosed().subscribe((result) => {
      if (!result?.create) return;
      this.api.post('/users', result.create).subscribe({
        next: () => {
          this.notify.info('user.created');
          this.reload$.next();
        },
        error: () => this.notify.error('error.generic'),
      });
    });
  }

  protected edit(user: User): void {
    const ref = this.dialog.open<UserFormDialog, UserFormData, UserFormResult>(UserFormDialog, {
      data: { mode: 'edit', user },
    });
    ref.afterClosed().subscribe((result) => {
      if (!result?.update) return;
      this.api.patch('/users', result.update).subscribe({
        next: () => {
          this.notify.info('user.updated');
          this.reload$.next();
        },
        error: () => this.notify.error('error.generic'),
      });
    });
  }

  protected toggleActive(user: User): void {
    this.api
      .patch('/users', { id: user.id, isActive: !user.isActive })
      .subscribe({
        next: () => this.reload$.next(),
        error: () => this.notify.error('error.generic'),
      });
  }

  protected remove(user: User): void {
    const ref = this.dialog.open<ConfirmDialog, ConfirmData, boolean>(ConfirmDialog, {
      data: {
        message: `${user.userName} (${user.email})`,
        confirmKey: 'user.deactivate',
        destructive: true,
      },
    });
    ref.afterClosed().subscribe((ok) => {
      if (!ok) return;
      this.api.delete('/users', { id: user.id }).subscribe({
        next: () => {
          this.notify.info('user.deactivated');
          this.reload$.next();
        },
        error: () => this.notify.error('error.generic'),
      });
    });
  }
}

