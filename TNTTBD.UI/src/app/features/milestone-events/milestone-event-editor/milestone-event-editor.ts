import {
  ChangeDetectionStrategy,
  Component,
  HostListener,
  Injector,
  computed,
  effect,
  inject,
  signal,
} from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSelectModule } from '@angular/material/select';
import { MatTooltipModule } from '@angular/material/tooltip';
import { startWith } from 'rxjs';
import { TranslatePipe } from '../../../core/i18n/translate.pipe';
import { TranslationService } from '../../../core/i18n/translation.service';
import { DialogService } from '../../../core/services/dialog.service';
import { NotificationService } from '../../../core/services/notification.service';
import { ConfirmData, ConfirmDialog } from '../../../shared/components/confirm-dialog';
import {
  GodparentPickerData,
  GodparentPickerDialog,
} from '../../../shared/components/godparent-picker-dialog/godparent-picker-dialog';
import { AuthService } from '../../../core/auth/auth.service';
import { MILESTONE_TYPE_KEYS, MilestoneType, UserRole, milestoneSupportsSponsor } from '../../../shared/enums';
import {
  FullName,
  IsoDate,
  MilestoneEventCandidate,
  MilestoneEventDetail,
  MilestonePrerequisiteWarning,
  MilestoneSponsor,
  SaveMilestoneEventPayload,
} from '../../../shared/models';
import { DateLocalePipe } from '../../../shared/pipes/date-locale.pipe';
import { FullNamePipe } from '../../../shared/pipes/full-name.pipe';
import { MilestoneEventsService } from '../milestone-events.service';
import {
  downloadBlob,
  formatPrerequisiteWarnings,
  fromIsoDate,
  parseCodeTemplate,
  toIsoDate,
} from '../milestone-event-utils';
import { CandidatePickerData, CandidatePickerDialog } from './candidate-picker-dialog';

export interface ParticipantRow {
  eucharisticYouthId: string;
  milestoneId: string | null;
  youthCode: string;
  fullName: FullName;
  dateOfBirth?: IsoDate | null;
  className?: string | null;
  baptismDate?: IsoDate | null;
  /** Per-row override; null = event date. */
  date: Date | null;
  /** Per-row override; empty = event place. */
  place: string;
  documentCode: string;
  /** Per-row override; empty = event celebrant. */
  celebrant: string;
  sponsor: MilestoneSponsor | null;
  note: string;
  /** From the server; refreshed on load/save. */
  warnings: MilestonePrerequisiteWarning[];
  /** A standalone record linked in this session (not saved yet). */
  linkingExisting: boolean;
}

// Vietnamese convention: given name first, then family and middle name (matches the server order)
function nameKey(n: FullName): string {
  return [n.givenName, n.familyName, n.middleName, n.christName].map((p) => p ?? '').join(' ');
}

function sortByName(rows: ParticipantRow[]): ParticipantRow[] {
  return [...rows].sort((a, b) => nameKey(a.fullName).localeCompare(nameKey(b.fullName), 'vi'));
}

@Component({
  selector: 'app-milestone-event-editor',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    RouterLink,
    MatButtonModule,
    MatCardModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatProgressBarModule,
    MatSelectModule,
    MatTooltipModule,
    TranslatePipe,
    DateLocalePipe,
    FullNamePipe,
  ],
  templateUrl: './milestone-event-editor.html',
  styleUrl: './milestone-event-editor.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [MilestoneEventsService],
})
export class MilestoneEventEditor {
  private service = inject(MilestoneEventsService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private dialogs = inject(DialogService);
  private notify = inject(NotificationService);
  private translate = inject(TranslationService);
  private injector = inject(Injector);
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private fullNamePipe = new FullNamePipe();

  protected readonly isAdmin = this.auth.hasRole([UserRole.Admin]);

  protected readonly typeKeys = MILESTONE_TYPE_KEYS;
  protected readonly types = Object.values(MilestoneType);

  protected form = this.fb.group({
    name: this.fb.nonNullable.control('', [Validators.required, Validators.maxLength(200)]),
    type: this.fb.nonNullable.control<MilestoneType>(MilestoneType.FirstCommunion, Validators.required),
    date: this.fb.control<Date | null>(null, Validators.required),
    place: this.fb.nonNullable.control('', Validators.maxLength(200)),
    celebrant: this.fb.nonNullable.control('', Validators.maxLength(200)),
    note: this.fb.nonNullable.control('', Validators.maxLength(1000)),
    codeTemplate: this.fb.nonNullable.control('', Validators.maxLength(100)),
    startIndex: this.fb.nonNullable.control(1, [Validators.required, Validators.min(0)]),
  });

  private formValue = toSignal(this.form.valueChanges.pipe(startWith(this.form.getRawValue())), {
    initialValue: this.form.getRawValue(),
  });

  protected id = signal<string | null>(null);
  protected savedParticipantCount = signal(0);
  protected rows = signal<ParticipantRow[]>([]);
  protected dirty = signal(false);
  protected loading = signal(false);
  protected saving = signal(false);
  protected exporting = signal(false);

  protected eventType = computed(() => {
    this.formValue();
    return this.form.getRawValue().type;
  });
  protected supportsSponsor = computed(() => milestoneSupportsSponsor(this.eventType()));
  protected eventDateIso = computed(() => {
    this.formValue();
    return toIsoDate(this.form.getRawValue().date);
  });
  protected eventPlace = computed(() => {
    this.formValue();
    return this.form.getRawValue().place;
  });
  protected eventCelebrant = computed(() => {
    this.formValue();
    return this.form.getRawValue().celebrant;
  });

  protected template = computed(() => {
    this.formValue();
    const raw = this.form.getRawValue().codeTemplate.trim();
    return { raw, parsed: raw ? parseCodeTemplate(raw) : null };
  });

  protected preview = computed(() => {
    const { parsed } = this.template();
    const v = this.form.getRawValue();
    this.formValue();
    if (!parsed) return '';
    const start = Number(v.startIndex) || 0;
    const fallback = this.eventDateIso() ?? toIsoDate(new Date())!;
    const rows = this.rows();
    const dateOf = (i: number) => toIsoDate(rows[i]?.date ?? null) ?? fallback;
    if (rows.length <= 1) return parsed.generate(start, dateOf(0));
    return `${parsed.generate(start, dateOf(0))} … ${parsed.generate(start + rows.length - 1, dateOf(rows.length - 1))}`;
  });

  protected canDelete = computed(
    () => this.isAdmin && !!this.id() && this.savedParticipantCount() === 0 && this.rows().length === 0,
  );

  constructor() {
    this.form.valueChanges.subscribe(() => {
      if (!this.loading()) this.dirty.set(true);
    });

    // The type is fixed once youths are in the list (eligibility and existing records depend on it)
    effect(() => {
      const control = this.form.controls.type;
      if (this.rows().length > 0) control.disable({ emitEvent: false });
      else control.enable({ emitEvent: false });
    });

    const id = this.route.snapshot.queryParamMap.get('id');
    if (id) void this.load(id);
  }

  @HostListener('window:beforeunload', ['$event'])
  protected onBeforeUnload(event: BeforeUnloadEvent): void {
    if (this.dirty()) event.preventDefault();
  }

  /** Used by the route's canDeactivate guard. */
  async canLeave(): Promise<boolean> {
    if (!this.dirty()) return true;
    return this.dialogs.confirm({
      message: this.translate.t('milestoneEvent.unsavedLeave'),
      confirmKey: 'milestoneEvent.leave',
      destructive: true,
    });
  }

  private async load(id: string): Promise<void> {
    this.loading.set(true);
    try {
      this.apply(await this.service.get(id));
    } catch {
      // error toast already shown by global http error handling
    } finally {
      this.loading.set(false);
    }
  }

  private apply(d: MilestoneEventDetail): void {
    const wasLoading = this.loading();
    this.loading.set(true);
    this.id.set(d.id);
    this.savedParticipantCount.set(d.participants.length);
    this.form.controls.type.enable({ emitEvent: false });
    this.form.setValue({
      name: d.name,
      type: d.type,
      date: fromIsoDate(d.date),
      place: d.place ?? '',
      celebrant: d.celebrant ?? '',
      note: d.note ?? '',
      codeTemplate: d.codeTemplate ?? '',
      startIndex: d.startIndex,
    });
    this.rows.set(
      sortByName(
        d.participants.map((p) => ({
          eucharisticYouthId: p.eucharisticYouthId,
          milestoneId: p.milestoneId,
          youthCode: p.youthCode,
          fullName: p.fullName,
          dateOfBirth: p.dateOfBirth,
          className: p.className,
          baptismDate: p.baptismDate,
          date: p.date && p.date !== d.date ? fromIsoDate(p.date) : null,
          place: p.place && p.place !== (d.place ?? '') ? p.place : '',
          documentCode: p.documentCode ?? '',
          celebrant: p.celebrant && p.celebrant !== (d.celebrant ?? '') ? p.celebrant : '',
          sponsor: p.sponsor ?? null,
          note: p.note ?? '',
          warnings: p.warnings ?? [],
          linkingExisting: false,
        })),
      ),
    );
    this.dirty.set(false);
    this.loading.set(wasLoading);
  }

  /** Rows are edited in place via ngModel; re-emit so derived values (preview) refresh. */
  protected markDirty(): void {
    this.rows.update((rows) => [...rows]);
    this.dirty.set(true);
  }

  // --- Participants ---

  protected async addYouths(): Promise<void> {
    const type = this.form.getRawValue().type;
    const picked = await this.dialogs.open<CandidatePickerDialog, CandidatePickerData, MilestoneEventCandidate[]>(
      CandidatePickerDialog,
      {
        data: {
          type,
          eventId: this.id(),
          eventDate: this.eventDateIso(),
          excludeIds: this.rows().map((r) => r.eucharisticYouthId),
        },
        injector: this.injector,
        width: '960px',
        maxWidth: '96vw',
      },
    );
    if (!picked?.length) return;

    let selected = picked;
    const standalone = picked.filter((c) => c.existingMilestone && !c.existingMilestone.milestoneEventId);
    if (standalone.length > 0) {
      const choice = await this.dialogs.open<ConfirmDialog, ConfirmData, boolean>(ConfirmDialog, {
        data: {
          title: this.translate.t('milestoneEvent.linkExistingTitle'),
          message: this.translate
            .t('milestoneEvent.linkExistingConfirm')
            .replace('{count}', String(standalone.length))
            .replace('{names}', standalone.map((c) => this.fullNamePipe.transform(c.fullName)).join(', ')),
          confirmKey: 'milestoneEvent.linkExisting',
          cancelKey: 'milestoneEvent.skipExisting',
        },
      });
      if (choice === undefined) return;
      if (choice === false) selected = picked.filter((c) => !standalone.includes(c));
    }

    const eventDate = this.eventDateIso();
    const eventPlace = this.eventPlace();
    const eventCelebrant = this.eventCelebrant();
    this.rows.update((rows) =>
      sortByName([
        ...rows,
        ...selected.map((c): ParticipantRow => {
          const m = c.existingMilestone;
          return {
            eucharisticYouthId: c.eucharisticYouthId,
            milestoneId: m?.milestoneId ?? null,
            youthCode: c.youthCode,
            fullName: c.fullName,
            dateOfBirth: c.dateOfBirth,
            className: c.className,
            baptismDate: c.baptismDate,
            date: m?.date && m.date !== eventDate ? fromIsoDate(m.date) : null,
            place: m?.place && m.place !== eventPlace ? m.place : '',
            documentCode: m?.documentCode ?? '',
            celebrant: m?.celebrant && m.celebrant !== eventCelebrant ? m.celebrant : '',
            sponsor: m?.sponsor ?? null,
            note: m?.note ?? '',
            warnings: c.warnings ?? [],
            linkingExisting: !!m && !m.milestoneEventId,
          };
        }),
      ]),
    );
    this.dirty.set(true);
  }

  protected warningText(row: ParticipantRow): string {
    return formatPrerequisiteWarnings(row.warnings, (k) => this.translate.t(k));
  }

  protected async pickSponsor(row: ParticipantRow): Promise<void> {
    const sponsor = await this.dialogs.open<GodparentPickerDialog, GodparentPickerData, MilestoneSponsor>(
      GodparentPickerDialog,
      { data: { current: row.sponsor }, injector: this.injector },
    );
    if (!sponsor) return;
    row.sponsor = sponsor;
    this.markDirty();
  }

  protected clearSponsor(row: ParticipantRow): void {
    row.sponsor = null;
    this.markDirty();
  }

  protected removeRow(index: number): void {
    this.rows.update((rows) => rows.filter((_, i) => i !== index));
    this.dirty.set(true);
  }

  // --- Codes ---

  protected async generateCodes(): Promise<void> {
    const { raw, parsed } = this.template();
    const rows = this.rows();
    if (!parsed || rows.length === 0) return;

    let fillBlanksOnly = false;
    if (rows.some((r) => r.documentCode.trim())) {
      const choice = await this.dialogs.open<ConfirmDialog, ConfirmData, boolean>(ConfirmDialog, {
        data: {
          message: this.translate.t('milestoneEvent.overwriteCodes'),
          confirmKey: 'milestoneEvent.overwriteAll',
          cancelKey: 'milestoneEvent.fillBlanksOnly',
        },
      });
      if (choice === undefined) return;
      fillBlanksOnly = choice === false;
    }

    const fallback = this.eventDateIso() ?? toIsoDate(new Date())!;
    try {
      const codes = await this.service.generateCodes(
        raw,
        Number(this.form.getRawValue().startIndex) || 0,
        rows.map((r) => toIsoDate(r.date) ?? fallback),
      );
      this.rows.update((current) =>
        current.map((r, i) =>
          fillBlanksOnly && r.documentCode.trim() ? r : { ...r, documentCode: codes[i] ?? r.documentCode },
        ),
      );
      this.dirty.set(true);
    } catch {
      // error toast already shown by global http error handling
    }
  }

  // --- Save / export / delete ---

  protected async save(): Promise<void> {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    if (this.saving()) return;
    this.saving.set(true);
    try {
      const v = this.form.getRawValue();
      const supportsSponsor = milestoneSupportsSponsor(v.type);
      const payload: SaveMilestoneEventPayload = {
        id: this.id(),
        name: v.name.trim(),
        type: v.type,
        date: toIsoDate(v.date)!,
        place: v.place.trim() || null,
        celebrant: v.celebrant.trim() || null,
        note: v.note.trim() || null,
        codeTemplate: v.codeTemplate.trim() || null,
        startIndex: Number(v.startIndex) || 0,
        participants: this.rows().map((r) => ({
          eucharisticYouthId: r.eucharisticYouthId,
          milestoneId: r.milestoneId,
          date: toIsoDate(r.date),
          place: r.place.trim() || null,
          documentCode: r.documentCode.trim() || null,
          celebrant: r.celebrant.trim() || null,
          sponsorContactId: supportsSponsor ? (r.sponsor?.contactId ?? null) : null,
          note: r.note.trim() || null,
        })),
      };
      const id = await this.service.save(payload);
      const detail = await this.service.get(id);
      this.apply(detail);
      this.notify.success(this.translate.t('common.saved'));
      if (!payload.id) {
        await this.router.navigate([], { relativeTo: this.route, queryParams: { id }, replaceUrl: true });
      }
    } catch {
      // error toast already shown by global http error handling
    } finally {
      this.saving.set(false);
    }
  }

  protected async exportExcel(): Promise<void> {
    const id = this.id();
    if (!id || this.dirty() || this.exporting()) return;
    this.exporting.set(true);
    try {
      const blob = await this.service.exportExcel(id, this.translate.currentLang());
      downloadBlob(blob, `${this.form.getRawValue().name || 'milestone-event'}.xlsx`);
    } catch {
      // error toast already shown by global http error handling
    } finally {
      this.exporting.set(false);
    }
  }

  protected async deleteEvent(): Promise<void> {
    const id = this.id();
    if (!id || !this.canDelete()) return;
    const ok = await this.dialogs.confirmDelete(
      this.translate.t('milestoneEvent.deleteConfirm').replace('{name}', this.form.getRawValue().name),
    );
    if (!ok) return;
    try {
      await this.service.remove(id);
      this.dirty.set(false);
      this.notify.success(this.translate.t('common.deleted'));
      await this.router.navigate(['/app/milestone-events']);
    } catch {
      // error toast already shown by global http error handling
    }
  }
}
