import { Component, inject, signal , ChangeDetectionStrategy} from '@angular/core';
import { CommonModule, NgOptimizedImage } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { switchMap, of, catchError, map } from 'rxjs';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import { ApiService } from '../../core/services/api.service';
import { PhotoService } from '../../core/services/photo.service';
import {
  CatechismClass,
  CatechismClassEucharisticYouth,
  EucharisticYouth,
  EucharisticYouthContact,
  PaginatedResult,
} from '../../shared/models';
import { ContactRelationType, EnrollmentStatus, MilestoneType } from '../../shared/enums';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../shared/pipes/full-name.pipe';
import { DateLocalePipe } from '../../shared/pipes/date-locale.pipe';

function emptyPage<T>(): PaginatedResult<T> {
  return { items: [], total: 0, page: 1, pageSize: 50 };
}

@Component({
  selector: 'app-youth-detail',
  standalone: true,
  imports: [
    NgOptimizedImage,
    CommonModule,
    RouterLink,
    MatCardModule,
    MatTabsModule,
    MatIconModule,
    MatChipsModule,
    MatButtonModule,
    MatListModule,
    TranslatePipe,
    FullNamePipe,
    DateLocalePipe,
  ],
  templateUrl: './youth-detail.html',
  styleUrl: './youth-detail.scss',

  changeDetection: ChangeDetectionStrategy.OnPush
})
export class YouthDetail {
  private route = inject(ActivatedRoute);
  private api = inject(ApiService);
  protected photoService = inject(PhotoService);

  /** Initial load: youth profile only (milestones + photos embedded) */
  protected data = toSignal(
    this.route.queryParamMap.pipe(
      switchMap((qp) => {
        const id = qp.get('id');
        if (!id) return of(null);
        return this.api.get<EucharisticYouth>('/youths/detail', { id }).pipe(catchError(() => of(null)));
      }),
    ),
    { initialValue: null },
  );

  /** Lazy-loaded contacts tab */
  protected contactsData = signal<PaginatedResult<EucharisticYouthContact> | null>(null);
  private contactsLoaded = false;

  /** Lazy-loaded class history tab */
  protected historyData = signal<PaginatedResult<CatechismClassEucharisticYouth> | null>(null);
  private historyLoaded = false;

  protected onTabChange(index: number): void {
    if (index === 3 && !this.contactsLoaded) {
      this.contactsLoaded = true;
      this.loadContacts();
    }
    if (index === 4 && !this.historyLoaded) {
      this.historyLoaded = true;
      this.loadHistory();
    }
  }

  private loadContacts(): void {
    const id = this.route.snapshot.queryParamMap.get('id');
    if (!id) return;
    this.api
      .get<EucharisticYouthContact[]>('/youths/contacts', { youthId: id })
      .pipe(
        map((items) => ({ items, total: items.length, page: 1, pageSize: items.length })),
        catchError(() => of(emptyPage<EucharisticYouthContact>())),
      )
      .subscribe((data) => this.contactsData.set(data));
  }

  private loadHistory(): void {
    const id = this.route.snapshot.queryParamMap.get('id');
    if (!id) return;
    this.api
      .get<CatechismClassEucharisticYouth[]>('/youths/enrollments', { youthId: id })
      .pipe(
        map((items) => ({ items, total: items.length, page: 1, pageSize: items.length })),
        catchError(() => of(emptyPage<CatechismClassEucharisticYouth>())),
      )
      .subscribe((data) => this.historyData.set(data));
  }

  protected classOf(id: string, list: CatechismClass[]): CatechismClass | undefined {
    return list.find((c) => c.id === id);
  }

  protected milestoneKey(type: MilestoneType): string {
    const map: Record<MilestoneType, string> = {
      [MilestoneType.Baptism]: 'milestone.baptism',
      [MilestoneType.FirstPenance]: 'milestone.firstPenance',
      [MilestoneType.FirstCommunion]: 'milestone.firstCommunion',
      [MilestoneType.Confirmation]: 'milestone.confirmation',
      [MilestoneType.SolemnCommunion]: 'milestone.solemnCommunion',
      [MilestoneType.Graduation]: 'milestone.graduation',
    };
    return map[type] ?? '';
  }

  protected relationKey(r: ContactRelationType): string {
    return `contact.relation.${r}`;
  }

  protected enrollmentStatusKey(s: EnrollmentStatus): string {
    return `enrollment.status.${s}`;
  }
}
