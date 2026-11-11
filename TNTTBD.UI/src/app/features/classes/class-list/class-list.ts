import { Component, inject, signal, computed, linkedSignal, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { NotificationService } from '../../../core/services/notification.service';
import { DialogService } from '../../../core/services/dialog.service';
import { CatechismClass, CatechismYear } from '../../../shared/models';
import { UserRole } from '../../../shared/enums';
import { TranslatePipe } from '../../../core/i18n/translate.pipe';
import { YearFormDialog, YearFormData, YearFormResult } from '../../../shared/components/year-form-dialog';
import { ClassFormDialog } from '../../../shared/components/class-form-dialog';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { EmptyState } from '../../../shared/components/empty-state';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { AuthService } from '../../../core/auth/auth.service';
import { ClassesService } from '../classes.service';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { RowActionsBottomSheet, RowActionsData, RowAction } from '../../../shared/components/row-actions-bottom-sheet/row-actions-bottom-sheet';
import { TranslationService } from '../../../core/i18n/translation.service';
import { DateLocalePipe } from '../../../shared/pipes/date-locale.pipe';

@Component({
  selector: 'app-class-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink,
    MatTableModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatPaginatorModule,
    MatSelectModule,
    TranslatePipe,
    EmptyState,
    MatListModule,
    MatProgressSpinner,
    DateLocalePipe
  ],
  templateUrl: './class-list.html',
  styleUrl: './class-list.scss',

  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [ClassesService],
})
export class ClassList {
  private classesService = inject(ClassesService);
  private dialogs = inject(DialogService);
  private notify = inject(NotificationService);
  private router = inject(Router);
  protected auth = inject(AuthService);
  #translate = inject(TranslationService);

  protected readonly UserRole = UserRole;

  protected years = this.classesService.years;
  protected isLoading = this.classesService.isListLoading;
  // Defaults to the latest year once loaded; keeps the user's choice (matched by id) when the list reloads
  protected selectedYear = linkedSignal<CatechismYear[], CatechismYear | null>({
    source: () => this.classesService.years(),
    computation: (years, previous) => {
      if (years.length === 0) return null;
      const kept = previous?.value ? years.find((y) => y.id === previous.value!.id) : undefined;
      return kept ?? [...years].sort((a, b) => b.startDate.localeCompare(a.startDate))[0];
    },
  });
  protected searchQuery = signal<string>('');

  protected myClassIds = computed(() => {
    const user = this.auth.currentUser();
    if (!user?.catechistId) return new Set<string>();
    return new Set(
      this.classesService
        .classCatechists()
        .filter((c) => c.catechistId === user.catechistId)
        .map((c) => c.catechismClassId),
    );
  });

  protected filtered = computed(() => {
    const yid = this.selectedYear()?.id;
    const query = this.searchQuery().toLowerCase().trim();
    let list = this.classesService.classes();

    if (yid) {
      list = list.filter((c) => c.catechismYearId === yid);
    }

    if (query) {
      list = list.filter((c) => c.className.toLowerCase().includes(query));
    }

    const mine = this.myClassIds();
    if (mine.size === 0) return list;
    return [...list].sort((a, b) => {
      const aIsMine = mine.has(a.id) ? 0 : 1;
      const bIsMine = mine.has(b.id) ? 0 : 1;
      return aIsMine - bIsMine;
    });
  });

  protected pageIndex = signal(0);
  protected pageSize = signal(50);

  protected paginatedFiltered = computed(() => {
    const list = this.filtered();
    const start = this.pageIndex() * this.pageSize();
    return list.slice(start, start + this.pageSize());
  });

  protected cols = ['name', 'actions'];

  protected onPageChange(event: PageEvent): void {
    this.pageIndex.set(event.pageIndex);
    this.pageSize.set(event.pageSize);
  }

  protected canManage(): boolean {
    return this.auth.hasRole([UserRole.Admin]);
  }

  protected isMyClass(id: string): boolean {
    return this.myClassIds().has(id);
  }

  protected async newYear(): Promise<void> {
    const v = await this.dialogs.open(YearFormDialog, { data: {} });
    if (!v) return;
    await this.classesService.createYear(v);
    this.notify.success('Saved');
  }

  protected async editYear(): Promise<void> {
    const year = this.selectedYear();
    if (!year || !this.canManage()) return;
    const v = await this.dialogs.open<YearFormDialog, YearFormData, YearFormResult>(YearFormDialog, {
      data: { year },
    });
    if (!v?.startDate || !v.endDate) return;
    await this.classesService.updateYear({
      catechismYearId: year.id,
      startDate: v.startDate,
      endDate: v.endDate,
      theme: v.theme,
    });
    this.notify.success('Saved');
  }

  protected async newClass(): Promise<void> {
    const v = await this.dialogs.open(ClassFormDialog, {
      data: { catechismYearId: this.selectedYear()?.id ?? null },
    });
    if (!v) return;
    await this.classesService.createClass(v);
    this.notify.success('Saved');
  }

  protected async editClass(catechismClass: CatechismClass): Promise<void> {
    const v = await this.dialogs.open(ClassFormDialog, {
      data: { catechismClass, years: this.years() },
    });
    if (!v) return;
    await this.classesService.updateClass(v);
    this.notify.success('Saved');
  }

  protected async deleteClass(catechismClass: CatechismClass): Promise<void> {
    const ok = await this.dialogs.confirmDelete(`Delete class "${catechismClass.className}"?`);
    if (!ok) return;
    await this.classesService.deleteClass(catechismClass.id);
    this.notify.success(this.#translate.t('common.deleted'));
  }

  protected async openActions(row: CatechismClass): Promise<void> {
    const canManage = this.canManage();
    const data: RowActionsData = { canView: true, canEdit: canManage, canDelete: canManage };
    const action = await this.dialogs.openBottomSheet<RowActionsBottomSheet, RowActionsData, RowAction>(
      RowActionsBottomSheet,
      { data },
    );
    switch (action) {
      case 'view':
        await this.router.navigate(['/app/classes/detail'], { queryParams: { id: row.id } });
        break;
      case 'edit':
        await this.editClass(row);
        break;
      case 'delete':
        await this.deleteClass(row);
        break;
    }
  }
}
