import { Component, inject, signal , ChangeDetectionStrategy} from '@angular/core';
import { CommonModule, NgOptimizedImage } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { switchMap, of, catchError, map } from 'rxjs';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatListModule } from '@angular/material/list';
import { ApiService } from '../../core/services/api.service';
import { PhotoService } from '../../core/services/photo.service';
import { AuthService } from '../../core/auth/auth.service';
import {
  Catechist,
  CatechismClassCatechist,
  PaginatedResult,
} from '../../shared/models';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../shared/pipes/full-name.pipe';
import { DateLocalePipe } from '../../shared/pipes/date-locale.pipe';
import { CatechismClassCatechistRole, EnrollmentStatus } from '../../shared/enums';

function emptyPage<T>(): PaginatedResult<T> {
  return { items: [], total: 0, page: 1, pageSize: 50 };
}

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

  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CatechistDetail {
  private route = inject(ActivatedRoute);
  private api = inject(ApiService);
  protected photoService = inject(PhotoService);
  private auth = inject(AuthService);

  protected readonly EnrollmentStatus = EnrollmentStatus;

  protected get canEdit(): boolean {
    const user = this.auth.currentUser();
    if (!user) return false;
    if (user.userRole === 'Admin') return true;
    const id = this.route.snapshot.queryParamMap.get('id');
    return user.catechistId === id;
  }

  /** Initial load: catechist profile only */
  protected data = toSignal(
    this.route.queryParamMap.pipe(
      switchMap((qp) => {
        const id = qp.get('id');
        if (!id) return of(null);
        return this.api.get<Catechist>('/catechists/detail', { id }).pipe(catchError(() => of(null)));
      }),
    ),
    { initialValue: null },
  );

  /** Lazy-loaded assignments tab data */
  protected assignmentsData = signal<PaginatedResult<CatechismClassCatechist> | null>(null);
  private assignmentsLoaded = false;

  protected onTabChange(index: number): void {
    if (index === 2 && !this.assignmentsLoaded) {
      this.assignmentsLoaded = true;
      this.loadAssignments();
    }
  }

  private loadAssignments(): void {
    const id = this.route.snapshot.queryParamMap.get('id');
    if (!id) return;
    this.api
      .list<CatechismClassCatechist>('/class-catechist', { catechistId: id })
      .pipe(catchError(() => of(emptyPage<CatechismClassCatechist>())))
      .subscribe((data) => this.assignmentsData.set(data));
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
}
