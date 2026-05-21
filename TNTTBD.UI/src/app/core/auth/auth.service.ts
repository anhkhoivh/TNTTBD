import { Injectable, computed, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { User } from '../../shared/models';
import { UserRole } from '../../shared/enums';

const TOKEN_KEY = 'tnttbd.token';
const USER_KEY = 'tnttbd.user';

export interface LoginResponse {
  token: string;
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

  login(email: string, password: string): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(`${environment.apiBaseUrl}/auth/login`, { email, password })
      .pipe(tap((res) => this.applyLogin(res)));
  }

  loginWithSSO(provider: 'google' | 'microsoft'): Observable<LoginResponse> {
    // Mock-friendly: backend accepts provider + a fake code
    return this.http
      .post<LoginResponse>(`${environment.apiBaseUrl}/auth/sso-callback`, {
        provider,
        code: 'mock-code',
      })
      .pipe(tap((res) => this.applyLogin(res)));
  }

  logout(): void {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(USER_KEY);
    this._currentUser.set(null);
    this.router.navigate(['/']);
  }

  hasRole(roles: UserRole[]): boolean {
    const role = this.userRole();
    return role !== null && roles.includes(role);
  }

  private applyLogin(res: LoginResponse): void {
    localStorage.setItem(TOKEN_KEY, res.token);
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
