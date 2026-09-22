import { Component, inject, effect, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule, Location, NgOptimizedImage } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import { PhotoService } from '../../core/services/photo.service';
import { CatechismClass } from '../../shared/models';
import { ContactRelationType, EnrollmentStatus, MilestoneType } from '../../shared/enums';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../shared/pipes/full-name.pipe';
import { DateLocalePipe } from '../../shared/pipes/date-locale.pipe';
import { YouthService } from './youth.service';

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

  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [YouthService],
})
export class YouthDetail {
  private route = inject(ActivatedRoute);
  private youthService = inject(YouthService);
  protected photoService = inject(PhotoService);
  #location = inject(Location);

  constructor() {
    const queryParamMap = toSignal(this.route.queryParamMap, { initialValue: this.route.snapshot.queryParamMap });
    effect(() => this.youthService.detailId.set(queryParamMap().get('id')));
  }

  /** Youth profile for the active route id (milestones + photos embedded) */
  protected data = this.youthService.detail;

  /** Lazy-loaded contacts tab */
  protected contactsData = this.youthService.contacts;

  /** Lazy-loaded class history tab */
  protected historyData = this.youthService.history;

  protected onTabChange(index: number): void {
    if (index === 3) this.youthService.contactsActive.set(true);
    if (index === 4) this.youthService.historyActive.set(true);
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

  navigateBack(): void {
    this.#location.back();
  }
}
