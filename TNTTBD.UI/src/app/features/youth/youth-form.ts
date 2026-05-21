import { Component, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
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
import { MatNativeDateModule } from '@angular/material/core';
import { MatDividerModule } from '@angular/material/divider';
import { MatRadioModule } from '@angular/material/radio';
import { ApiService } from '../../core/services/api.service';
import { NotificationService } from '../../core/services/notification.service';
import { EucharisticYouth } from '../../shared/models';
import { MilestoneType } from '../../shared/enums';
import { TranslatePipe } from '../../core/i18n/translate.pipe';

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
    MatNativeDateModule,
    MatDividerModule,
    MatRadioModule,
    TranslatePipe,
  ],
  templateUrl: './youth-form.html',
  styleUrl: './youth-form.scss',
})
export class YouthForm {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private api = inject(ApiService);
  private notify = inject(NotificationService);

  protected readonly milestoneTypes = [
    { v: MilestoneType.Baptism, k: 'milestone.baptism' },
    { v: MilestoneType.FirstPenance, k: 'milestone.firstPenance' },
    { v: MilestoneType.FirstCommunion, k: 'milestone.firstCommunion' },
    { v: MilestoneType.Confirmation, k: 'milestone.confirmation' },
    { v: MilestoneType.SolemnCommunion, k: 'milestone.solemnCommunion' },
    { v: MilestoneType.Graduation, k: 'milestone.graduation' },
  ];

  protected loading = signal(false);
  protected isEdit = signal(false);

  protected form = this.fb.group({
    code: ['', Validators.required],
    givenName: ['', Validators.required],
    middleName: [''],
    familyName: [''],
    christName: [''],
    dateOfBirth: [null as Date | null, Validators.required],
    isMale: [true, Validators.required],
    address: [''],
    milestones: this.fb.array<ReturnType<typeof this.makeMilestone>>([]),
  });

  protected milestones = this.form.get('milestones') as FormArray;

  constructor() {
    this.route.queryParamMap
      .pipe(
        switchMap((qp) => {
          const id = qp.get('id');
          if (!id) return of(null);
          this.isEdit.set(true);
          return this.api.get<EucharisticYouth>('/youths/detail', { id });
        }),
      )
      .subscribe((y) => {
        if (!y) return;
        this.form.patchValue({
          code: y.code,
          givenName: y.fullName.givenName,
          middleName: y.fullName.middleName ?? '',
          familyName: y.fullName.familyName ?? '',
          christName: y.fullName.christName ?? '',
          dateOfBirth: y.dateOfBirth ? new Date(y.dateOfBirth + 'T00:00:00') : null,
          isMale: y.isMale,
          address: y.address ?? '',
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
      });
  }

  protected addMilestone(): void {
    this.milestones.push(this.makeMilestone(MilestoneType.Baptism, null, '', '', ''));
  }

  protected removeMilestone(i: number): void {
    this.milestones.removeAt(i);
  }

  protected submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    const v = this.form.getRawValue();
    const payload = {
      code: v.code,
      fullName: {
        givenName: v.givenName,
        middleName: v.middleName || null,
        familyName: v.familyName || null,
        christName: v.christName || null,
      },
      dateOfBirth: this.toIsoDate(v.dateOfBirth),
      isMale: v.isMale,
      address: v.address || null,
      milestones: (v.milestones as Array<{ type: MilestoneType; date: Date | null; place: string; documentCode: string; note: string }>).map((m) => ({
        type: m.type,
        date: this.toIsoDate(m.date),
        place: m.place || null,
        documentCode: m.documentCode || null,
        note: m.note || null,
      })),
    };

    this.loading.set(true);
    const req$ = this.isEdit()
      ? this.api.patch('/youths', payload)
      : this.api.post('/youths', payload);

    req$.subscribe({
      next: () => {
        this.loading.set(false);
        this.notify.success('Saved');
        this.router.navigate(['/app/youth']);
      },
      error: () => {
        this.loading.set(false);
        this.notify.error('Save failed');
      },
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
      date: [date, Validators.required],
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
