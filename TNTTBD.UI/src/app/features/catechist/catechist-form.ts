import { Component, inject, signal, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { switchMap, of } from 'rxjs';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatRadioModule } from '@angular/material/radio';
import { ApiService } from '../../core/services/api.service';
import { NotificationService } from '../../core/services/notification.service';
import { AuthService } from '../../core/auth/auth.service';
import { Catechist, PatronSaint } from '../../shared/models';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { PatronSaintInput } from '../../shared/components/patron-saint-input';
import { PhotoUpload } from '../../shared/components/photo-upload';
import { TranslationService } from '../../core/i18n/translation.service';

@Component({
  selector: 'app-catechist-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatDatepickerModule,
    MatRadioModule,
    TranslatePipe,
    PatronSaintInput,
    PhotoUpload,
  ],
  templateUrl: './catechist-form.html',
  styleUrl: './catechist-form.scss',

  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CatechistForm {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private api = inject(ApiService);
  private notify = inject(NotificationService);
  private auth = inject(AuthService);
  private translate = inject(TranslationService);
  #location = inject(Location);

  protected loading = signal(false);
  protected isEdit = signal(false);
  protected editIdSignal = signal<string | null>(null);
  private get editId(): string | null { return this.editIdSignal(); }
  private set editId(v: string | null) { this.editIdSignal.set(v); }

  protected form = this.fb.group({
    givenName: ['', Validators.required],
    middleName: [''],
    familyName: [''],
    christName: [''],
    dateOfBirth: [null as Date | null, Validators.required],
    isMale: [true, Validators.required],
    address: [''],
    patronSaints: [[] as PatronSaint[]],
  });

  constructor() {
    this.route.queryParamMap
      .pipe(
        switchMap((qp) => {
          const id = qp.get('id');
          if (!id) return of(null);
          // Catechist role can only edit their own profile
          const user = this.auth.currentUser();
          if (user?.userRole !== 'Admin' && user?.catechistId !== id) {
            this.router.navigate(['/app/catechists']);
            return of(null);
          }
          this.isEdit.set(true);
          this.editId = id;
          return this.api.get<Catechist>('/catechists/detail', { id });
        }),
      )
      .subscribe((c) => {
        if (!c) return;
        this.form.patchValue({
          givenName: c.fullName.givenName,
          middleName: c.fullName.middleName ?? '',
          familyName: c.fullName.familyName ?? '',
          christName: c.fullName.christName ?? '',
          dateOfBirth: c.dateOfBirth ? new Date(c.dateOfBirth + 'T00:00:00') : null,
          isMale: c.isMale,
          address: c.address ?? '',
          patronSaints: c.patronSaints ?? [],
        });
      });
  }

  protected submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    const v = this.form.getRawValue();
    const payload = {
      ...(this.isEdit() ? { catechistId: this.editId } : {}),
      givenName: v.givenName,
      middleName: v.middleName || null,
      familyName: v.familyName || null,
      christName: v.christName || null,
      dateOfBirth: this.toIsoDate(v.dateOfBirth),
      isMale: v.isMale,
      address: v.address || null,
      patronSaints: v.patronSaints,
    };

    this.loading.set(true);
    const req$ = this.isEdit()
      ? this.api.patch('/catechists', payload)
      : this.api.post('/catechists', payload);

    req$.subscribe({
      next: (res: any) => {
        this.loading.set(false);
        this.notify.success(this.translate.t('common.saved'));
        this.navigateBack();
      },
      error: () => {
        this.loading.set(false);
      },
    });
  }

  private toIsoDate(d: Date | null): string | null {
    if (!d) return null;
    const yyyy = d.getFullYear();
    const mm = String(d.getMonth() + 1).padStart(2, '0');
    const dd = String(d.getDate()).padStart(2, '0');
    return `${yyyy}-${mm}-${dd}`;
  }

  protected onPhotoUploaded(): void {
    this.notify.success('photo.uploaded');
  }

  navigateBack(): void {
    this.#location.back();
  }
}
