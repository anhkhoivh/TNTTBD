import { Component, inject, signal, computed, ChangeDetectionStrategy } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { catchError, debounceTime, map, of } from 'rxjs';
import { rxResource, toSignal } from '@angular/core/rxjs-interop';
import { ApiService } from '../../../core/services/api.service';
import { EucharisticYouth, PaginatedResult } from '../../models';
import { ContactRelationType } from '../../enums';
import { CONTACT_RELATION_TYPES } from '../../constants/contact-relation-types';
import { TranslatePipe } from '../../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../pipes/full-name.pipe';

export interface LinkYouthData {
  contactId: string;
  existingYouthIds: Set<string>;
}

export interface LinkYouthResult {
  eucharisticYouthId: string;
  contactId: string;
  relationType: ContactRelationType;
}

interface YouthOption {
  id: string;
  label: string;
}

@Component({
  selector: 'app-link-youth-dialog',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatAutocompleteModule,
    MatButtonModule,
    TranslatePipe,
  ],
  templateUrl: './link-youth-dialog.html',
  styleUrl: './link-youth-dialog.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LinkYouthDialog {
  protected ref = inject<MatDialogRef<LinkYouthDialog, LinkYouthResult>>(MatDialogRef);
  private data = inject<LinkYouthData>(MAT_DIALOG_DATA);
  private fb = inject(FormBuilder);
  private api = inject(ApiService);
  private fullNamePipe = new FullNamePipe();

  protected readonly relationTypes = CONTACT_RELATION_TYPES;

  protected form = this.fb.nonNullable.group({
    youthSearch: [''],
    relationType: [ContactRelationType.Undefined as ContactRelationType, Validators.required],
  });

  protected selected = signal<YouthOption | null>(null);

  private searchTerm = toSignal(this.form.controls.youthSearch.valueChanges.pipe(debounceTime(200)), {
    initialValue: '',
  });

  protected suggestionsRes = rxResource<YouthOption[], string>({
    params: () => (this.searchTerm() ?? '').trim(),
    stream: ({ params }) => {
      if (params.length < 1) return of([]);
      return this.api.list<EucharisticYouth>('/youths', { search: params }).pipe(
        map((r: PaginatedResult<EucharisticYouth>) =>
          r.items
            .filter((y) => !this.data.existingYouthIds.has(y.id))
            .map((y) => ({ id: y.id, label: `${this.fullNamePipe.transform(y.fullName)} — ${y.code}` })),
        ),
        catchError(() => of([])),
      );
    },
  });
  protected suggestions = computed(() => this.suggestionsRes.value() ?? []);

  protected pick(item: YouthOption): void {
    this.selected.set(item);
  }

  protected save(): void {
    const youth = this.selected();
    if (!youth || this.form.invalid) return;

    this.ref.close({
      eucharisticYouthId: youth.id,
      contactId: this.data.contactId,
      relationType: this.form.getRawValue().relationType,
    });
  }
}
