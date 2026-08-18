import { Component, inject, signal, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatRadioModule } from '@angular/material/radio';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { debounceTime, of, switchMap } from 'rxjs';
import { toSignal } from '@angular/core/rxjs-interop';
import { ApiService } from '../../core/services/api.service';
import { User, Catechist, EucharisticYouth, PaginatedResult } from '../../shared/models';
import { UserRole } from '../../shared/enums';
import { TranslatePipe } from '../../core/i18n/translate.pipe';

// Backend Contact DTO (Application layer)
export interface ContactLookup {
  id: string;
  fullName: { givenName: string; familyName?: string | null };
  phoneNumber?: string | null;
  email?: string | null;
}

export type LinkKind = 'None' | 'Catechist' | 'Youth' | 'Contact';

export interface UserFormData {
  mode: 'create' | 'edit';
  user?: User;
}

export interface UserFormResult {
  mode: 'create' | 'edit';
  create?: {
    email: string;
    userName: string;
    password: string;
    role: UserRole;
    catechistId?: string;
    eucharisticYouthId?: string;
    contactId?: string;
  };
  update?: {
    id: string;
    userName?: string;
    isActive?: boolean;
    role?: UserRole;
    link?: { kind: LinkKind; id?: string };
  };
}

@Component({
  selector: 'app-user-form-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatSlideToggleModule,
    MatRadioModule,
    MatAutocompleteModule,
    MatButtonModule,
    TranslatePipe,
  ],
  templateUrl: './user-form-dialog.html',
  styleUrl: './user-form-dialog.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UserFormDialog {
  protected ref = inject(MatDialogRef<UserFormDialog, UserFormResult>);
  protected data = inject<UserFormData>(MAT_DIALOG_DATA);
  private fb = inject(FormBuilder);
  private api = inject(ApiService);

  protected readonly roles: UserRole[] = [
    UserRole.Admin,
    UserRole.Catechist,
    UserRole.EucharisticYouth,
    UserRole.EucharisticYouthContact,
  ];

  protected readonly isEdit = this.data.mode === 'edit';

  private initialLinkKind: LinkKind =
    this.data.user?.catechistId ? 'Catechist'
    : this.data.user?.eucharisticYouthId ? 'Youth'
    : this.data.user?.contactId ? 'Contact'
    : 'None';

  private initialLinkedId: string | null =
    this.data.user?.catechistId
      ?? this.data.user?.eucharisticYouthId
      ?? this.data.user?.contactId
      ?? null;

  protected form = this.fb.nonNullable.group({
    email: [{ value: this.data.user?.email ?? '', disabled: this.isEdit }, [Validators.required, Validators.email]],
    userName: [this.data.user?.userName ?? '', Validators.required],
    password: [''],
    role: [this.data.user?.userRole ?? UserRole.EucharisticYouthContact, Validators.required],
    isActive: [this.data.user?.isActive ?? true],
    linkKind: [this.initialLinkKind as LinkKind],
    linkQuery: [''],
  });

  protected selectedLinkedId = signal<string | null>(this.initialLinkedId);
  protected selectedLinkedLabel = signal<string>('');

  // Reactively query the correct endpoint whenever the search text or link kind changes
  protected suggestions = toSignal(
    this.form.controls.linkQuery.valueChanges.pipe(
      debounceTime(200),
      switchMap((search) => {
        const kind = this.form.controls.linkKind.value;
        if (kind === 'None') return of([]);
        const s = (search ?? '').trim();
        if (kind === 'Catechist') {
          return this.api
            .list<Catechist>('/catechists')
            .pipe(switchMap((r: PaginatedResult<Catechist>) => of(this.filterCatechists(r.items, s))));
        }
        if (kind === 'Youth') {
          return this.api
            .list<EucharisticYouth>('/youths', { search: s || undefined })
            .pipe(switchMap((r: PaginatedResult<EucharisticYouth>) => of(this.mapYouths(r.items))));
        }
        return this.api
          .list<ContactLookup>('/contacts', { search: s || undefined })
          .pipe(switchMap((r: PaginatedResult<ContactLookup>) => of(this.mapContacts(r.items))));
      }),
    ),
    { initialValue: [] as { id: string; label: string }[] },
  );

  constructor() {
    if (this.isEdit) {
      this.form.controls.password.clearValidators();
    } else {
      this.form.controls.password.setValidators([Validators.required, Validators.minLength(6)]);
    }
    if (this.initialLinkedId) {
      this.selectedLinkedLabel.set(this.initialLinkedId);
    }
  }

  protected onLinkKindChange(): void {
    this.selectedLinkedId.set(null);
    this.selectedLinkedLabel.set('');
    this.form.controls.linkQuery.setValue('');
  }

  protected pickSuggestion(item: { id: string; label: string }): void {
    this.selectedLinkedId.set(item.id);
    this.selectedLinkedLabel.set(item.label);
    this.form.controls.linkQuery.setValue(item.label, { emitEvent: false });
  }

  protected save(): void {
    if (this.form.invalid) return;
    const v = this.form.getRawValue();
    const linkKind = v.linkKind;
    const linkedId = this.selectedLinkedId();

    if (this.isEdit && this.data.user) {
      const result: UserFormResult = {
        mode: 'edit',
        update: {
          id: this.data.user.id,
          userName: v.userName,
          isActive: v.isActive,
          role: v.role,
          link: {
            kind: linkKind,
            id: linkKind === 'None' ? undefined : (linkedId ?? undefined),
          },
        },
      };
      this.ref.close(result);
      return;
    }

    const result: UserFormResult = {
      mode: 'create',
      create: {
        email: v.email,
        userName: v.userName,
        password: v.password,
        role: v.role,
        catechistId: linkKind === 'Catechist' ? (linkedId ?? undefined) : undefined,
        eucharisticYouthId: linkKind === 'Youth' ? (linkedId ?? undefined) : undefined,
        contactId: linkKind === 'Contact' ? (linkedId ?? undefined) : undefined,
      },
    };
    this.ref.close(result);
  }

  private filterCatechists(items: Catechist[], search: string): { id: string; label: string }[] {
    const s = search.toLowerCase();
    return items
      .filter((c) => {
        if (!s) return true;
        const name = [c.fullName.givenName, c.fullName.familyName, c.fullName.christName]
          .filter(Boolean).join(' ').toLowerCase();
        return name.includes(s);
      })
      .map((c) => ({
        id: c.id,
        label: [c.fullName.christName, c.fullName.familyName, c.fullName.middleName, c.fullName.givenName]
          .filter(Boolean).join(' '),
      }));
  }

  private mapYouths(items: EucharisticYouth[]): { id: string; label: string }[] {
    return items.map((y) => ({
      id: y.id,
      label: `${y.code} — ` + [y.fullName.christName, y.fullName.familyName, y.fullName.middleName, y.fullName.givenName]
        .filter(Boolean).join(' '),
    }));
  }

  private mapContacts(items: ContactLookup[]): { id: string; label: string }[] {
    return items.map((c) => ({
      id: c.id,
      label: [c.fullName.familyName, c.fullName.givenName].filter(Boolean).join(' ')
        + (c.phoneNumber ? ` (${c.phoneNumber})` : ''),
    }));
  }
}
