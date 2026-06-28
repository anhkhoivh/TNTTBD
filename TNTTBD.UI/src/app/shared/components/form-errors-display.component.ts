import { Component, Input } from '@angular/core';
import { AbstractControl } from '@angular/forms';
import { TranslatePipe } from '../../core/i18n/translate.pipe';

@Component({
  selector: 'app-form-errors-display',
  standalone: true,
  template: `
    @if (control && control.invalid && (control.dirty || control.touched)) {
      <div class="form-error">
        @if (control.errors?.['required']) {
          <div>{{ fieldName }} {{ 'form.error.required' | t }}</div>
        }
        @if (control.errors?.['min']) {
          <div>{{ 'form.error.min' | t }} {{ control.errors!['min'].min }}</div>
        }
        @if (control.errors?.['max']) {
          <div>{{ 'form.error.max' | t }} {{ control.errors!['max'].max }}</div>
        }
        @if (control.errors?.['scoreRange']) {
          <div>{{ 'form.error.scoreRange' | t }} {{ control.errors!['scoreRange'].min }} {{ 'form.error.and' | t }} {{ control.errors!['scoreRange'].max }}</div>
        }
        @if (control.errors?.['scoreStep']) {
          <div>{{ 'form.error.scoreStep' | t }}</div>
        }
        @if (control.errors?.['isoDate']) {
          <div>{{ 'form.error.isoDate' | t }}</div>
        }
        @if (control.errors?.['maxlength']) {
          <div>{{ 'form.error.maxlength' | t }} {{ control.errors!['maxlength'].requiredLength }}</div>
        }
        <!-- Add more error messages as needed -->
      </div>
    }
  `,
  styles: [
    `.form-error { color: #d32f2f; font-size: 0.9em; margin-top: 2px; }`
  ],
  imports: [
    TranslatePipe,
  ],
})
export class FormErrorsDisplayComponent {
  @Input() control!: AbstractControl | null;
  @Input() fieldName = '';
}
