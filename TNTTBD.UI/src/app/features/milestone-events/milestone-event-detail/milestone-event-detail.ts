import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { AuthService } from '../../../core/auth/auth.service';
import { TranslatePipe } from '../../../core/i18n/translate.pipe';
import { TranslationService } from '../../../core/i18n/translation.service';
import { MILESTONE_TYPE_KEYS, UserRole, milestoneSupportsSponsor } from '../../../shared/enums';
import { MilestoneEventDetail, MilestoneEventParticipant } from '../../../shared/models';
import { DateLocalePipe } from '../../../shared/pipes/date-locale.pipe';
import { FullNamePipe } from '../../../shared/pipes/full-name.pipe';
import { MilestoneEventsService } from '../milestone-events.service';
import { downloadBlob, formatPrerequisiteWarnings } from '../milestone-event-utils';

/**
 * Read-only view of a sacrament event. Everyone signed in can open it; the server strips
 * personal details (birth date, codes, godparent, notes) for parents and youths.
 */
@Component({
  selector: 'app-milestone-event-detail',
  standalone: true,
  imports: [
    RouterLink,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatProgressBarModule,
    MatTooltipModule,
    TranslatePipe,
    DateLocalePipe,
    FullNamePipe,
  ],
  templateUrl: './milestone-event-detail.html',
  styleUrl: './milestone-event-detail.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [MilestoneEventsService],
})
export class MilestoneEventDetailPage {
  private service = inject(MilestoneEventsService);
  private route = inject(ActivatedRoute);
  private auth = inject(AuthService);
  private translate = inject(TranslationService);

  protected readonly typeKeys = MILESTONE_TYPE_KEYS;
  protected readonly isStaff = this.auth.hasRole([UserRole.Admin, UserRole.Catechist]);

  protected detail = signal<MilestoneEventDetail | null>(null);
  protected loading = signal(false);
  protected notFound = signal(false);
  protected exporting = signal(false);

  protected supportsSponsor = computed(() => {
    const d = this.detail();
    return !!d && milestoneSupportsSponsor(d.type);
  });
  protected warningCount = computed(() => this.detail()?.participants.filter((p) => p.warnings.length).length ?? 0);

  constructor() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) void this.load(id);
    else this.notFound.set(true);
  }

  private async load(id: string): Promise<void> {
    this.loading.set(true);
    try {
      this.detail.set(await this.service.get(id));
    } catch {
      this.notFound.set(true);
    } finally {
      this.loading.set(false);
    }
  }

  protected warningText(p: MilestoneEventParticipant): string {
    return formatPrerequisiteWarnings(p.warnings, (k) => this.translate.t(k));
  }

  /** The participant's own value, or null when it is the same as the event's. */
  protected own(value: string | null | undefined, eventValue: string | null | undefined): string | null {
    return value && value !== eventValue ? value : null;
  }

  protected async exportExcel(): Promise<void> {
    const d = this.detail();
    if (!d || this.exporting()) return;
    this.exporting.set(true);
    try {
      const blob = await this.service.exportExcel(d.id, this.translate.currentLang());
      downloadBlob(blob, `${d.name || 'milestone-event'}.xlsx`);
    } catch {
      // error toast already shown by global http error handling
    } finally {
      this.exporting.set(false);
    }
  }
}
