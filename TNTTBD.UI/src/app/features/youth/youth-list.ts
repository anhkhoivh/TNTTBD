import { Component, inject, signal , ChangeDetectionStrategy, effect} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { of, switchMap, catchError } from 'rxjs';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { ApiService } from '../../core/services/api.service';
import { AuthService } from '../../core/auth/auth.service';
import {
  CatechistCurrentClass,
  EucharisticYouth,
  PaginatedResult,
} from '../../shared/models';
import { UserRole } from '../../shared/enums';
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
  private auth = inject(AuthService);

  protected data = signal<PaginatedResult<EucharisticYouth>>({
    items: [],
    total: 0,
    page: 1,
    pageSize: 50,
  });

  protected displayedColumns = ['code', 'fullName', 'dateOfBirth', 'gender', 'actions'];
  protected searchQuery = signal<string>('');

  constructor() {
    effect(() => {
      this.loadData();
    });
  }

  private loadData() {
    this.api
      .list<EucharisticYouth>('/youths', {
        search: this.searchQuery(),
        page: this.data().page,
        pageSize: this.data().pageSize
      })
      .subscribe(result => this.data.set(result));
  }
  
  onPageChange(event: PageEvent) {
    this.data.update(d => ({
      ...d,
      page: event.pageIndex + 1,
      pageSize: event.pageSize
    }));
  }
  
}
