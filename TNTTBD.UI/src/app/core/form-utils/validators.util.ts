import { Validators, ValidatorFn, AbstractControl } from '@angular/forms';

export const REQUIRED = [Validators.required];
export const REQUIRED_DATE = [Validators.required];
export const PATRON_DATE_VALIDATORS = [Validators.required, Validators.min(1), Validators.max(31)];

export function scoreRangeValidator(min = 0, max = 10, step = 0.5): ValidatorFn {
  return (control: AbstractControl) => {
    const value = control.value;
    if (value == null || value === '') return null;
    if (typeof value !== 'number') return { scoreType: true };
    if (value < min || value > max) return { scoreRange: { min, max } };
    if (Math.abs((value * 2) % 1) > 1e-8) return { scoreStep: { step } };
    return null;
  };
}

export function isoDateValidator(): ValidatorFn {
  return (control: AbstractControl) => {
    const value = control.value;
    if (!value) return null;
    return /^\d{4}-\d{2}-\d{2}$/.test(value) ? null : { isoDate: true };
  };
}
