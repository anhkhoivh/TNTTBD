import { Injectable, computed, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, firstValueFrom, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { User } from '../../shared/models';
import { UserRole } from '../../shared/enums';

const TOKEN_KEY = 'tnttbd.token';
const REFRESH_TOKEN_KEY = 'tnttbd.refreshToken';
const USER_KEY = 'tnttbd.user';
const SSO_STATE_KEY = 'tnttbd.sso_state';
const SSO_PROVIDER_KEY = 'tnttbd.sso_provider';

export interface LoginResponse {
  token: string;
  refreshToken: string;
  user: User;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);

  private _currentUser = signal<User | null>(this.loadStoredUser());
  readonly currentUser = this._currentUser.asReadonly();

  readonly isAuthenticated = computed(() => this._currentUser() !== null);
  readonly userRole = computed(() => this._currentUser()?.userRole ?? null);

  getToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  isAdminOrCatechist(): boolean {
    const role = this.userRole();
    return role != null && [UserRole.Admin, UserRole.Catechist].includes(role);
  }

  getRefreshToken(): string | null {
    return localStorage.getItem(REFRESH_TOKEN_KEY);
  }

  getStoredUserId(): string | null {
    const user = this._currentUser();
    return user?.id ?? null;
  }

  refreshAccessToken(): Observable<LoginResponse> {
    const refreshToken = this.getRefreshToken();
    const userId = this.getStoredUserId();
    return this.http
      .post<LoginResponse>(`${environment.apiBaseUrl}/auth/refresh`, { refreshToken, userId })
      .pipe(tap((res) => this.applyLogin(res)));
  }

  login(identifier: string, password: string): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(`${environment.apiBaseUrl}/auth/login`, { identifier, password })
      .pipe(tap((res) => this.applyLogin(res)));
  }

  async loginWithSSO(provider: 'google' | 'microsoft'): Promise<void> {
    const state = crypto.randomUUID();
    sessionStorage.setItem(SSO_STATE_KEY, state);
    sessionStorage.setItem(SSO_PROVIDER_KEY, provider);

    try {
      const res = await firstValueFrom(
        this.http.get<{ url: string }>(`${environment.apiBaseUrl}/auth/sso/url`, {
          params: {
            provider,
            redirectUri: environment.ssoRedirectUri,
            state,
          },
        }),
      );
      window.location.href = res.url;
    } catch {
      sessionStorage.removeItem(SSO_STATE_KEY);
      sessionStorage.removeItem(SSO_PROVIDER_KEY);
    }
  }

  exchangeSsoCode(provider: string, code: string, redirectUri: string): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(`${environment.apiBaseUrl}/auth/sso-callback`, {
        provider,
        code,
        redirectUri,
      })
      .pipe(tap((res) => this.applyLogin(res)));
  }

  getSsoState(): { state: string | null; provider: string | null } {
    return {
      state: sessionStorage.getItem(SSO_STATE_KEY),
      provider: sessionStorage.getItem(SSO_PROVIDER_KEY),
    };
  }

  clearSsoState(): void {
    sessionStorage.removeItem(SSO_STATE_KEY);
    sessionStorage.removeItem(SSO_PROVIDER_KEY);
  }

  async logout(): Promise<void> {
    // Fire-and-forget: revoke refresh tokens on server
    firstValueFrom(this.http.post(`${environment.apiBaseUrl}/auth/logout`, {})).catch(() => undefined);

    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(REFRESH_TOKEN_KEY);
    localStorage.removeItem(USER_KEY);
    this._currentUser.set(null);
    this.router.navigate(['/']);
  }

  hasRole(roles: UserRole[]): boolean {
    const role = this.userRole();
    return role !== null && roles.includes(role);
  }

  async changePassword(currentPassword: string, newPassword: string): Promise<void> {
    await firstValueFrom(
      this.http.post(`${environment.apiBaseUrl}/auth/change-password`, { currentPassword, newPassword }),
    );
  }

  private applyLogin(res: LoginResponse): void {
    localStorage.setItem(TOKEN_KEY, res.token);
    localStorage.setItem(REFRESH_TOKEN_KEY, res.refreshToken);
    localStorage.setItem(USER_KEY, JSON.stringify(res.user));
    this._currentUser.set(res.user);
  }

  private loadStoredUser(): User | null {
    const raw = localStorage.getItem(USER_KEY);
    if (!raw) return null;
    try {
      return JSON.parse(raw) as User;
    } catch {
      return null;
    }
  }
}
