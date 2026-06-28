import { Component, inject, signal , ChangeDetectionStrategy} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { forkJoin, map, of, switchMap } from 'rxjs';
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
  CatechismClassCatechist,
  CatechismClassEucharisticYouth,
  CatechismYear,
  EucharisticYouth,
  PaginatedResult,
} from '../../shared/models';
import { UserRole } from '../../shared/enums';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../shared/pipes/full-name.pipe';
import { DateLocalePipe } from '../../shared/pipes/date-locale.pipe';

import { EmptyState } from '../../shared/components/empty-state';

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
      forkJoin({
        years: this.api.list<CatechismYear>('/catechism-years'),
        assignments: this.api.list<CatechismClassCatechist>('/class-catechist'),
      })
        .pipe(
          switchMap(({ years, assignments }) => {
            const latestYear = [...years.items].sort((a, b) =>
              b.startDate.localeCompare(a.startDate),
            )[0];
            const myAssignment = assignments.items.find(
              (c) =>
                c.catechistId === user.catechistId &&
                // we need to cross-ref with classes to check year
                true,
            );
            // Get classes to find current-year class
            return this.api.list<{ id: string; catechismYearId: string }>('/classes').pipe(
              switchMap((classes) => {
                const currentYearClassIds = new Set(
                  classes.items
                    .filter((c) => c.catechismYearId === latestYear?.id)
                    .map((c) => c.id),
                );
                const myClassId = assignments.items.find(
                  (a) =>
                    a.catechistId === user.catechistId &&
                    currentYearClassIds.has(a.catechismClassId),
                )?.catechismClassId;
                if (!myClassId) return of({ items: [], total: 0, page: 1, pageSize: 50 } as PaginatedResult<EucharisticYouth>);
                this.myCurrentClassId.set(myClassId);
                return forkJoin({
                  enrollments: this.api.list<CatechismClassEucharisticYouth>('/class-youth', {
                    classId: myClassId,
                  }),
                  youth: this.api.list<EucharisticYouth>('/youths'),
                }).pipe(
                  map(({ enrollments, youth }) => {
                    const enrolledIds = new Set(
                      enrollments.items.map((e) => e.eucharisticYouthId),
                    );
                    const filtered = youth.items.filter((y) => enrolledIds.has(y.id));
                    return { ...youth, items: filtered, total: filtered.length };
                  }),
                );
              }),
            );
          }),
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
