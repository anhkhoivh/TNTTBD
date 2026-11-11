import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { AuthService } from '../../../core/auth/auth.service';
import { TranslatePipe } from '../../../core/i18n/translate.pipe';
import { TranslationService } from '../../../core/i18n/translation.service';
import { DialogService } from '../../../core/services/dialog.service';
import { NotificationService } from '../../../core/services/notification.service';
import { EmptyState } from '../../../shared/components/empty-state';
import {
  RowAction,
  RowActionsBottomSheet,
  RowActionsData,
} from '../../../shared/components/row-actions-bottom-sheet/row-actions-bottom-sheet';
import { MILESTONE_TYPE_KEYS, MilestoneType, UserRole } from '../../../shared/enums';
import { MilestoneEventSummary } from '../../../shared/models';
import { DateLocalePipe } from '../../../shared/pipes/date-locale.pipe';
import { MilestoneEventsService } from '../milestone-events.service';
import { downloadBlob, toIsoDate } from '../milestone-event-utils';

@Component({
  selector: 'app-milestone-event-list',
  standalone: true,
  imports: [
    FormsModule,
    RouterLink,
    MatButtonModule,
    MatCardModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatPaginatorModule,
    MatProgressSpinner,
    MatSelectModule,
    MatTableModule,
    TranslatePipe,
    DateLocalePipe,
    EmptyState,
  ],
  templateUrl: './milestone-event-list.html',
  styleUrl: './milestone-event-list.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [MilestoneEventsService],
})
export class MilestoneEventList {
  private service = inject(MilestoneEventsService);
  private router = inject(Router);
  private dialogs = inject(DialogService);
  private notify = inject(NotificationService);
  private translate = inject(TranslationService);
  private auth = inject(AuthService);

  protected readonly canEdit = this.auth.hasRole([UserRole.Admin, UserRole.Catechist]);
  protected readonly isAdmin = this.auth.hasRole([UserRole.Admin]);
  protected readonly typeKeys = MILESTONE_TYPE_KEYS;
  protected readonly types = Object.values(MilestoneType);
  protected readonly displayedColumns = [
    'name',
    'type',
    'date',
    'place',
    'participantCount',
    ...(this.canEdit ? ['actions'] : []),
  ];

  protected data = this.service.list;
  protected isLoading = this.service.isListLoading;

  protected search = signal('');
  protected type = signal<MilestoneType | ''>('');
  protected from = signal<Date | null>(null);
  protected to = signal<Date | null>(null);

  protected typeKey(row: MilestoneEventSummary): string {
    return MILESTONE_TYPE_KEYS[row.type];
  }

  protected applyFilters(): void {
    this.service.listParams.update((p) => ({
      ...p,
      search: this.search().trim(),
      type: this.type(),
      from: toIsoDate(this.from()) ?? '',
      to: toIsoDate(this.to()) ?? '',
      page: 1,
    }));
  }

  protected onPageChange(event: PageEvent): void {
    this.service.listParams.update((p) => ({ ...p, page: event.pageIndex + 1, pageSize: event.pageSize }));
  }

  protected async openActions(row: MilestoneEventSummary): Promise<void> {
    const data: RowActionsData = {
      canView: true,
      canEdit: this.canEdit,
      canDelete: this.isAdmin && row.participantCount === 0,
      extraActions: [{ key: 'export', icon: 'download', labelKey: 'milestoneEvent.exportExcel' }],
    };
    const action = await this.dialogs.openBottomSheet<RowActionsBottomSheet, RowActionsData, RowAction | string>(
      RowActionsBottomSheet,
      { data },
    );
    if (action === 'view') {
      await this.router.navigate(['/app/milestone-events', row.id]);
    } else if (action === 'edit') {
      await this.router.navigate(['/app/milestone-events/edit'], { queryParams: { id: row.id } });
    } else if (action === 'export') {
      try {
        const blob = await this.service.exportExcel(row.id, this.translate.currentLang());
        downloadBlob(blob, `${row.name}.xlsx`);
      } catch {
        // error toast already shown by global http error handling
      }
    } else if (action === 'delete') {
      const ok = await this.dialogs.confirmDelete(
        this.translate.t('milestoneEvent.deleteConfirm').replace('{name}', row.name),
      );
      if (!ok) return;
      try {
        await this.service.remove(row.id);
        this.notify.success(this.translate.t('common.deleted'));
      } catch {
        // error toast already shown by global http error handling
      }
    }
  }
}
