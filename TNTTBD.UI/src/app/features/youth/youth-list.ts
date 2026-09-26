import { Component, inject, signal, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../shared/pipes/full-name.pipe';
import { DateLocalePipe } from '../../shared/pipes/date-locale.pipe';

import { EmptyState } from '../../shared/components/empty-state';
import { FormsModule } from '@angular/forms';
import { YouthService } from './youth.service';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { DialogService } from '../../core/services/dialog.service';
import { NotificationService } from '../../core/services/notification.service';
import { AuthService } from '../../core/auth/auth.service';
import { UserRole } from '../../shared/enums';
import { RowActionsBottomSheet, RowActionsData, RowAction } from '../../shared/components/row-actions-bottom-sheet/row-actions-bottom-sheet';
import { EucharisticYouth } from '../../shared/models';

@Component({
  selector: 'app-youth-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatPaginatorModule,
    TranslatePipe,
    FullNamePipe,
    DateLocalePipe,
    EmptyState,
    FormsModule,
    MatProgressSpinner
],
  templateUrl: './youth-list.html',
  styleUrl: './youth-list.scss',

  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [YouthService],
})
export class YouthList {
  private youthService = inject(YouthService);
  private auth = inject(AuthService);
  private router = inject(Router);
  private dialogs = inject(DialogService);
  private notify = inject(NotificationService);

  protected data = this.youthService.list;
  protected isLoading = this.youthService.isListLoading;

  protected displayedColumns = ['code', 'fullName', 'dateOfBirth', 'gender', 'actions'];
  protected searchQuery = signal('');

  onPageChange(event: PageEvent) {
    this.youthService.listParams.update((p) => ({
      ...p,
      page: event.pageIndex + 1,
      pageSize: event.pageSize,
    }));
  }

  onSearch(query: string) {
    this.searchQuery.set(query);
    this.youthService.listParams.update((p) => ({ ...p, search: query, page: 1 }));
  }

  private canDelete(): boolean {
    return this.auth.hasRole([UserRole.Admin]);
  }

  protected async openActions(row: EucharisticYouth): Promise<void> {
    const data: RowActionsData = { canView: true, canEdit: true, canDelete: this.canDelete() };
    const action = await this.dialogs.openBottomSheet<RowActionsBottomSheet, RowActionsData, RowAction>(
      RowActionsBottomSheet,
      { data },
    );
    switch (action) {
      case 'view':
        await this.router.navigate(['/app/youth/detail'], { queryParams: { id: row.id } });
        break;
      case 'edit':
        await this.router.navigate(['/app/youth/edit'], { queryParams: { id: row.id } });
        break;
      case 'delete': {
        const ok = await this.dialogs.confirmDelete(`Delete youth "${row.fullName.givenName}"?`);
        if (!ok) return;
        await this.youthService.remove(row.id);
        this.notify.success('Deleted');
        break;
      }
    }
  }
}
