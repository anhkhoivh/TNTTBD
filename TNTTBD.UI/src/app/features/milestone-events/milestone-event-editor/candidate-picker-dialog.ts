import { ChangeDetectionStrategy, Component, computed, effect, inject, signal, untracked } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { FormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';
import { catchError, from, map, of, switchMap, timer } from 'rxjs';
import { TranslatePipe } from '../../../core/i18n/translate.pipe';
import { TranslationService } from '../../../core/i18n/translation.service';
import { ApiService } from '../../../core/services/api.service';
import { MilestoneType } from '../../../shared/enums';
import {
  CatechismClass,
  CatechismYear,
  IsoDate,
  MilestoneEventCandidate,
  PaginatedResult,
} from '../../../shared/models';
import { DateLocalePipe } from '../../../shared/pipes/date-locale.pipe';
import { FullNamePipe } from '../../../shared/pipes/full-name.pipe';
import { MilestoneEventsService } from '../milestone-events.service';
import { formatPrerequisiteWarnings } from '../milestone-event-utils';

export interface CandidatePickerData {
  type: MilestoneType;
  eventId: string | null;
  /** Used to check that prerequisites are dated before the event. */
  eventDate?: IsoDate | null;
  /** Youths already in the editor list. */
  excludeIds: string[];
}

type Eligibility = 'ok' | 'linkExisting' | 'inList' | 'notBaptised' | 'otherEvent';

@Component({
  selector: 'app-candidate-picker-dialog',
  standalone: true,
  imports: [
    FormsModule,
    MatDialogModule,
    MatButtonModule,
    MatCheckboxModule,
    MatChipsModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatPaginatorModule,
    MatProgressBarModule,
    MatSelectModule,
    MatTableModule,
    MatTooltipModule,
    TranslatePipe,
    DateLocalePipe,
    FullNamePipe,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <h2 mat-dialog-title>{{ 'milestoneEvent.addYouths' | t }}</h2>
    <mat-dialog-content>
      <div class="filters">
        <mat-form-field appearance="outline">
          <mat-label>{{ 'class.year' | t }}</mat-label>
          <mat-select [ngModel]="yearId()" (ngModelChange)="onYear($event)">
            <mat-option [value]="null">{{ 'common.all' | t }}</mat-option>
            @for (y of years(); track y.id) {
              <mat-option [value]="y.id">{{ y.title }}</mat-option>
            }
          </mat-select>
        </mat-form-field>
        <mat-form-field appearance="outline">
          <mat-label>{{ 'milestoneEvent.class' | t }}</mat-label>
          <mat-select [ngModel]="classId()" (ngModelChange)="classId.set($event); page.set(1)">
            <mat-option [value]="null">{{ 'common.all' | t }}</mat-option>
            @for (c of classes(); track c.id) {
              <mat-option [value]="c.id">{{ c.className }}</mat-option>
            }
          </mat-select>
        </mat-form-field>
        <mat-form-field appearance="outline" class="search">
          <mat-icon matPrefix>search</mat-icon>
          <mat-label>{{ 'common.search' | t }}</mat-label>
          <input matInput [ngModel]="search()" (ngModelChange)="search.set($event); page.set(1)" />
        </mat-form-field>
      </div>

      @if (candidatesRes.isLoading()) {
        <mat-progress-bar mode="indeterminate"></mat-progress-bar>
      }

      <div class="table-responsive">
        <table mat-table [dataSource]="result().items" class="full-width">
          <ng-container matColumnDef="select">
            <th mat-header-cell *matHeaderCellDef>
              <mat-checkbox
                [checked]="allPageSelected()"
                [indeterminate]="somePageSelected() && !allPageSelected()"
                [disabled]="selectablePage().length === 0"
                (change)="togglePage($event.checked)"
                [attr.aria-label]="'milestoneEvent.selectPage' | t"></mat-checkbox>
            </th>
            <td mat-cell *matCellDef="let row">
              <mat-checkbox
                [checked]="selected().has(row.eucharisticYouthId)"
                [disabled]="!isSelectable(row)"
                (change)="toggle(row, $event.checked)"></mat-checkbox>
            </td>
          </ng-container>
          <ng-container matColumnDef="name">
            <th mat-header-cell *matHeaderCellDef>{{ 'youth.fullName' | t }}</th>
            <td mat-cell *matCellDef="let row">
              <div>{{ row.fullName | fullName }}</div>
              <div class="sub">{{ row.youthCode }}@if (row.dateOfBirth) { · {{ row.dateOfBirth | dateLocale }} }</div>
            </td>
          </ng-container>
          <ng-container matColumnDef="class">
            <th mat-header-cell *matHeaderCellDef>{{ 'milestoneEvent.class' | t }}</th>
            <td mat-cell *matCellDef="let row">{{ row.className }}</td>
          </ng-container>
          <ng-container matColumnDef="status">
            <th mat-header-cell *matHeaderCellDef>{{ 'milestoneEvent.status' | t }}</th>
            <td mat-cell *matCellDef="let row">
              @switch (eligibility(row)) {
                @case ('inList') { <span class="badge muted">{{ 'milestoneEvent.status.inList' | t }}</span> }
                @case ('notBaptised') { <span class="badge warn">{{ 'milestoneEvent.status.notBaptised' | t }}</span> }
                @case ('otherEvent') {
                  <span class="badge warn">{{ 'milestoneEvent.status.otherEvent' | t }}: {{ row.existingMilestone?.milestoneEventName }}</span>
                }
                @case ('linkExisting') {
                  <span class="badge info" [matTooltip]="'milestoneEvent.status.linkExistingHint' | t">
                    {{ 'milestoneEvent.status.linkExisting' | t }}:
                    {{ row.existingMilestone?.date | dateLocale }}@if (row.existingMilestone?.documentCode) { · {{ row.existingMilestone?.documentCode }} }
                  </span>
                }
                @default {
                  @if (row.baptismDate) {
                    <span class="sub">{{ 'milestone.baptism' | t }}: {{ row.baptismDate | dateLocale }}</span>
                  }
                }
              }
              @if (isSelectable(row) && row.warnings?.length) {
                <span
                  class="badge caution"
                  [matTooltip]="warningText(row)"
                  matTooltipClass="multiline-tooltip"
                >
                  <mat-icon inline>warning</mat-icon> {{ 'milestone.warning.title' | t }}
                </span>
              }
            </td>
          </ng-container>
          <tr mat-header-row *matHeaderRowDef="cols"></tr>
          <tr mat-row *matRowDef="let row; columns: cols" [class.disabled]="!isSelectable(row)"></tr>
        </table>
      </div>
      @if (!candidatesRes.isLoading() && result().items.length === 0) {
        <p class="empty">{{ 'common.noData' | t }}</p>
      }
      <mat-paginator
        [length]="result().total"
        [pageSize]="pageSize()"
        [pageIndex]="page() - 1"
        [pageSizeOptions]="[25, 50, 100]"
        (page)="onPage($event)"></mat-paginator>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <span class="count">{{ 'milestoneEvent.selectedCount' | t }}: {{ selected().size }}</span>
      <button mat-button (click)="ref.close()">{{ 'common.cancel' | t }}</button>
      <button mat-flat-button color="primary" [disabled]="selected().size === 0" (click)="confirm()">
        {{ 'common.add' | t }}
      </button>
    </mat-dialog-actions>
  `,
  styles: [
    `
      .filters { display: flex; gap: 12px; flex-wrap: wrap; }
      .filters mat-form-field { min-width: 180px; }
      .filters .search { flex: 1; }
      table.full-width { width: 100%; }
      .sub { font: var(--mat-sys-body-small); color: var(--mat-sys-on-surface-variant); }
      tr.disabled td { opacity: 0.6; }
      .badge { font: var(--mat-sys-label-medium); padding: 2px 8px; border-radius: 12px; white-space: nowrap; }
      .badge.muted { background: var(--mat-sys-surface-variant); }
      .badge.warn { background: var(--mat-sys-error-container); color: var(--mat-sys-on-error-container); }
      .badge.caution { background: var(--mat-sys-secondary-container); color: var(--mat-sys-on-secondary-container); cursor: help; margin-left: 4px; }
      .badge.info { background: var(--mat-sys-tertiary-container); color: var(--mat-sys-on-tertiary-container); }
      .empty { text-align: center; padding: 16px; }
      .count { margin-right: auto; padding-left: 8px; }
    `,
  ],
})
export class CandidatePickerDialog {
  protected ref = inject<MatDialogRef<CandidatePickerDialog, MilestoneEventCandidate[]>>(MatDialogRef);
  protected data = inject<CandidatePickerData>(MAT_DIALOG_DATA);
  private api = inject(ApiService);
  private service = inject(MilestoneEventsService);
  private translate = inject(TranslationService);

  protected readonly cols = ['select', 'name', 'class', 'status'];
  private readonly excluded = new Set(this.data.excludeIds);

  protected yearId = signal<string | null>(null);
  protected classId = signal<string | null>(null);
  protected search = signal('');
  protected page = signal(1);
  protected pageSize = signal(50);
  protected selected = signal(new Map<string, MilestoneEventCandidate>());

  private yearsRes = rxResource<CatechismYear[], void>({
    stream: () =>
      this.api.list<CatechismYear>('/catechism-years').pipe(
        map((r) => [...r.items].sort((a, b) => b.startDate.localeCompare(a.startDate))),
        catchError(() => of([])),
      ),
  });
  protected years = computed(() => this.yearsRes.value() ?? []);

  private classesRes = rxResource<CatechismClass[], string | null>({
    params: () => this.yearId(),
    stream: ({ params }) =>
      this.api
        .list<CatechismClass>('/classes', { yearId: params ?? undefined })
        .pipe(map((r) => r.items), catchError(() => of([]))),
  });
  protected classes = computed(() => this.classesRes.value() ?? []);

  protected candidatesRes = rxResource<PaginatedResult<MilestoneEventCandidate>, {
    search: string; classId: string | null; yearId: string | null; page: number; pageSize: number;
  }>({
    params: () => ({
      search: this.search().trim(),
      classId: this.classId(),
      yearId: this.yearId(),
      page: this.page(),
      pageSize: this.pageSize(),
    }),
    // Debounce typing in the search box
    stream: ({ params }) =>
      timer(params.search ? 300 : 0).pipe(
        switchMap(() =>
          from(
            this.service.candidates({
              type: this.data.type,
              search: params.search,
              classId: params.classId,
              catechismYearId: params.yearId,
              date: this.data.eventDate ?? null,
              page: params.page,
              pageSize: params.pageSize,
            }),
          ),
        ),
        catchError(() => of({ items: [], total: 0, page: 1, pageSize: params.pageSize })),
      ),
  });
  protected result = computed(
    () => this.candidatesRes.value() ?? { items: [], total: 0, page: 1, pageSize: this.pageSize() },
  );

  protected selectablePage = computed(() => this.result().items.filter((c) => this.isSelectable(c)));
  protected allPageSelected = computed(() => {
    const page = this.selectablePage();
    return page.length > 0 && page.every((c) => this.selected().has(c.eucharisticYouthId));
  });
  protected somePageSelected = computed(() =>
    this.selectablePage().some((c) => this.selected().has(c.eucharisticYouthId)),
  );

  constructor() {
    // Default to the latest year once loaded
    effect(() => {
      const years = this.years();
      if (years.length > 0 && untracked(() => this.yearId()) === null) {
        this.yearId.set(years[0].id);
      }
    });
  }

  protected onYear(id: string | null): void {
    this.yearId.set(id);
    this.classId.set(null);
    this.page.set(1);
  }

  protected onPage(e: PageEvent): void {
    this.page.set(e.pageIndex + 1);
    this.pageSize.set(e.pageSize);
  }

  protected eligibility(c: MilestoneEventCandidate): Eligibility {
    if (this.excluded.has(c.eucharisticYouthId)) return 'inList';
    const existing = c.existingMilestone;
    if (existing?.milestoneEventId && existing.milestoneEventId !== this.data.eventId) return 'otherEvent';
    if (this.data.type !== MilestoneType.Baptism && !c.hasBaptism) return 'notBaptised';
    if (existing && !existing.milestoneEventId) return 'linkExisting';
    return 'ok';
  }

  protected warningText(c: MilestoneEventCandidate): string {
    return formatPrerequisiteWarnings(c.warnings, (k) => this.translate.t(k));
  }

  protected isSelectable(c: MilestoneEventCandidate): boolean {
    const e = this.eligibility(c);
    return e === 'ok' || e === 'linkExisting';
  }

  protected toggle(c: MilestoneEventCandidate, checked: boolean): void {
    this.selected.update((m) => {
      const next = new Map(m);
      if (checked) next.set(c.eucharisticYouthId, c);
      else next.delete(c.eucharisticYouthId);
      return next;
    });
  }

  protected togglePage(checked: boolean): void {
    for (const c of this.selectablePage()) this.toggle(c, checked);
  }

  protected confirm(): void {
    this.ref.close([...this.selected().values()]);
  }
}
