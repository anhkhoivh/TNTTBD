import { Component, inject, computed , ChangeDetectionStrategy} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { toSignal, rxResource } from '@angular/core/rxjs-interop';
import { of, catchError } from 'rxjs';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { DateLocalePipe } from '../../shared/pipes/date-locale.pipe';
import { FullNamePipe } from '../../shared/pipes/full-name.pipe';
import { AuthService } from '../../core/auth/auth.service';
import { ApiService } from '../../core/services/api.service';
import { ContactDetail, DashboardSummary } from '../../shared/models';
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
    FullNamePipe,
  ],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss',

  changeDetection: ChangeDetectionStrategy.OnPush
})
export class Dashboard {
  protected auth = inject(AuthService);
  private api = inject(ApiService);

  protected readonly UserRole = UserRole;
  protected role = computed(() => this.auth.currentUser()?.userRole);
  protected isAdminOrCatechist = computed(() => this.auth.isAdminOrCatechist());

  protected data = toSignal(
    this.api.get<DashboardSummary>('/dashboard/summary').pipe(catchError(() => of(null))),
    { initialValue: null },
  );

  protected currentYearLabel = computed(() => this.data()?.currentYearTitle ?? '—');

  protected upcomingSessions = computed(() => this.data()?.upcomingSessions ?? []);

  protected myClasses = computed(() => this.data()?.myClasses ?? []);

  protected adminTotals = computed(() => this.data()?.totals ?? null);

  protected myYouthId = computed(() =>
    this.role() === UserRole.EucharisticYouth ? (this.auth.currentUser()?.eucharisticYouthId ?? null) : null,
  );

  private myContactRes = rxResource<ContactDetail | null, string>({
    params: () =>
      this.role() === UserRole.EucharisticYouthContact ? (this.auth.currentUser()?.contactId ?? '') : '',
    stream: ({ params }) =>
      params ? this.api.get<ContactDetail>('/contacts/detail', { id: params }).pipe(catchError(() => of(null))) : of(null),
  });
  protected myYouths = computed(() => this.myContactRes.value()?.youths ?? []);
}
