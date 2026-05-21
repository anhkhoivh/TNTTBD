import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { switchMap, of, forkJoin } from 'rxjs';
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
  Contact,
  EucharisticYouth,
  EucharisticYouthContact,
} from '../../shared/models';
import { ContactRelationType, EnrollmentStatus, MilestoneType } from '../../shared/enums';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../shared/pipes/full-name.pipe';
import { DateLocalePipe } from '../../shared/pipes/date-locale.pipe';

@Component({
  selector: 'app-youth-detail',
  standalone: true,
  imports: [
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
})
export class YouthDetail {
  private route = inject(ActivatedRoute);
  private api = inject(ApiService);
  protected photoService = inject(PhotoService);

  protected data = toSignal(
    this.route.queryParamMap.pipe(
      switchMap((qp) => {
        const id = qp.get('id');
        if (!id) return of(null);
        return forkJoin({
          youth: this.api.get<EucharisticYouth>('/youths/detail', { id }),
          enrollments: this.api.list<CatechismClassEucharisticYouth>(
            '/youths/enrollments',
            { youthId: id },
          ),
          youthContacts: this.api.list<EucharisticYouthContact>('/youths/contacts', {
            youthId: id,
          }),
          contacts: this.api.list<Contact>('/contacts'),
          classes: this.api.list<CatechismClass>('/classes'),
        });
      }),
    ),
    { initialValue: null },
  );

  protected contactOf(id: string, list: Contact[]): Contact | undefined {
    return list.find((c) => c.id === id);
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
    return `contact.relation.${ContactRelationType[r]}`;
  }

  protected enrollmentStatusKey(s: EnrollmentStatus): string {
    return `enrollment.status.${EnrollmentStatus[s]}`;
  }
}
