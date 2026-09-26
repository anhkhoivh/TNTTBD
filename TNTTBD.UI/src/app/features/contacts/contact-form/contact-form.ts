import { Component, inject, signal, effect, ChangeDetectionStrategy } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { NotificationService } from '../../../core/services/notification.service';
import { TranslationService } from '../../../core/i18n/translation.service';
import { TranslatePipe } from '../../../core/i18n/translate.pipe';
import { ContactRelationType } from '../../../shared/enums';
import { CONTACT_RELATION_TYPES } from '../../../shared/constants/contact-relation-types';
import { ContactService } from '../contact.service';

@Component({
  selector: 'app-contact-form',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    TranslatePipe,
  ],
  templateUrl: './contact-form.html',
  styleUrl: './contact-form.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [ContactService],
})
export class ContactForm {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private contactService = inject(ContactService);
  private notify = inject(NotificationService);
  private translate = inject(TranslationService);
  #location = inject(Location);

  protected readonly relationTypes = CONTACT_RELATION_TYPES;

  protected loading = signal(false);
  protected isEdit = signal(false);
  private editId: string | null = null;
  /** Set when navigating from youth-detail "Add new" with ?youthId= */
  protected linkYouthId = signal<string | null>(null);

  protected form = this.fb.nonNullable.group({
    christName: [''],
    familyName: [''],
    middleName: [''],
    givenName: ['', Validators.required],
    phoneNumber: [''],
    email: ['', Validators.email],
    relationType: [ContactRelationType.Undefined as ContactRelationType],
  });

  constructor() {
    const params = this.route.snapshot.queryParamMap;
    const id = params.get('id');
    if (id) {
      this.isEdit.set(true);
      this.editId = id;
      this.contactService.detailId.set(id);
    } else {
      this.linkYouthId.set(params.get('youthId'));
    }

    // Reactive forms are imperative; patching from the loaded resource is the accepted effect() exception.
    effect(() => {
      const c = this.contactService.detail();
      if (!c) return;
      this.form.patchValue({
        christName: c.fullName.christName ?? '',
        familyName: c.fullName.familyName ?? '',
        middleName: c.fullName.middleName ?? '',
        givenName: c.fullName.givenName,
        phoneNumber: c.phoneNumber ?? '',
        email: c.email ?? '',
      });
    });
  }

  protected async submit(): Promise<void> {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const v = this.form.getRawValue();
    const youthId = this.linkYouthId();
    const payload = {
      ...(this.isEdit() ? { id: this.editId } : {}),
      givenName: v.givenName,
      christName: v.christName || null,
      familyName: v.familyName || null,
      middleName: v.middleName || null,
      phoneNumber: v.phoneNumber || null,
      email: v.email || null,
      ...(!this.isEdit() && youthId ? { linkEucharisticYouthId: youthId, linkRelationType: v.relationType } : {}),
    };

    this.loading.set(true);
    try {
      await this.contactService.save(payload, this.isEdit());
      this.notify.success(this.translate.t('common.saved'));
      this.navigateBack();
    } catch {
      // error toast already shown by global http error handling
    } finally {
      this.loading.set(false);
    }
  }

  navigateBack(): void {
    this.#location.back();
  }
}
