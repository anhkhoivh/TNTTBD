import { Component, inject, signal, computed, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatListModule } from '@angular/material/list';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ApiService } from '../../core/services/api.service';
import { NotificationService } from '../../core/services/notification.service';
import {
  CatechismYear,
  CatechismClass,
  CatechismClassEucharisticYouth,
  PaginatedResult,
} from '../../shared/models';
import { EnrollmentStatus } from '../../shared/enums';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../shared/pipes/full-name.pipe';

@Component({
  selector: 'app-class-migration',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    FormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatSelectModule,
    MatListModule,
    MatCheckboxModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    TranslatePipe,
    FullNamePipe,
  ],
  templateUrl: './class-migration.html',
  styleUrl: './class-migration.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ClassMigration {
  private api = inject(ApiService);
  private notify = inject(NotificationService);

  protected years = signal<CatechismYear[]>([]);
  protected allClasses = signal<CatechismClass[]>([]);

  protected srcYearId = signal<string | null>(null);
  protected srcClassId = signal<string | null>(null);
  protected dstYearId = signal<string | null>(null);
  protected dstClassId = signal<string | null>(null);

  protected srcYouth = signal<CatechismClassEucharisticYouth[]>([]);
  protected dstYouth = signal<CatechismClassEucharisticYouth[]>([]);
  protected selected = signal<Set<string>>(new Set());
  protected migrating = signal(false);

  protected srcClasses = computed(() => {
    const yId = this.srcYearId();
    return yId ? this.allClasses().filter((c) => c.catechismYearId === yId) : [];
  });

  protected dstClasses = computed(() => {
    const yId = this.dstYearId();
    return yId ? this.allClasses().filter((c) => c.catechismYearId === yId) : [];
  });

  protected allSelected = computed(() => {
    const youth = this.srcYouth();
    const sel = this.selected();
    return youth.length > 0 && youth.every((y) => sel.has(y.eucharisticYouthId));
  });

  protected someSelected = computed(() => this.selected().size > 0);

  constructor() {
    this.api.list<CatechismYear>('/catechism-years').subscribe((r) => this.years.set(r.items));
    this.api.list<CatechismClass>('/classes').subscribe((r) => this.allClasses.set(r.items));
  }

  protected onSrcClassChange(): void {
    const classId = this.srcClassId();
    if (!classId) {
      this.srcYouth.set([]);
      return;
    }
    this.selected.set(new Set());
    this.api
      .list<CatechismClassEucharisticYouth>('/class-youth', { classId })
      .subscribe((r) => {
        this.srcYouth.set(r.items.filter((e) => e.status === EnrollmentStatus.Active));
      });
  }

  protected onDstClassChange(): void {
    const classId = this.dstClassId();
    if (!classId) {
      this.dstYouth.set([]);
      return;
    }
    this.api
      .list<CatechismClassEucharisticYouth>('/class-youth', { classId })
      .subscribe((r) => {
        this.dstYouth.set(r.items.filter((e) => e.status === EnrollmentStatus.Active));
      });
  }

  protected toggleAll(checked: boolean): void {
    if (checked) {
      this.selected.set(new Set(this.srcYouth().map((y) => y.eucharisticYouthId)));
    } else {
      this.selected.set(new Set());
    }
  }

  protected toggleOne(youthId: string, checked: boolean): void {
    const s = new Set(this.selected());
    if (checked) s.add(youthId);
    else s.delete(youthId);
    this.selected.set(s);
  }

  protected isSelected(youthId: string): boolean {
    return this.selected().has(youthId);
  }

  protected migrate(): void {
    const src = this.srcClassId();
    const dst = this.dstClassId();
    const ids = [...this.selected()];
    if (!src || !dst || ids.length === 0) return;

    this.migrating.set(true);
    this.api
      .post<{ migrated: number }>('/class-youth/migrate', {
        sourceClassId: src,
        destinationClassId: dst,
        eucharisticYouthIds: ids,
      })
      .subscribe({
        next: (res) => {
          this.notify.success(`class.migrateSuccess`);
          this.migrating.set(false);
          this.selected.set(new Set());
          this.onSrcClassChange();
          this.onDstClassChange();
        },
        error: () => {
          this.notify.error('error.generic');
          this.migrating.set(false);
        },
      });
  }
}
