import { Component, inject, signal, effect, untracked, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatRadioModule } from '@angular/material/radio';
import { NotificationService } from '../../../core/services/notification.service';
import { AuthService } from '../../../core/auth/auth.service';
import { PatronSaint } from '../../../shared/models';
import { TranslatePipe } from '../../../core/i18n/translate.pipe';
import { PatronSaintInput } from '../../../shared/components/patron-saint-input/patron-saint-input';
import { TranslationService } from '../../../core/i18n/translation.service';
import { CatechistService } from '../catechist.service';

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
  ],
  templateUrl: './catechist-form.html',
  styleUrl: './catechist-form.scss',

  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [CatechistService],
})
export class CatechistForm {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private catechistService = inject(CatechistService);
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
    const id = this.route.snapshot.queryParamMap.get('id');
    if (id) {
      // Catechist role can only edit their own profile
      const user = this.auth.currentUser();
      if (user?.userRole !== 'Admin' && user?.catechistId !== id) {
        this.router.navigate(['/app/catechists']);
      } else {
        this.isEdit.set(true);
        this.editId = id;
        this.catechistService.detailId.set(id);
      }
    }

    // Reactive forms are imperative; patching from the loaded resource is the accepted effect() exception.
    // untracked keeps signals read by child form controls from re-running this effect and discarding edits.
    effect(() => {
      const c = this.catechistService.detail();
      if (!c) return;
      untracked(() => this.form.patchValue({
        givenName: c.fullName.givenName,
        middleName: c.fullName.middleName ?? '',
        familyName: c.fullName.familyName ?? '',
        christName: c.fullName.christName ?? '',
        dateOfBirth: c.dateOfBirth ? new Date(c.dateOfBirth + 'T00:00:00') : null,
        isMale: c.isMale,
        address: c.address ?? '',
        patronSaints: c.patronSaints ?? [],
      }));
    });
  }

  protected async submit(): Promise<void> {
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
    try {
      await this.catechistService.save(payload, this.isEdit());
      this.notify.success(this.translate.t('common.saved'));
      this.navigateBack();
    } catch {
      // swallow: error toast already shown by global http error handling
    } finally {
      this.loading.set(false);
    }
  }

  private toIsoDate(d: Date | null): string | null {
    if (!d) return null;
    const yyyy = d.getFullYear();
    const mm = String(d.getMonth() + 1).padStart(2, '0');
    const dd = String(d.getDate()).padStart(2, '0');
    return `${yyyy}-${mm}-${dd}`;
  }

  navigateBack(): void {
    this.#location.back();
  }
}
