import { Component, inject, signal, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { AuthService } from '../../core/auth/auth.service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule,
    MatProgressSpinnerModule,
    TranslatePipe,
  ],
  templateUrl: './login.html',
  styleUrl: './login.scss',

  changeDetection: ChangeDetectionStrategy.OnPush
})
export class Login {
  private auth = inject(AuthService);
  private router = inject(Router);

  private fb = inject(FormBuilder);
  protected form = this.fb.nonNullable.group({
    identifier: ['', [Validators.required]],
    password: ['', [Validators.required]],
  });
  protected loading = signal(false);
  protected error = signal<string | null>(null);

  protected async submit(): Promise<void> {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.loading.set(true);
    this.error.set(null);
    const { identifier, password } = this.form.getRawValue();
    try {
      await firstValueFrom(this.auth.login(identifier, password));
      this.router.navigate(['/app/dashboard']);
    } catch {
      this.error.set('auth.invalidCredentials');
    } finally {
      this.loading.set(false);
    }
  }

  protected loginSSO(provider: 'google' | 'microsoft'): void {
    this.loading.set(true);
    this.error.set(null);
    this.auth.loginWithSSO(provider);
  }
}
