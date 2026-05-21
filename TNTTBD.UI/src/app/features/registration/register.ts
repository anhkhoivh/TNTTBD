import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators, FormArray } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatStepperModule } from '@angular/material/stepper';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatToolbarModule } from '@angular/material/toolbar';
import { ApiService } from '../../core/services/api.service';
import { NotificationService } from '../../core/services/notification.service';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { ContactRelationType } from '../../shared/enums';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatStepperModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatRadioModule,
    MatButtonModule,
    MatIconModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatCheckboxModule,
    MatToolbarModule,
    TranslatePipe,
  ],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class RegisterPage {
  private fb = inject(FormBuilder);
  private api = inject(ApiService);
  private notify = inject(NotificationService);
  private router = inject(Router);

  protected readonly relations = [
    { v: ContactRelationType.Father, k: 'contact.relation.Father' },
    { v: ContactRelationType.Mother, k: 'contact.relation.Mother' },
    { v: ContactRelationType.Guardian, k: 'contact.relation.Guardian' },
    { v: ContactRelationType.Grandfather, k: 'contact.relation.Grandfather' },
    { v: ContactRelationType.Grandmother, k: 'contact.relation.Grandmother' },
    { v: ContactRelationType.Babysitter, k: 'contact.relation.Babysitter' },
    { v: ContactRelationType.Sibling, k: 'contact.relation.Sibling' },
    { v: ContactRelationType.Undefined, k: 'contact.relation.Undefined' },
  ];

  protected loading = signal(false);

  protected youthForm = this.fb.group({
    givenName: ['', Validators.required],
    middleName: [''],
    familyName: [''],
    christName: [''],
    dateOfBirth: [null as Date | null, Validators.required],
    isMale: [true, Validators.required],
    address: [''],
    baptismDate: [null as Date | null],
    baptismPlace: [''],
  });

  protected contactsForm = this.fb.group({
    contacts: this.fb.array<ReturnType<typeof this.makeContact>>([this.makeContact()]),
  });

  protected accountForm = this.fb.group({
    hasAccount: [false],
    email: [''],
  });

  protected contacts = this.contactsForm.get('contacts') as FormArray;

  protected addContact(): void {
    this.contacts.push(this.makeContact());
  }

  protected removeContact(i: number): void {
    if (this.contacts.length > 1) this.contacts.removeAt(i);
  }

  protected submit(): void {
    if (this.youthForm.invalid || this.contactsForm.invalid) return;
    const y = this.youthForm.getRawValue();
    const payload = {
      youth: {
        fullName: {
          givenName: y.givenName,
          middleName: y.middleName || null,
          familyName: y.familyName || null,
          christName: y.christName || null,
        },
        dateOfBirth: this.toIsoDate(y.dateOfBirth),
        isMale: y.isMale,
        address: y.address || null,
        baptism: y.baptismDate
          ? { date: this.toIsoDate(y.baptismDate), place: y.baptismPlace || null }
          : null,
      },
      contacts: this.contacts.getRawValue(),
      accountEmail: this.accountForm.get('hasAccount')?.value ? this.accountForm.get('email')?.value : null,
    };
    this.loading.set(true);
    this.api.post('/registration/youth', payload).subscribe({
      next: () => {
        this.loading.set(false);
        this.notify.success('Registration submitted');
        this.router.navigate(['/']);
      },
      error: () => {
        this.loading.set(false);
        this.notify.error('Submit failed');
      },
    });
  }

  private makeContact() {
    return this.fb.group({
      givenName: ['', Validators.required],
      familyName: [''],
      relation: [ContactRelationType.Father, Validators.required],
      phoneNumber: ['', Validators.required],
      email: [''],
    });
  }

  private toIsoDate(d: Date | null): string | null {
    if (!d) return null;
    const yyyy = d.getFullYear();
    const mm = String(d.getMonth() + 1).padStart(2, '0');
    const dd = String(d.getDate()).padStart(2, '0');
    return `${yyyy}-${mm}-${dd}`;
  }
}

