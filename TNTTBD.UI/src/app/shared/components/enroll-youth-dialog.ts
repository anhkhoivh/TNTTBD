import { Component, inject, signal, computed, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { debounceTime, map, of } from 'rxjs';
import { rxResource, toSignal } from '@angular/core/rxjs-interop';
import { ApiService } from '../../core/services/api.service';
import { EucharisticYouth, PaginatedResult } from '../models';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { FullNamePipe } from '../pipes/full-name.pipe';

export interface EnrollYouthData {
  classId: string;
  enrolledYouthIds: Set<string>;
}

@Component({
  selector: 'app-enroll-youth-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatAutocompleteModule,
    MatButtonModule,
    TranslatePipe,
  ],
  template: `
    <h2 mat-dialog-title>{{ 'class.addExistingYouth' | t }}</h2>
    <mat-dialog-content class="form">
      <mat-form-field appearance="outline" class="full-width">
        <mat-label>{{ 'class.searchYouth' | t }}</mat-label>
        <input matInput [formControl]="searchCtrl" [matAutocomplete]="auto" />
        <mat-autocomplete #auto>
          @for (s of suggestions(); track s.id) {
            <mat-option [value]="s.label" (onSelectionChange)="pick(s)">
              {{ s.label }}
            </mat-option>
          }
        </mat-autocomplete>
      </mat-form-field>
      @if (selected()) {
        <p>{{ selected()!.label }}</p>
      }
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button (click)="ref.close()">{{ 'common.cancel' | t }}</button>
      <button mat-flat-button color="primary" (click)="submit()" [disabled]="!selected()">
        {{ 'common.save' | t }}
      </button>
    </mat-dialog-actions>
  `,
  styles: [`.form { min-width: 400px; } .full-width { width: 100%; }`],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EnrollYouthDialog {
  protected ref = inject(MatDialogRef<EnrollYouthDialog>);
  private data = inject<EnrollYouthData>(MAT_DIALOG_DATA);
  private fb = inject(FormBuilder);
  private api = inject(ApiService);
  private fullNamePipe = new FullNamePipe();

  protected searchCtrl = this.fb.control('');
  protected selected = signal<{ id: string; label: string } | null>(null);

  private searchTerm = toSignal(this.searchCtrl.valueChanges.pipe(debounceTime(200)), { initialValue: '' });

  protected suggestionsRes = rxResource<{ id: string; label: string }[], string>({
    params: () => (this.searchTerm() ?? '').trim(),
    stream: ({ params }) => {
      if (params.length < 1) return of([]);
      return this.api.list<EucharisticYouth>('/youths', { search: params }).pipe(
        map((r: PaginatedResult<EucharisticYouth>) =>
          r.items
            .filter((y) => !this.data.enrolledYouthIds.has(y.id))
            .map((y) => ({ id: y.id, label: `${y.code} — ${this.fullNamePipe.transform(y.fullName)}` })),
        ),
      );
    },
  });
  protected suggestions = computed(() => this.suggestionsRes.value() ?? []);

  protected pick(item: { id: string; label: string }): void {
    this.selected.set(item);
  }

  protected submit(): void {
    const s = this.selected();
    if (!s) return;
    this.ref.close({
      catechismClassId: this.data.classId,
      eucharisticYouthId: s.id,
    });
  }
}
