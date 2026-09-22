import { Component, inject, effect, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule, Location, NgOptimizedImage } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatListModule } from '@angular/material/list';
import { PhotoService } from '../../core/services/photo.service';
import { AuthService } from '../../core/auth/auth.service';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../shared/pipes/full-name.pipe';
import { DateLocalePipe } from '../../shared/pipes/date-locale.pipe';
import { CatechismClassCatechistRole, EnrollmentStatus } from '../../shared/enums';
import { CatechistService } from './catechist.service';

@Component({
  selector: 'app-catechist-detail',
  standalone: true,
  imports: [
    NgOptimizedImage,
    CommonModule,
    RouterLink,
    MatCardModule,
    MatTabsModule,
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
      .photo-grid {
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(140px, 1fr));
        gap: 1rem;
      }
      .photo {
        width: 100%;
        aspect-ratio: 1;
        object-fit: cover;
        border-radius: 8px;
      }
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
  private catechistService = inject(CatechistService);
  protected photoService = inject(PhotoService);
  private auth = inject(AuthService);
  #location = inject(Location);

  protected readonly EnrollmentStatus = EnrollmentStatus;

  protected get canEdit(): boolean {
    const user = this.auth.currentUser();
    if (!user) return false;
    if (user.userRole === 'Admin') return true;
    const id = this.route.snapshot.queryParamMap.get('id');
    return user.catechistId === id;
  }

  constructor() {
    const queryParamMap = toSignal(this.route.queryParamMap, { initialValue: this.route.snapshot.queryParamMap });
    effect(() => this.catechistService.detailId.set(queryParamMap().get('id')));
  }

  /** Catechist profile for the active route id */
  protected data = this.catechistService.detail;

  /** Lazy-loaded assignments tab data */
  protected assignmentsData = this.catechistService.assignments;

  protected onTabChange(index: number): void {
    if (index === 2) this.catechistService.assignmentsActive.set(true);
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
