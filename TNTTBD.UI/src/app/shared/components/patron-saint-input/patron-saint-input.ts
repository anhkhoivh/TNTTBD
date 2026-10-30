import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  DestroyRef,
  forwardRef,
  inject,
} from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import {
  AbstractControl,
  ControlValueAccessor,
  FormBuilder,
  FormGroup,
  NG_VALIDATORS,
  NG_VALUE_ACCESSOR,
  ReactiveFormsModule,
  ValidationErrors,
  Validator,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { TranslatePipe } from '../../../core/i18n/translate.pipe';
import { PatronSaint } from '../../models';

@Component({
  selector: 'app-patron-saint-input',
  standalone: true,
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule, TranslatePipe],
  providers: [
    { provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => PatronSaintInput), multi: true },
    { provide: NG_VALIDATORS, useExisting: forwardRef(() => PatronSaintInput), multi: true },
  ],
  templateUrl: './patron-saint-input.html',
  styleUrl: './patron-saint-input.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PatronSaintInput implements ControlValueAccessor, Validator {
  private fb = inject(FormBuilder);
  private cdr = inject(ChangeDetectorRef);

  protected saints = this.fb.array<FormGroup>([]);
  protected disabled = false;

  private onChange: (value: PatronSaint[]) => void = () => {};
  private onTouched: () => void = () => {};

  constructor() {
    this.saints.valueChanges
      .pipe(takeUntilDestroyed(inject(DestroyRef)))
      .subscribe(() => this.onChange(this.value()));
  }

  protected add(): void {
    this.saints.push(this.makeGroup());
    this.onTouched();
  }

  protected remove(index: number): void {
    this.saints.removeAt(index);
    this.onTouched();
  }

  writeValue(value: PatronSaint[] | null): void {
    this.saints.clear({ emitEvent: false });
    for (const saint of value ?? []) {
      this.saints.push(this.makeGroup(saint), { emitEvent: false });
    }
    if (this.disabled) this.saints.disable({ emitEvent: false });
    this.cdr.markForCheck();
  }

  registerOnChange(fn: (value: PatronSaint[]) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(disabled: boolean): void {
    this.disabled = disabled;
    if (disabled) this.saints.disable({ emitEvent: false });
    else this.saints.enable({ emitEvent: false });
    this.cdr.markForCheck();
  }

  validate(_control: AbstractControl): ValidationErrors | null {
    return this.saints.invalid ? { invalidSaint: true } : null;
  }

  private value(): PatronSaint[] {
    return this.saints.getRawValue() as PatronSaint[];
  }

  private makeGroup(saint?: PatronSaint): FormGroup {
    return this.fb.group({
      saintName: [saint?.saintName ?? '', Validators.required],
      day: [saint?.day ?? 1, [Validators.required, Validators.min(1), Validators.max(31)]],
      month: [saint?.month ?? 1, [Validators.required, Validators.min(1), Validators.max(12)]],
    });
  }
}
