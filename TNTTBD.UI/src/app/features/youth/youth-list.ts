import { Component, inject, signal , ChangeDetectionStrategy} from '@angular/core';
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

  /** Catechist's current class ID (needed by youth-form for "add to my class") */
  protected myCurrentClassId = signal<string | null>(null);

  constructor() {
    const user = this.auth.currentUser();
    if (user?.userRole === UserRole.Catechist && user.catechistId) {
      // Catechist: load only youth in their current-year class
      this.api
        .get<CatechistCurrentClass | null>('/catechists/current-class')
        .pipe(
          switchMap((current) => {
            if (!current) {
              return of(emptyPage<EucharisticYouth>());
            }
            this.myCurrentClassId.set(current.classId);
            return this.api.list<EucharisticYouth>('/youths', { classId: current.classId });
          }),
          catchError(() => of(emptyPage<EucharisticYouth>())),
        )
        .subscribe((result) => this.data.set(result));
    } else {
      // Admin: load all youth
      this.api
        .list<EucharisticYouth>('/youths')
        .subscribe((result) => this.data.set(result));
    }
  }
}
