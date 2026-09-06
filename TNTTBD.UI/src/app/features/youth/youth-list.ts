import { Component, inject, signal , ChangeDetectionStrategy, effect, computed} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { ApiService } from '../../core/services/api.service';
import {
  EucharisticYouth,
  PaginatedResult,
} from '../../shared/models';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../shared/pipes/full-name.pipe';
import { DateLocalePipe } from '../../shared/pipes/date-locale.pipe';

import { EmptyState } from '../../shared/components/empty-state';
import { FormsModule } from '@angular/forms';

function emptyPage<T>(): PaginatedResult<T> {
  return { items: [], total: 0, page: 1, pageSize: 50 };
}

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
    FormsModule
  ],
  templateUrl: './youth-list.html',
  styleUrl: './youth-list.scss',

  changeDetection: ChangeDetectionStrategy.OnPush
})
export class YouthList {
  private api = inject(ApiService);

  protected data = signal<PaginatedResult<EucharisticYouth>>({
    items: [],
    total: 0,
    page: 1,
    pageSize: 50,
  });

  protected displayedColumns = ['code', 'fullName', 'dateOfBirth', 'gender'];
  protected searchQuery = signal<string>('');
  protected params = computed(() => ({
    search: this.searchQuery(),
    page: this.data().page,
    pageSize: this.data().pageSize
  }));

  constructor() {
    this.loadData();
  }

  private loadData() {
    const p = this.params();
    this.api
      .list<EucharisticYouth>('/youths', p)
      .subscribe(result => this.data.set(result));
  }
  
  onPageChange(event: PageEvent) {
    this.data.update(d => ({
      ...d,
      page: event.pageIndex + 1,
      pageSize: event.pageSize
    }));
    this.loadData();
  }
  
  onSearch(query: string) {
    this.searchQuery.set(query);
    this.data.update(d => ({
      ...d,
      page: 1,
    }));
    this.loadData();
  }
}
