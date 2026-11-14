import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { rxResource, toSignal } from '@angular/core/rxjs-interop';
import { catchError, debounceTime, map, of } from 'rxjs';
import { ApiService } from '../../../core/services/api.service';
import { TranslatePipe } from '../../../core/i18n/translate.pipe';
import { Contact, FullName, MilestoneSponsor, PaginatedResult } from '../../models';
import { FullNamePipe } from '../../pipes/full-name.pipe';

export interface GodparentPickerData {
  current?: MilestoneSponsor | null;
}

/**
 * Picks the godparent contact of a Baptism/Confirmation, or creates a new contact.
 * Closes with the chosen sponsor, or with undefined when cancelled.
 */
@Component({
  selector: 'app-godparent-picker-dialog',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatAutocompleteModule,
    MatButtonModule,
    MatButtonToggleModule,
    TranslatePipe,
  ],
  template: `
    <h2 mat-dialog-title>{{ 'milestone.sponsorPick' | t }}</h2>
    <mat-dialog-content class="form">
      <mat-button-toggle-group [value]="mode()" (change)="mode.set($event.value)" class="mode">
        <mat-button-toggle value="search">{{ 'contact.linkExisting' | t }}</mat-button-toggle>
        <mat-button-toggle value="create">{{ 'contact.addNew' | t }}</mat-button-toggle>
      </mat-button-toggle-group>

      @if (mode() === 'search') {
        <mat-form-field appearance="outline" class="full-width">
          <mat-label>{{ 'contact.searchContact' | t }}</mat-label>
          <input matInput [formControl]="search" [matAutocomplete]="auto" />
          <mat-autocomplete #auto>
            @for (c of suggestions(); track c.id) {
              <mat-option [value]="label(c)" (onSelectionChange)="$event.isUserInput && selected.set(c)">
                {{ label(c) }}
              </mat-option>
            }
          </mat-autocomplete>
        </mat-form-field>
      } @else {
        <form [formGroup]="form" class="create">
          <mat-form-field appearance="outline">
            <mat-label>{{ 'fullName.christ' | t }}</mat-label>
            <input matInput formControlName="christName" maxlength="100" />
          </mat-form-field>
          <mat-form-field appearance="outline">
            <mat-label>{{ 'fullName.family' | t }}</mat-label>
            <input matInput formControlName="familyName" maxlength="100" />
          </mat-form-field>
          <mat-form-field appearance="outline">
            <mat-label>{{ 'fullName.middle' | t }}</mat-label>
            <input matInput formControlName="middleName" maxlength="100" />
          </mat-form-field>
          <mat-form-field appearance="outline">
            <mat-label>{{ 'fullName.given' | t }}</mat-label>
            <input matInput formControlName="givenName" maxlength="100" required />
          </mat-form-field>
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>{{ 'contact.phone' | t }}</mat-label>
            <input matInput formControlName="phoneNumber" type="tel" maxlength="30" />
          </mat-form-field>
        </form>
      }
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button type="button" (click)="ref.close()">{{ 'common.cancel' | t }}</button>
      <button mat-flat-button color="primary" type="button" [disabled]="!canSave() || saving()" (click)="save()">
        {{ 'common.save' | t }}
      </button>
    </mat-dialog-actions>
  `,
  styles: `
    .form { display: flex; flex-direction: column; gap: 12px; min-width: min(480px, 80vw); }
    .mode { align-self: flex-start; }
    .full-width { width: 100%; }
    .create { display: grid; grid-template-columns: 1fr 1fr; column-gap: 12px; }
    .create .full-width { grid-column: 1 / -1; }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class GodparentPickerDialog {
  protected ref = inject<MatDialogRef<GodparentPickerDialog, MilestoneSponsor>>(MatDialogRef);
  private data = inject<GodparentPickerData | null>(MAT_DIALOG_DATA, { optional: true });
  private api = inject(ApiService);
  private fb = inject(FormBuilder);
  private fullNamePipe = new FullNamePipe();

  protected mode = signal<'search' | 'create'>('search');
  protected selected = signal<Contact | null>(null);
  protected saving = signal(false);

  protected search = this.fb.nonNullable.control(this.data?.current ? this.sponsorLabel(this.data.current) : '');
  protected form = this.fb.nonNullable.group({
    christName: [''],
    familyName: [''],
    middleName: [''],
    givenName: ['', [Validators.required, Validators.maxLength(100)]],
    phoneNumber: [''],
  });

  private searchTerm = toSignal(this.search.valueChanges.pipe(debounceTime(200)), { initialValue: '' });
  private formStatus = toSignal(this.form.statusChanges, { initialValue: this.form.status });

  private suggestionsRes = rxResource<Contact[], string>({
    params: () => (this.searchTerm() ?? '').trim(),
    stream: ({ params }) => {
      if (params.length < 1) return of([]);
      return this.api.list<Contact>('/contacts', { search: params }).pipe(
        map((r: PaginatedResult<Contact>) => r.items),
        catchError(() => of([])),
      );
    },
  });
  protected suggestions = computed(() => this.suggestionsRes.value() ?? []);

  protected canSave = computed(() => (this.mode() === 'search' ? !!this.selected() : this.formStatus() === 'VALID'));

  protected label(c: Contact): string {
    return this.sponsorLabel({ contactId: c.id, fullName: c.fullName, phoneNumber: c.phoneNumber });
  }

  protected async save(): Promise<void> {
    if (this.mode() === 'search') {
      const c = this.selected();
      if (c) this.ref.close({ contactId: c.id, fullName: c.fullName, phoneNumber: c.phoneNumber });
      return;
    }
    if (this.form.invalid) return;
    const v = this.form.getRawValue();
    const fullName: FullName = {
      givenName: v.givenName.trim(),
      christName: v.christName.trim() || null,
      familyName: v.familyName.trim() || null,
      middleName: v.middleName.trim() || null,
    };
    const phoneNumber = v.phoneNumber.trim() || null;
    this.saving.set(true);
    try {
      const id = await this.api.postAsync<string>('/contacts', { ...fullName, phoneNumber, email: null });
      this.ref.close({ contactId: id, fullName, phoneNumber });
    } catch {
      // The HTTP error interceptor reports the failure; keep the dialog open.
    } finally {
      this.saving.set(false);
    }
  }

  private sponsorLabel(s: MilestoneSponsor): string {
    const name = this.fullNamePipe.transform(s.fullName);
    return s.phoneNumber ? `${name} — ${s.phoneNumber}` : name;
  }
}
