import { Component, inject, effect, signal, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatListModule } from '@angular/material/list';
import { PhotoGallery } from '../../../shared/components/photo-gallery/photo-gallery';
import { MemberAvatar } from '../../../shared/components/member-avatar/member-avatar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { AuthService } from '../../../core/auth/auth.service';
import { TranslatePipe } from '../../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../../shared/pipes/full-name.pipe';
import { DateLocalePipe } from '../../../shared/pipes/date-locale.pipe';
import { CatechismClassCatechistRole, EnrollmentStatus } from '../../../shared/enums';
import { CatechistService } from '../catechist.service';
import { tabIndexFromKey, tabKeyForSelection } from '../../../shared/utils/tab-query-state';
import { ScrollableTabsDirective } from '../../../shared/directives/scrollable-tabs.directive';

@Component({
  selector: 'app-catechist-detail',
  standalone: true,
  imports: [
    PhotoGallery,
    MemberAvatar,
    MatTooltipModule,
    CommonModule,
    RouterLink,
    MatCardModule,
    MatTabsModule,
    ScrollableTabsDirective,
    MatIconModule,
    MatButtonModule,
    MatChipsModule,
    MatListModule,
    TranslatePipe,
    FullNamePipe,
    DateLocalePipe,
  ],
  templateUrl: './catechist-detail.html',
  styles: [
    `
      .field {
        display: flex;
        align-items: center;
        gap: 0.5rem;
        margin: 0.25rem 0;
      }
    `,
  ],

  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [CatechistService],
})
export class CatechistDetail {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private catechistService = inject(CatechistService);
  private auth = inject(AuthService);
  #location = inject(Location);

  protected readonly EnrollmentStatus = EnrollmentStatus;
  protected selectedTabIndex = signal(0);
  private readonly tabKeys = ['profile', 'photos', 'class-history'];

  protected get canEdit(): boolean {
    const user = this.auth.currentUser();
    if (!user) return false;
    if (user.userRole === 'Admin') return true;
    const id = this.route.snapshot.queryParamMap.get('id');
    return user.catechistId === id;
  }

  constructor() {
    const queryParamMap = toSignal(this.route.queryParamMap, { initialValue: this.route.snapshot.queryParamMap });
    effect(() => {
      const params = queryParamMap();
      this.catechistService.detailId.set(params.get('id'));
      const index = tabIndexFromKey(this.tabKeys, params.get('tab'));
      this.selectedTabIndex.set(index);
      if (index === 2) this.catechistService.assignmentsActive.set(true);
    });
  }

  /** Catechist profile for the active route id */
  protected data = this.catechistService.detail;

  protected reloadPhotos(): void {
    this.catechistService.detailRes.reload();
  }

  protected openPhotos(): void {
    this.onTabChange(1);
  }

  /** Lazy-loaded assignments tab data */
  protected assignmentsData = this.catechistService.assignments;

  protected onTabChange(index: number): void {
    if (index === 2) this.catechistService.assignmentsActive.set(true);
    const currentTab = this.route.snapshot.queryParamMap.get('tab');
    const tab = tabKeyForSelection(this.tabKeys, index, currentTab);
    if (!tab) return;
    void this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { tab },
      queryParamsHandling: 'merge',
    });
  }

  protected roleKey(role: CatechismClassCatechistRole): string {
    const map: Record<CatechismClassCatechistRole, string> = {
      [CatechismClassCatechistRole.Main]: 'catechistRole.Main',
      [CatechismClassCatechistRole.Member]: 'catechistRole.Member',
      [CatechismClassCatechistRole.Trainee]: 'catechistRole.Trainee',
      [CatechismClassCatechistRole.Auxiliary]: 'catechistRole.Auxiliary',
    };
    return map[role];
  }

  protected enrollmentStatusKey(s: EnrollmentStatus): string {
    return `enrollment.status.${s}`;
  }

  navigateBack(): void {
    this.#location.back();
  }
}
