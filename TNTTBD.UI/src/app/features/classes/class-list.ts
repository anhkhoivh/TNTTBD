import { Component, inject, signal, computed, ChangeDetectionStrategy, effect } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ApiService } from '../../core/services/api.service';
import { NotificationService } from '../../core/services/notification.service';
import { AuthService } from '../../core/auth/auth.service';
import { CatechismClass, CatechismClassCatechist, CatechismYear } from '../../shared/models';
import { UserRole } from '../../shared/enums';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { YearFormDialog } from '../../shared/components/year-form-dialog';
import { ClassFormDialog } from '../../shared/components/class-form-dialog';
import { ConfirmDialog } from '../../shared/components/confirm-dialog';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { EmptyState } from '../../shared/components/empty-state';
import { MatInputModule } from '@angular/material/input';

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
    MatDialogModule,
    TranslatePipe,
    EmptyState,
  ],
  templateUrl: './class-list.html',
  styles: [
    `
      .toolbar {
        display: flex;
        gap: 1rem;
        align-items: center;
        flex-wrap: wrap;
        margin-bottom: 1rem;
      }
      .toolbar .spacer { flex: 1; }
      mat-form-field { min-width: 220px; }
      .my-class { font-weight: 600; }
      .star-icon { font-size: 18px; height: 18px; width: 18px; vertical-align: middle; color: var(--mat-sys-primary); margin-right: 4px; }
    `,
  ],

  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ClassList {
  private api = inject(ApiService);
  private dialog = inject(MatDialog);
  private notify = inject(NotificationService);
  protected auth = inject(AuthService);

  protected readonly UserRole = UserRole;

  protected years = signal<CatechismYear[]>([]);
  protected classes = signal<CatechismClass[]>([]);
  protected selectedYearId = signal<string | null>(null);
  protected myClassIds = signal<Set<string>>(new Set());
  protected searchQuery = signal<string>('');

  protected filtered = computed(() => {
    const yid = this.selectedYearId();
    const query = this.searchQuery().toLowerCase().trim();
    let list = this.classes();
    
    if (yid) {
      console.log(list);
      list = list.filter((c) => c.catechismYearId === yid);
      console.log("id= ", yid);
      console.log(list);
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

  constructor() {
    this.reload();
  }

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

  private reload(): void {
    this.api.list<CatechismYear>('/catechism-years').subscribe((r) => {
      this.years.set(r.items);
      if (!this.selectedYearId() && r.items.length > 0) {
        const latest = [...r.items].sort((a, b) => b.startDate.localeCompare(a.startDate))[0];
        this.selectedYearId.set(latest.id);
      }
    });
    this.api.list<CatechismClass>('/classes').subscribe((r) => this.classes.set(r.items));

    const user = this.auth.currentUser();
    if (user?.catechistId) {
      this.api.list<CatechismClassCatechist>('/class-catechist').subscribe((r) => {
        const ids = new Set(
          r.items.filter((c) => c.catechistId === user.catechistId).map((c) => c.catechismClassId),
        );
        this.myClassIds.set(ids);
      });
    }
  }

  protected newYear(): void {
    this.dialog
      .open(YearFormDialog, { data: {} })
      .afterClosed()
      .subscribe((v) => {
        if (!v) return;
        this.api.post('/catechism-years', v).subscribe(() => {
          this.notify.success('Saved');
          this.reload();
        });
      });
  }

  protected newClass(): void {
    this.dialog
      .open(ClassFormDialog, {
        data: { years: this.years(), defaultYearId: this.selectedYearId() ?? undefined },
      })
      .afterClosed()
      .subscribe((v) => {
        if (!v) return;
        this.api.post('/classes', v).subscribe(() => {
          this.notify.success('Saved');
          this.reload();
        });
      });
  }

  protected editClass(catechismClass: CatechismClass): void {
    this.dialog
      .open(ClassFormDialog, { data: { catechismClass, years: this.years() } })
      .afterClosed()
      .subscribe((v) => {
        if (!v) return;
        this.api.patch('/classes', v).subscribe(() => {
          this.notify.success('Saved');
          this.reload();
        });
      });
  }

  protected deleteClass(catechismClass: CatechismClass): void {
    this.dialog
      .open(ConfirmDialog, {
        data: {
          message: `Delete class "${catechismClass.className}"?`,
          destructive: true,
          confirmKey: 'common.delete',
        },
      })
      .afterClosed()
      .subscribe((ok) => {
        if (!ok) return;
        this.api.delete('/classes', { catechismClassId: catechismClass.id }).subscribe(() => {
          this.notify.success('Deleted');
          this.reload();
        });
      });
  }
}
