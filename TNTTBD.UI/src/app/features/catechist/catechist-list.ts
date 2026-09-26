import { Component, inject , ChangeDetectionStrategy, signal} from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../shared/pipes/full-name.pipe';
import { DateLocalePipe } from '../../shared/pipes/date-locale.pipe';
import { AuthService } from '../../core/auth/auth.service';
import { MatListModule } from '@angular/material/list';
import { CatechistService } from './catechist.service';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { DialogService } from '../../core/services/dialog.service';
import { NotificationService } from '../../core/services/notification.service';
import { UserRole } from '../../shared/enums';
import { RowActionsBottomSheet, RowActionsData, RowAction } from '../../shared/components/row-actions-bottom-sheet/row-actions-bottom-sheet';
import { Catechist } from '../../shared/models';

@Component({
  selector: 'app-catechist-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatTableModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    TranslatePipe,
    FullNamePipe,
    DateLocalePipe,
    MatListModule,
    MatProgressSpinner
],
  templateUrl: './catechist-list.html',
  styleUrl: './catechist-list.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [CatechistService],
})
export class CatechistList {
  private catechistService = inject(CatechistService);
  protected isLoading = this.catechistService.isListLoading;
  private auth = inject(AuthService);
  private router = inject(Router);
  private dialogs = inject(DialogService);
  private notify = inject(NotificationService);
  protected data = this.catechistService.catechists;
  protected cols = ['index', 'name', 'dob', 'gender', 'actions'];
  private user = signal(this.auth.currentUser());
  protected myCatechistId = signal<string | null>(null);
  protected isMe(id: string): boolean {
    return this.user()?.catechistId != null && this.user()?.catechistId === id;
  }

  private canEdit(row: Catechist): boolean {
    const user = this.auth.currentUser();
    if (!user) return false;
    if (user.userRole === UserRole.Admin) return true;
    return user.catechistId === row.id;
  }

  private canDelete(): boolean {
    return this.auth.hasRole([UserRole.Admin]);
  }

  protected async openActions(row: Catechist): Promise<void> {
    const data: RowActionsData = { canView: true, canEdit: this.canEdit(row), canDelete: this.canDelete() };
    const action = await this.dialogs.openBottomSheet<RowActionsBottomSheet, RowActionsData, RowAction>(
      RowActionsBottomSheet,
      { data },
    );
    switch (action) {
      case 'view':
        await this.router.navigate(['/app/catechists/detail'], { queryParams: { id: row.id } });
        break;
      case 'edit':
        await this.router.navigate(['/app/catechists/edit'], { queryParams: { id: row.id } });
        break;
      case 'delete': {
        const ok = await this.dialogs.confirmDelete(`Delete catechist "${row.fullName.givenName}"?`);
        if (!ok) return;
        await this.catechistService.remove(row.id);
        this.notify.success('Deleted');
        break;
      }
    }
  }
}
