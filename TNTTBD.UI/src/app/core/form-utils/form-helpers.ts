import { FormGroup } from '@angular/forms';

export function markFormGroupTouched(form: FormGroup) {
  Object.values(form.controls).forEach(control => {
    control.markAsTouched();
    if ((control as any).controls) {
      markFormGroupTouched(control as FormGroup);
    }
  });
}

export function extractFormErrors(form: FormGroup): Record<string, any> {
  const errors: Record<string, any> = {};
  Object.keys(form.controls).forEach(key => {
    const control = form.get(key);
    if (control && control.errors) {
      errors[key] = control.errors;
    }
    if (control instanceof FormGroup) {
      const childErrors = extractFormErrors(control);
      if (Object.keys(childErrors).length) errors[key] = childErrors;
    }
  });
  return errors;
}

export function patchFormValue<T>(form: FormGroup, data: Partial<T>) {
  form.patchValue(data);
}

export function makeContact(type: string, data: any = {}): any {
  return { relationType: type, givenName: '', phone: '', email: '', ...data };
}
