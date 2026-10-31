import { Component, inject, signal, computed, effect, untracked, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import {
  FormBuilder,
  FormArray,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDividerModule } from '@angular/material/divider';
import { MatRadioModule } from '@angular/material/radio';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { NotificationService } from '../../../core/services/notification.service';
import { DialogService } from '../../../core/services/dialog.service';
import { ApiService } from '../../../core/services/api.service';
import { AuthService } from '../../../core/auth/auth.service';
import { Contact, EucharisticYouth, PatronSaint } from '../../../shared/models';
import { ContactRelationType, MilestoneType, UserRole } from '../../../shared/enums';
import { CONTACT_RELATION_TYPES } from '../../../shared/constants/contact-relation-types';
import { TranslatePipe } from '../../../core/i18n/translate.pipe';
import { PatronSaintInput } from '../../../shared/components/patron-saint-input/patron-saint-input';
import {
  LinkContactDialog,
  LinkContactData,
  LinkContactResult,
} from '../../../shared/components/link-contact-dialog/link-contact-dialog';
import { FullNamePipe } from '../../../shared/pipes/full-name.pipe';
import { TranslationService } from '../../../core/i18n/translation.service';
import { YouthService } from '../youth.service';

@Component({
  selector: 'app-youth-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatDatepickerModule,
    MatDividerModule,
    MatRadioModule,
    MatCheckboxModule,
    TranslatePipe,
    PatronSaintInput,
  ],
  templateUrl: './youth-form.html',
  styleUrl: './youth-form.scss',

  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [YouthService],
})
export class YouthForm {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private youthService = inject(YouthService);
  private notify = inject(NotificationService);
  private auth = inject(AuthService);
  private translate = inject(TranslationService);
  private dialogs = inject(DialogService);
  private api = inject(ApiService);
  private fullNamePipe = new FullNamePipe();

  protected readonly UserRole = UserRole;

  protected readonly milestoneTypes = [
    { v: MilestoneType.Baptism, k: 'milestone.baptism' },
    { v: MilestoneType.FirstPenance, k: 'milestone.firstPenance' },
    { v: MilestoneType.FirstCommunion, k: 'milestone.firstCommunion' },
    { v: MilestoneType.Confirmation, k: 'milestone.confirmation' },
    { v: MilestoneType.SolemnCommunion, k: 'milestone.solemnCommunion' },
    { v: MilestoneType.Graduation, k: 'milestone.graduation' },
  ];

  protected readonly relationTypes = CONTACT_RELATION_TYPES;

  protected readonly ContactRelationType = ContactRelationType;

  protected loading = signal(false);
  protected isEdit = signal(false);
  protected editIdSignal = signal<string | null>(null);
  private get editId(): string | null { return this.editIdSignal(); }
  private set editId(v: string | null) { this.editIdSignal.set(v); }
  protected addToMyClass = signal(true);
  protected myCurrentClassId = signal<string | null>(null);
  protected myClassName = signal<string | null>(null);
  protected isCatechist = signal(false);
  // Set when navigating from class-detail with ?classId=
  protected enrollInClassId = signal<string | null>(null);
  #location = inject(Location);

  protected form = this.fb.group({
    givenName: ['', Validators.required],
    middleName: [''],
    familyName: [''],
    christName: [''],
    dateOfBirth: [null as Date | null, Validators.required],
    isMale: [true],
    address: [''],
    patronSaints: [[] as PatronSaint[]],
    milestones: this.fb.array<ReturnType<typeof this.makeMilestone>>([]),
    contacts: this.fb.array<ReturnType<typeof this.makeContact>>([]),
  });

  protected milestones = this.form.get('milestones') as FormArray;
  protected contacts = this.form.get('contacts') as FormArray;

  constructor() {
    // Pre-populate Father and Mother contacts for new youth
    this.contacts.push(this.makeContact(ContactRelationType.Father));
    this.contacts.push(this.makeContact(ContactRelationType.Mother));

    // Check for classId from class-detail navigation
    const classIdParam = this.route.snapshot.queryParamMap.get('classId');
    if (classIdParam) {
      this.enrollInClassId.set(classIdParam);
    }

    // Set when navigating from contact-detail "Add new youth" with ?contactId=
    const contactIdParam = this.route.snapshot.queryParamMap.get('contactId');
    if (contactIdParam && !this.route.snapshot.queryParamMap.get('id')) {
      void this.prefillContact(contactIdParam);
    }

    // Resolve catechist's current class for "add to my class" checkbox
    const user = this.auth.currentUser();
    if (user?.userRole === UserRole.Catechist && user.catechistId) {
      this.isCatechist.set(true);
      this.youthService.loadCurrentClass().then((current) => {
        if (!current) return;
        this.myCurrentClassId.set(current.classId);
        this.myClassName.set(current.className);
      });
    }

    const id = this.route.snapshot.queryParamMap.get('id');
    if (id) {
      this.isEdit.set(true);
      this.editId = id;
      this.youthService.detailId.set(id);
    }

    // Reactive forms are imperative; patching from the loaded resource is the accepted effect() exception.
    // untracked keeps signals read by child form controls from re-running this effect and discarding edits.
    effect(() => {
      const y = this.youthService.detail();
      if (!y) return;
      untracked(() => this.patchFromDetail(y));
    });
  }

  private patchFromDetail(y: EucharisticYouth): void {
    this.form.patchValue({
      givenName: y.fullName.givenName,
      middleName: y.fullName.middleName ?? '',
      familyName: y.fullName.familyName ?? '',
      christName: y.fullName.christName ?? '',
      dateOfBirth: y.dateOfBirth ? new Date(y.dateOfBirth + 'T00:00:00') : null,
      isMale: y.isMale,
      address: y.address ?? '',
      patronSaints: y.patronSaints ?? [],
    });
    this.milestones.clear();
    for (const m of y.milestones) {
      this.milestones.push(
        this.makeMilestone(
          m.type,
          m.date ? new Date(m.date + 'T00:00:00') : null,
          m.place ?? '',
          m.documentCode ?? '',
          m.note ?? '',
        ),
      );
    }
    // In edit mode contacts are managed via the detail page — clear defaults
    this.contacts.clear();
  }

  protected addMilestone(): void {
    this.milestones.push(this.makeMilestone(MilestoneType.Baptism, null, '', '', ''));
  }

  protected removeMilestone(i: number): void {
    this.milestones.removeAt(i);
  }

  protected addContact(): void {
    this.contacts.push(this.makeContact(ContactRelationType.Undefined));
  }

  protected async linkExistingContact(): Promise<void> {
    const existingContactIds = new Set<string>(
      this.contacts.controls.map((c) => c.value.contactId).filter((id): id is string => !!id),
    );
    const result = await this.dialogs.open<LinkContactDialog, LinkContactData, LinkContactResult>(LinkContactDialog, {
      data: { existingContactIds },
      width: '480px',
    });
    if (!result) return;
    this.contacts.push(this.makeExistingContact(result.relationType, result.contactId, result.contactName));
  }

  /** Replaces the blank Father/Mother defaults with the contact we came from. */
  private async prefillContact(contactId: string): Promise<void> {
    try {
      const c = await this.api.getAsync<Contact>('/contacts/detail', { id: contactId });
      if (!c) return;
      const name = this.fullNamePipe.transform(c.fullName);
      const extra = c.phoneNumber || c.email;
      this.contacts.clear();
      this.contacts.push(
        this.makeExistingContact(ContactRelationType.Undefined, c.id, extra ? `${name} — ${extra}` : name),
      );
    } catch {
      // error toast already shown by global http error handling
    }
  }

  protected removeContact(i: number): void {
    this.contacts.removeAt(i);
  }

  protected async submit(): Promise<void> {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const v = this.form.getRawValue();
    const payload = {
      ...(this.isEdit() ? { eucharisticYouthId: this.editId } : {}),
      givenName: v.givenName,
      middleName: v.middleName || null,
      familyName: v.familyName || null,
      christName: v.christName || null,
      dateOfBirth: this.toIsoDate(v.dateOfBirth),
      isMale: v.isMale,
      address: v.address || null,
      patronSaints: v.patronSaints,
      milestones: (v.milestones as Array<{ type: MilestoneType; date: Date | null; place: string; documentCode: string; note: string }>).map((m) => ({
        type: m.type,
        date: this.toIsoDate(m.date),
        place: m.place || null,
        documentCode: m.documentCode || null,
        note: m.note || null,
      })),
      ...(this.isEdit() ? {} : {
        contacts: (v.contacts as Array<{ contactId: string | null; relationType: ContactRelationType; christName: string; familyName: string; middleName: string; givenName: string; phoneNumber: string; email: string }>).map((c) =>
          c.contactId
            ? { relationType: c.relationType, contactId: c.contactId }
            : {
                relationType: c.relationType,
                givenName: c.givenName,
                christName: c.christName || null,
                familyName: c.familyName || null,
                middleName: c.middleName || null,
                phoneNumber: c.phoneNumber || null,
                email: c.email || null,
              }),
      }),
      catechismClassId: this.enrollInClassId() ?? (this.addToMyClass() && this.myCurrentClassId() ? this.myCurrentClassId() : null),
    };

    this.loading.set(true);
    try {
      await this.youthService.save(payload, this.isEdit());
      this.notify.success(this.translate.t('common.saved'));
      this.navigateBack();
    } catch {
      // swallow: error toast already shown by global http error handling
    } finally {
      this.loading.set(false);
    }
  }


  navigateBack(): void {
    this.#location.back();
  }

  private makeContact(
    relationType: ContactRelationType,
    christName = '',
    familyName = '',
    middleName = '',
    givenName = '',
    phoneNumber = '',
    email = '',
  ) {
    return this.fb.group({
      id: [crypto.randomUUID()],
      contactId: [null as string | null],
      contactName: [''],
      relationType: [relationType, Validators.required],
      christName: [christName],
      familyName: [familyName],
      middleName: [middleName],
      givenName: [givenName, Validators.required],
      phoneNumber: [phoneNumber],
      email: [email],
    });
  }

  /** Entry for an already-existing contact: only the relation is editable. */
  private makeExistingContact(relationType: ContactRelationType, contactId: string, contactName: string) {
    const group = this.makeContact(relationType);
    group.patchValue({ contactId, contactName });
    group.controls.givenName.clearValidators();
    group.controls.givenName.updateValueAndValidity();
    return group;
  }

  private makeMilestone(
    type: MilestoneType,
    date: Date | null,
    place: string,
    documentCode: string,
    note: string,
  ) {
    return this.fb.group({
      type: [type, Validators.required],
      date: [date],
      place: [place],
      documentCode: [documentCode],
      note: [note],
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
