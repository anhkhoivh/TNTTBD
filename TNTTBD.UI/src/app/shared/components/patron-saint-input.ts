import { Component, forwardRef, inject , ChangeDetectionStrategy} from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ControlValueAccessor,
  FormBuilder,
  FormArray,
  ReactiveFormsModule,
  NG_VALUE_ACCESSOR,
  Validators,
  Validator,
  AbstractControl,
  ValidationErrors,
  NG_VALIDATORS,
} from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { PatronSaint } from '../models';

@Component({
  selector: 'app-patron-saint-input',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    TranslatePipe,
  ],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => PatronSaintInput),
      multi: true,
    },
    {
    provide: NG_VALIDATORS,
      useExisting: forwardRef(() => PatronSaintInput),
      multi: true,
    },
  ],
  template: `
    <div class="patron-list">
      <label class="section-label">{{ 'patron.title' | t }}</label>
      @for (ctrl of saints.controls; track $index; let i = $index) {
        <div class="patron-row" [formGroup]="asGroup(ctrl)">
          <mat-form-field appearance="outline">
            <mat-label>{{ 'patron.name' | t }}</mat-label>
            <input matInput formControlName="saintName" required />
          </mat-form-field>
          <mat-form-field appearance="outline" class="narrow">
            <mat-label>{{ 'patron.day' | t }}</mat-label>
            <input matInput type="number" formControlName="day" min="1" max="31" />
          </mat-form-field>
          <mat-form-field appearance="outline" class="narrow">
            <mat-label>{{ 'patron.month' | t }}</mat-label>
            <input matInput type="number" formControlName="month" min="1" max="12" />
          </mat-form-field>
          <button mat-icon-button type="button" (click)="remove(i)" aria-label="Remove">
            <mat-icon>close</mat-icon>
          </button>
        </div>
      }
      <button mat-stroked-button type="button" (click)="add()">
        <mat-icon>add</mat-icon> {{ 'patron.add' | t }}
      </button>
    </div>
  `,
  styles: [
    `
      .patron-list {
        display: flex;
        flex-direction: column;
        gap: 0.5rem;
      }
      .section-label {
        font: var(--mat-sys-label-large);
        color: var(--mat-sys-on-surface-variant);
      }
      .patron-row {
        display: flex;
        align-items: center;
        gap: 0.5rem;
      }
      .narrow {
        max-width: 80px;
      }
    `,
  ],

  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PatronSaintInput implements ControlValueAccessor, Validator {
  private fb = inject(FormBuilder);

  saints = new FormArray<ReturnType<typeof this.makeGroup>>([]);

  private onChange: (val: PatronSaint[]) => void = () => {};
  private onTouched: () => void = () => {};

  constructor() {
    this.saints.valueChanges.subscribe(() => {
      this.onChange(this.getValue());
    });
  }

  protected asGroup(ctrl: unknown) {
    return ctrl as ReturnType<typeof this.makeGroup>;
  }

  private makeGroup(s?: PatronSaint) {
    return this.fb.nonNullable.group({
      saintName: [s?.saintName ?? '', Validators.required],
      day: [s?.day ?? 1, [Validators.required, Validators.min(1), Validators.max(31)]],
      month: [s?.month ?? 1, [Validators.required, Validators.min(1), Validators.max(12)]],
    });
  }

  private getValue(): PatronSaint[] {
    return this.saints.controls.map((c) => c.getRawValue());
  }

  add(): void {
    this.saints.push(this.makeGroup());
    this.onTouched();
  }

  remove(i: number): void {
    this.saints.removeAt(i);
    this.onTouched();
  }

  writeValue(val: PatronSaint[] | null): void {
    this.saints.clear({ emitEvent: false });
    if (val) {
      for (const s of val) {
        this.saints.push(this.makeGroup(s), { emitEvent: false });
      }
    }
  }

  registerOnChange(fn: (val: PatronSaint[]) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(disabled: boolean): void {
    disabled ? this.saints.disable() : this.saints.enable();
  }
  validate(control: AbstractControl): ValidationErrors | null {
    if (this.saints.length === 0) {
      return null;
    }

    const invalid = this.saints.controls.some(group => group.invalid);
    if (invalid) {
      return { invalidSaint: true };
    }

    return null;
  }
}
