import { Component, forwardRef, ChangeDetectionStrategy, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ControlValueAccessor,
  NG_VALUE_ACCESSOR,
  NG_VALIDATORS,
  Validator,
  AbstractControl,
  ValidationErrors,
  Validators,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
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
      @for (ctrl of saints(); track $index; let i = $index) {
        <div class="patron-row" [formGroup]="ctrl">
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
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PatronSaintInput implements ControlValueAccessor, Validator {
  private fb = inject(FormBuilder);

  saints = signal<FormGroup[]>([]);

  private onChange: (val: PatronSaint[]) => void = () => { };
  private onTouched: () => void = () => { };

  private makeGroup(s?: PatronSaint) {
    return this.fb.group({
      saintName: [s?.saintName ?? '', Validators.required],
      day: [s?.day ?? 1, [Validators.required, Validators.min(1), Validators.max(31)]],
      month: [s?.month ?? 1, [Validators.required, Validators.min(1), Validators.max(12)]],
    });
  }

  add(): void {
    this.saints.update(list => [...list, this.makeGroup()]);
    this.emitValue();
    this.onTouched();
  }

  remove(i: number): void {
    this.saints.update(list => list.filter((_, idx) => idx !== i));
    this.emitValue();
    this.onTouched();
  }

  private emitValue() {
    const val = this.saints().map(c => c.getRawValue() as PatronSaint);
    this.onChange(val);
  }

  // ControlValueAccessor
  writeValue(val: PatronSaint[] | null): void {
    if (val && val.length > 0) {
      this.saints.set(val.map(s => this.makeGroup(s)));
    } 
    this.emitValue();
  }

  registerOnChange(fn: (val: PatronSaint[]) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(disabled: boolean): void {
    this.saints().forEach(c => disabled ? c.disable() : c.enable());
  }

  // Validator
  validate(control: AbstractControl): ValidationErrors | null {
    const invalid = this.saints().some(group => group.invalid);
    return invalid ? { invalidSaint: true } : null;
  }
}
