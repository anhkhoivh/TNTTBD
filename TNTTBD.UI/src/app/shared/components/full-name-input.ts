import { Component, forwardRef , ChangeDetectionStrategy, inject} from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ControlValueAccessor,
  FormBuilder,
  ReactiveFormsModule,
  NG_VALUE_ACCESSOR,
  Validators,
} from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { FullName } from '../models';

@Component({
  selector: 'app-full-name-input',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    TranslatePipe,
  ],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => FullNameInput),
      multi: true,
    },
  ],
  template: `
    <div class="name-grid" [formGroup]="form">
      <mat-form-field appearance="outline">
        <mat-label>{{ 'fullName.christ' | t }}</mat-label>
        <input matInput formControlName="christName" />
      </mat-form-field>
      <mat-form-field appearance="outline">
        <mat-label>{{ 'fullName.family' | t }}</mat-label>
        <input matInput formControlName="familyName" />
      </mat-form-field>
      <mat-form-field appearance="outline">
        <mat-label>{{ 'fullName.middle' | t }}</mat-label>
        <input matInput formControlName="middleName" />
      </mat-form-field>
      <mat-form-field appearance="outline">
        <mat-label>{{ 'fullName.given' | t }}</mat-label>
        <input matInput formControlName="givenName" />
      </mat-form-field>
    </div>
  `,
  styles: [
    `
      .name-grid {
        display: grid;
        grid-template-columns: 1fr 1fr;
        gap: 0 1rem;
      }
    `,
  ],

  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FullNameInput implements ControlValueAccessor {
  private fb = inject(FormBuilder);

  form = this.fb.nonNullable.group({
    christName: [''],
    familyName: [''],
    middleName: [''],
    givenName: ['', Validators.required],
  });

  private onChange: (val: FullName) => void = () => {};
  private onTouched: () => void = () => {};

  constructor() {
    this.form.valueChanges.subscribe(() => {
      const v = this.form.getRawValue();
      this.onChange({
        givenName: v.givenName,
        christName: v.christName || null,
        familyName: v.familyName || null,
        middleName: v.middleName || null,
      });
    });
  }

  writeValue(val: FullName | null): void {
    if (!val) {
      this.form.reset({}, { emitEvent: false });
      return;
    }
    this.form.setValue(
      {
        christName: val.christName ?? '',
        familyName: val.familyName ?? '',
        middleName: val.middleName ?? '',
        givenName: val.givenName ?? '',
      },
      { emitEvent: false },
    );
  }

  registerOnChange(fn: (val: FullName) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(disabled: boolean): void {
    disabled ? this.form.disable() : this.form.enable();
  }
}
