import { Component, OnInit, inject, signal, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../../core/auth/auth.service';
import { environment } from '../../../environments/environment';
import { TranslatePipe } from '../../core/i18n/translate.pipe';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-auth-callback',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatButtonModule,
    TranslatePipe,
  ],
  template: `
    <div class="callback-container">
      <mat-card>
        @if (loading()) {
          <mat-card-content class="center">
            <mat-spinner diameter="40"></mat-spinner>
            <p>{{ 'auth.signingIn' | t }}</p>
          </mat-card-content>
        } @else if (error()) {
          <mat-card-content class="center">
            <p class="error">{{ error() }}</p>
            <button mat-flat-button color="primary" (click)="goToLogin()">
              {{ 'auth.backToLogin' | t }}
            </button>
          </mat-card-content>
        }
      </mat-card>
    </div>
  `,
  styles: `
    .callback-container {
      display: flex;
      justify-content: center;
      align-items: center;
      min-height: 100vh;
    }
    .center {
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 16px;
      padding: 32px;
    }
    .error {
      color: var(--mat-sys-error);
    }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AuthCallback implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private auth = inject(AuthService);

  protected loading = signal(true);
  protected error = signal<string | null>(null);

  ngOnInit(): void {
    void this.handleCallback();
  }

  private async handleCallback(): Promise<void> {
    const params = this.route.snapshot.queryParams;
    const code = params['code'];
    const state = params['state'];
    const errorParam = params['error'];

    if (errorParam) {
      this.loading.set(false);
      this.error.set(`SSO error: ${errorParam}`);
      this.auth.clearSsoState();
      return;
    }

    if (!code || !state) {
      this.loading.set(false);
      this.error.set('Missing authorization code or state parameter.');
      this.auth.clearSsoState();
      return;
    }

    // Validate state (CSRF protection)
    const { state: savedState, provider } = this.auth.getSsoState();
    if (state !== savedState || !provider) {
      this.loading.set(false);
      this.error.set('Invalid state parameter. Please try again.');
      this.auth.clearSsoState();
      return;
    }

    this.auth.clearSsoState();

    // Exchange code for tokens
    try {
      await firstValueFrom(this.auth.exchangeSsoCode(provider, code, environment.ssoRedirectUri));
      this.loading.set(false);
      this.router.navigate(['/app/dashboard']);
    } catch (err: any) {
      this.loading.set(false);
      const message = err?.error?.message || err?.error?.detail || 'Authentication failed. Contact your administrator.';
      this.error.set(message);
    }
  }

  protected goToLogin(): void {
    this.router.navigate(['/auth/login']);
  }
}
