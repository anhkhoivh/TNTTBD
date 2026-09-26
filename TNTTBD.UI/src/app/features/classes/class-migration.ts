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
import { NotificationService } from '../../core/services/notification.service';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../shared/pipes/full-name.pipe';
import { TranslationService } from '../../core/i18n/translation.service';
import { ClassesService } from './classes.service';

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
  providers: [ClassesService],
})
export class ClassMigration {
  private classesService = inject(ClassesService);
  private notify = inject(NotificationService);
  private translate = inject(TranslationService);

  protected years = this.classesService.years;
  protected allClasses = this.classesService.classes;

  protected srcYearId = signal<string | null>(null);
  protected srcClassId = signal<string | null>(null);
  protected dstYearId = signal<string | null>(null);
  protected dstClassId = signal<string | null>(null);

  protected srcYouth = this.classesService.srcYouth;
  protected dstYouth = this.classesService.dstYouth;
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

  protected onSrcClassChange(): void {
    this.selected.set(new Set());
    this.classesService.srcClassId.set(this.srcClassId());
  }

  protected onDstClassChange(): void {
    this.classesService.dstClassId.set(this.dstClassId());
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

  protected async migrate(): Promise<void> {
    const src = this.srcClassId();
    const dst = this.dstClassId();
    const ids = [...this.selected()];
    if (!src || !dst || ids.length === 0) return;

    this.migrating.set(true);
    try {
      await this.classesService.migrateYouth({
        sourceClassId: src,
        destinationClassId: dst,
        eucharisticYouthIds: ids,
      });
      this.notify.success(this.translate.t(`class.migrateSuccess`));
      this.selected.set(new Set());
    } catch {
      this.notify.error('error.generic');
    } finally {
      this.migrating.set(false);
    }
  }
}
