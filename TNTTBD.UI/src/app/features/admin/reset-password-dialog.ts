import { Component, inject, signal, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { TranslatePipe } from '../../core/i18n/translate.pipe';

export interface ResetPasswordData {
  userId: string;
  userName: string;
}

@Component({
  selector: 'app-reset-password-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    TranslatePipe,
  ],
  template: `
    <h2 mat-dialog-title>{{ 'password.reset' | t }}: {{ data.userName }}</h2>
    <form [formGroup]="form" (ngSubmit)="submit()">
      <mat-dialog-content class="form">
        <mat-form-field appearance="outline">
          <mat-label>{{ 'password.new' | t }}</mat-label>
          <input matInput formControlName="newPassword" type="password" required minlength="6" />
        </mat-form-field>
        <mat-form-field appearance="outline">
          <mat-label>{{ 'password.confirm' | t }}</mat-label>
          <input matInput formControlName="confirmPassword" type="password" required />
        </mat-form-field>
        @if (mismatch()) {
          <p class="error">{{ 'password.mismatch' | t }}</p>
        }
      </mat-dialog-content>
      <mat-dialog-actions align="end">
        <button mat-button type="button" (click)="ref.close()">{{ 'common.cancel' | t }}</button>
        <button mat-flat-button color="primary" type="submit">{{ 'common.save' | t }}</button>
      </mat-dialog-actions>
    </form>
  `,
  styles: [
    `.form { display: flex; flex-direction: column; gap: 0.5rem; min-width: 340px; }`,
    `.error { color: var(--mat-sys-error); font: var(--mat-sys-body-small); margin: 0; }`,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ResetPasswordDialog {
  protected ref = inject(MatDialogRef<ResetPasswordDialog>);
  protected data = inject<ResetPasswordData>(MAT_DIALOG_DATA);
  private fb = inject(FormBuilder);

  protected mismatch = signal(false);

  protected form = this.fb.nonNullable.group({
    newPassword: ['', [Validators.required, Validators.minLength(6)]],
    confirmPassword: ['', [Validators.required]],
  });

  protected submit(): void {
    const v = this.form.getRawValue();
    if (v.newPassword !== v.confirmPassword) {
      this.mismatch.set(true);
      return;
    }
    this.mismatch.set(false);
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.ref.close({ userId: this.data.userId, newPassword: v.newPassword });
  }
}
