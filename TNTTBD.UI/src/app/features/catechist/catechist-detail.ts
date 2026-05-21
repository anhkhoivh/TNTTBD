import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { switchMap, of, forkJoin } from 'rxjs';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatListModule } from '@angular/material/list';
import { ApiService } from '../../core/services/api.service';
import { PhotoService } from '../../core/services/photo.service';
import { Catechist, CatechismClass, CatechismClassCatechist } from '../../shared/models';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../shared/pipes/full-name.pipe';
import { DateLocalePipe } from '../../shared/pipes/date-locale.pipe';
import { CatechismClassCatechistRole } from '../../shared/enums';

@Component({
  selector: 'app-catechist-detail',
  standalone: true,
  imports: [
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
})
export class CatechistDetail {
  private route = inject(ActivatedRoute);
  private api = inject(ApiService);
  protected photoService = inject(PhotoService);

  protected data = toSignal(
    this.route.queryParamMap.pipe(
      switchMap((qp) => {
        const id = qp.get('id');
        if (!id) return of(null);
        return forkJoin({
          catechist: this.api.get<Catechist>('/catechists/detail', { id }),
          assignments: this.api.list<CatechismClassCatechist>('/catechists/assignments', {
            catechistId: id,
          }),
          classes: this.api.list<CatechismClass>('/classes'),
        });
      }),
    ),
    { initialValue: null },
  );

  protected classOf(id: string, list: CatechismClass[]): CatechismClass | undefined {
    return list.find((c) => c.id === id);
  }

  protected roleKey(role: CatechismClassCatechistRole): string {
    const map: Record<CatechismClassCatechistRole, string> = {
      [CatechismClassCatechistRole.Main]: 'catechistRole.Main',
      [CatechismClassCatechistRole.Member]: 'catechistRole.Member',
      [CatechismClassCatechistRole.Assistance]: 'catechistRole.Assistance',
    };
    return map[role];
  }
}
