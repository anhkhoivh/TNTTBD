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
import { Contact, PaginatedResult } from '../../models';
import { ContactRelationType } from '../../enums';
import { CONTACT_RELATION_TYPES } from '../../constants/contact-relation-types';
import { TranslatePipe } from '../../../core/i18n/translate.pipe';
import { FullNamePipe } from '../../pipes/full-name.pipe';

export interface LinkContactData {
  youthId: string;
  existingContactIds: Set<string>;
}

export interface LinkContactResult {
  eucharisticYouthId: string;
  contactId: string;
  relationType: ContactRelationType;
}

interface ContactOption {
  id: string;
  label: string;
}

@Component({
  selector: 'app-link-contact-dialog',
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
  templateUrl: './link-contact-dialog.html',
  styleUrl: './link-contact-dialog.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LinkContactDialog {
  protected ref = inject<MatDialogRef<LinkContactDialog, LinkContactResult>>(MatDialogRef);
  private data = inject<LinkContactData>(MAT_DIALOG_DATA);
  private fb = inject(FormBuilder);
  private api = inject(ApiService);
  private fullNamePipe = new FullNamePipe();

  protected readonly relationTypes = CONTACT_RELATION_TYPES;

  protected form = this.fb.nonNullable.group({
    contactSearch: [''],
    relationType: [ContactRelationType.Undefined as ContactRelationType, Validators.required],
  });

  protected selected = signal<ContactOption | null>(null);

  private searchTerm = toSignal(this.form.controls.contactSearch.valueChanges.pipe(debounceTime(200)), {
    initialValue: '',
  });

  protected suggestionsRes = rxResource<ContactOption[], string>({
    params: () => (this.searchTerm() ?? '').trim(),
    stream: ({ params }) => {
      if (params.length < 1) return of([]);
      return this.api.list<Contact>('/contacts', { search: params }).pipe(
        map((r: PaginatedResult<Contact>) =>
          r.items
            .filter((c) => !this.data.existingContactIds.has(c.id))
            .map((c) => ({ id: c.id, label: this.label(c) })),
        ),
        catchError(() => of([])),
      );
    },
  });
  protected suggestions = computed(() => this.suggestionsRes.value() ?? []);

  protected pick(item: ContactOption): void {
    this.selected.set(item);
  }

  protected save(): void {
    const contact = this.selected();
    if (!contact || this.form.invalid) return;

    this.ref.close({
      eucharisticYouthId: this.data.youthId,
      contactId: contact.id,
      relationType: this.form.getRawValue().relationType,
    });
  }

  private label(c: Contact): string {
    const extra = c.phoneNumber || c.email;
    const name = this.fullNamePipe.transform(c.fullName);
    return extra ? `${name} — ${extra}` : name;
  }
}
