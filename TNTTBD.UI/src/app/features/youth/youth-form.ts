import { Component, inject, signal, computed, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import {
  FormBuilder,
  FormArray,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { toSignal } from '@angular/core/rxjs-interop';
import { switchMap, of } from 'rxjs';
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
import { ApiService } from '../../core/services/api.service';
import { NotificationService } from '../../core/services/notification.service';
import { AuthService } from '../../core/auth/auth.service';
import {
  CatechistCurrentClass,
  EucharisticYouth,
  PatronSaint,
} from '../../shared/models';
import { ContactRelationType, MilestoneType, UserRole } from '../../shared/enums';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { PatronSaintInput } from '../../shared/components/patron-saint-input';
import { PhotoUpload } from '../../shared/components/photo-upload';
import { TranslationService } from '../../core/i18n/translation.service';

@Component({
  selector: 'app-youth-form',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
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
    PhotoUpload,
  ],
  templateUrl: './youth-form.html',
  styleUrl: './youth-form.scss',

  changeDetection: ChangeDetectionStrategy.OnPush
})
export class YouthForm {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private api = inject(ApiService);
  private notify = inject(NotificationService);
  private auth = inject(AuthService);
  private translate = inject(TranslationService);

  protected readonly UserRole = UserRole;

  protected readonly milestoneTypes = [
    { v: MilestoneType.Baptism, k: 'milestone.baptism' },
    { v: MilestoneType.FirstPenance, k: 'milestone.firstPenance' },
    { v: MilestoneType.FirstCommunion, k: 'milestone.firstCommunion' },
    { v: MilestoneType.Confirmation, k: 'milestone.confirmation' },
    { v: MilestoneType.SolemnCommunion, k: 'milestone.solemnCommunion' },
    { v: MilestoneType.Graduation, k: 'milestone.graduation' },
  ];

  protected readonly relationTypes = [
    { v: ContactRelationType.Father, k: 'contact.relation.Father' },
    { v: ContactRelationType.Mother, k: 'contact.relation.Mother' },
    { v: ContactRelationType.Guardian, k: 'contact.relation.Guardian' },
    { v: ContactRelationType.Grandfather, k: 'contact.relation.Grandfather' },
    { v: ContactRelationType.Grandmother, k: 'contact.relation.Grandmother' },
    { v: ContactRelationType.Babysitter, k: 'contact.relation.Babysitter' },
    { v: ContactRelationType.Sibling, k: 'contact.relation.Sibling' },
    { v: ContactRelationType.Undefined, k: 'contact.relation.Undefined' },
  ];

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

    // Resolve catechist's current class for "add to my class" checkbox
    const user = this.auth.currentUser();
    if (user?.userRole === UserRole.Catechist && user.catechistId) {
      this.isCatechist.set(true);
      this.api
        .get<CatechistCurrentClass | null>('/catechists/current-class')
        .subscribe((current) => {
          if (!current) return;
          this.myCurrentClassId.set(current.classId);
          this.myClassName.set(current.className);
        });
    }

    this.route.queryParamMap
      .pipe(
        switchMap((qp) => {
          const id = qp.get('id');
          if (!id) return of(null);
          this.isEdit.set(true);
          this.editId = id;
          return this.api.get<EucharisticYouth>('/youths/detail', { id });
        }),
      )
      .subscribe((y) => {
        if (!y) return;
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
      });
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

  protected removeContact(i: number): void {
    this.contacts.removeAt(i);
  }

  protected onPhotoUploaded(): void {
    this.notify.success('photo.uploaded');
  }

  protected submit(): void {
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
        contacts: (v.contacts as Array<{ relationType: ContactRelationType; christName: string; familyName: string; middleName: string; givenName: string; phoneNumber: string; email: string }>).map((c) => ({
          relationType: c.relationType,
          givenName: c.givenName,
          christName: c.christName || null,
          familyName: c.familyName || null,
          middleName: c.middleName || null,
          phoneNumber: c.phoneNumber || null,
          email: c.email || null,
        })),
      }),
      catechismClassId: this.enrollInClassId() ?? (this.addToMyClass() && this.myCurrentClassId() ? this.myCurrentClassId() : null),
    };

    this.loading.set(true);
    const req$ = this.isEdit()
      ? this.api.patch('/youths', payload)
      : this.api.post('/youths', payload);

    req$.subscribe({
      next: (res: any) => {
        this.loading.set(false);
        this.notify.success(this.translate.t('common.saved'));
        this.navigateBack();
      },
      error: () => {
        this.loading.set(false);
        this.navigateBack();
      },
    });
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
      relationType: [relationType, Validators.required],
      christName: [christName],
      familyName: [familyName],
      middleName: [middleName],
      givenName: [givenName, Validators.required],
      phoneNumber: [phoneNumber],
      email: [email],
    });
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
