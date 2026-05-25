import { Component, inject, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { forkJoin, of, switchMap } from 'rxjs';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { DateLocalePipe } from '../../shared/pipes/date-locale.pipe';
import { AuthService } from '../../core/auth/auth.service';
import { ApiService } from '../../core/services/api.service';
import {
  CatechismClass,
  CatechismClassCatechist,
  CatechismClassEucharisticYouth,
  CatechismSession,
  CatechismYear,
  Catechist,
  EucharisticYouth,
} from '../../shared/models';
import { UserRole } from '../../shared/enums';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    MatListModule,
    TranslatePipe,
    DateLocalePipe,
  ],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss',
})
export class Dashboard {
  protected auth = inject(AuthService);
  private api = inject(ApiService);

  protected readonly UserRole = UserRole;
  protected role = computed(() => this.auth.currentUser()?.userRole);

  protected data = toSignal(
    of(this.auth.currentUser()).pipe(
      switchMap(() =>
        forkJoin({
          years: this.api.list<CatechismYear>('/catechism-years'),
          classes: this.api.list<CatechismClass>('/classes'),
          sessions: this.api.list<CatechismSession>('/sessions'),
          enrollments: this.api.list<CatechismClassEucharisticYouth>('/class-youth'),
          classCatechists: this.api.list<CatechismClassCatechist>('/class-catechist'),
          youth: this.api.list<EucharisticYouth>('/youths'),
          catechists: this.api.list<Catechist>('/catechists'),
        }),
      ),
    ),
    { initialValue: null },
  );

  protected currentYearLabel = computed(() => {
    const ys = this.data()?.years.items ?? [];
    if (ys.length === 0) return '—';
    const latest = [...ys].sort((a, b) => b.startDate.localeCompare(a.startDate))[0];
    return `${latest.startDate.slice(0, 4)}–${latest.endDate.slice(0, 4)}`;
  });

  protected upcomingSessions = computed(() => {
    const list = this.data()?.sessions.items ?? [];
    const now = Date.now();
    return list
      .filter((s) => new Date(s.dateTime).getTime() >= now)
      .sort((a, b) => a.dateTime.localeCompare(b.dateTime))
      .slice(0, 5);
  });

  protected myCatechistClasses = computed(() => {
    const u = this.auth.currentUser();
    const d = this.data();
    if (!u?.catechistId || !d) return [];
    // Find latest year
    const years = d.years.items;
    if (years.length === 0) return [];
    const latestYear = [...years].sort((a, b) => b.startDate.localeCompare(a.startDate))[0];
    const ids = new Set(
      d.classCatechists.items.filter((c) => c.catechistId === u.catechistId).map((c) => c.catechismClassId),
    );
    return d.classes.items.filter((c) => ids.has(c.id) && c.catechismYearId === latestYear.id);
  });

  protected myYouthEnrollments = computed(() => {
    const u = this.auth.currentUser();
    const d = this.data();
    if (!u?.eucharisticYouthId || !d) return [];
    return d.enrollments.items.filter((e) => e.eucharisticYouthId === u.eucharisticYouthId);
  });

  protected classOf(id: string): CatechismClass | undefined {
    return this.data()?.classes.items.find((c) => c.id === id);
  }

  protected adminTotals = computed(() => {
    const d = this.data();
    if (!d) return null;
    return {
      youth: d.youth.total,
      catechists: d.catechists.total,
      classes: d.classes.total,
      years: d.years.total,
    };
  });
}
