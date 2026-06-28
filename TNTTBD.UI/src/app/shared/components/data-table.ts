import {
  Component,
  ContentChildren,
  Directive,
  Input,
  QueryList,
  TemplateRef,
  input,
  computed,
  signal,
  inject,
  ChangeDetectionStrategy
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule, Sort } from '@angular/material/sort';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { TranslatePipe } from '../../core/i18n/translate.pipe';

/**
 * Directive to define a custom column template.
 * Usage:
 * ```html
 * <app-data-table [data]="items" [columns]="cols">
 *   <ng-template appColumnDef="name" let-row>{{ row.fullName | fullName }}</ng-template>
 * </app-data-table>
 * ```
 */
@Directive({
  selector: '[appColumnDef]',
  standalone: true,
})
export class ColumnDef {
  @Input('appColumnDef') column = '';
  constructor(public template: TemplateRef<unknown>) {}
}

export interface ColumnConfig {
  key: string;
  labelKey: string;
  sortable?: boolean;
}

@Component({
  selector: 'app-data-table',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatSortModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    TranslatePipe,
  ],
  template: `
    <div class="table-toolbar">
      @if (searchable()) {
        <mat-form-field appearance="outline" class="search-field">
          <mat-icon matPrefix>search</mat-icon>
          <input
            matInput
            [placeholder]="'common.search' | t"
            (input)="onSearch($event)"
          />
        </mat-form-field>
      }
      <ng-content select="[tableActions]"></ng-content>
    </div>

    <div class="table-container">
      <table
        mat-table
        [dataSource]="pagedData()"
        matSort
        (matSortChange)="onSort($event)"
      >
        @for (col of columns(); track col.key) {
          <ng-container [matColumnDef]="col.key">
            <th mat-header-cell *matHeaderCellDef [mat-sort-header]="col.sortable !== false ? col.key : ''">
              {{ col.labelKey | t }}
            </th>
            <td mat-cell *matCellDef="let row">
              @if (getTemplate(col.key); as tmpl) {
                <ng-container *ngTemplateOutlet="tmpl; context: { $implicit: row }"></ng-container>
              } @else {
                {{ row[col.key] }}
              }
            </td>
          </ng-container>
        }
        <tr mat-header-row *matHeaderRowDef="columnKeys()"></tr>
        <tr mat-row *matRowDef="let row; columns: columnKeys()"></tr>
      </table>
    </div>

    @if (paginate()) {
      <mat-paginator
        [length]="filteredData().length"
        [pageSize]="pageSize()"
        [pageSizeOptions]="[10, 25, 50]"
        (page)="onPage($event)"
        showFirstLastButtons
      ></mat-paginator>
    }
  `,
  styles: [
    `
      .table-toolbar {
        display: flex;
        align-items: center;
        gap: 1rem;
        margin-bottom: 1rem;
        flex-wrap: wrap;
      }
      .search-field {
        flex: 1;
        min-width: 200px;
        max-width: 360px;
      }
      .table-container {
        overflow-x: auto;
      }
      table {
        width: 100%;
      }
    `,
  ],

  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DataTable<T = unknown> {
  /** Full data array */
  readonly data = input<T[]>([]);
  /** Column definitions */
  readonly columns = input<ColumnConfig[]>([]);
  /** Enable client-side search filtering */
  readonly searchable = input(true);
  /** Enable pagination */
  readonly paginate = input(true);
  /** Default page size */
  readonly pageSize = input(10);
  /** Property keys to search against */
  readonly searchKeys = input<string[]>([]);

  @ContentChildren(ColumnDef) columnDefs!: QueryList<ColumnDef>;

  protected searchTerm = signal('');
  protected page = signal(0);
  protected sort = signal<Sort | null>(null);

  protected columnKeys = computed(() => this.columns().map((c) => c.key));

  protected filteredData = computed(() => {
    const term = this.searchTerm().toLowerCase();
    let items = this.data();
    if (term) {
      const keys = this.searchKeys().length > 0 ? this.searchKeys() : this.columnKeys();
      items = items.filter((row) =>
        keys.some((k) => {
          const val = (row as Record<string, unknown>)[k];
          return val != null && String(val).toLowerCase().includes(term);
        }),
      );
    }
    const s = this.sort();
    if (s && s.direction) {
      items = [...items].sort((a, b) => {
        const aVal = (a as Record<string, unknown>)[s.active];
        const bVal = (b as Record<string, unknown>)[s.active];
        const cmp = String(aVal ?? '').localeCompare(String(bVal ?? ''));
        return s.direction === 'asc' ? cmp : -cmp;
      });
    }
    return items;
  });

  protected pagedData = computed(() => {
    const items = this.filteredData();
    if (!this.paginate()) return items;
    const start = this.page() * this.pageSize();
    return items.slice(start, start + this.pageSize());
  });

  protected getTemplate(key: string): TemplateRef<unknown> | null {
    const def = this.columnDefs?.find((d) => d.column === key);
    return def?.template ?? null;
  }

  protected onSearch(ev: Event): void {
    this.searchTerm.set((ev.target as HTMLInputElement).value);
    this.page.set(0);
  }

  protected onSort(sort: Sort): void {
    this.sort.set(sort);
  }

  protected onPage(ev: PageEvent): void {
    this.page.set(ev.pageIndex);
  }
}
