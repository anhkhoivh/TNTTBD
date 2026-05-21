import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthService } from './auth.service';
import { UserRole } from '../../shared/enums';

export const authGuard: CanActivateFn = () => {
  const auth = inject(AuthService);
  const router = inject(Router);
  if (auth.isAuthenticated()) return true;
  router.navigate(['/auth/login']);
  return false;
};

export const roleGuard: CanActivateFn = (route) => {
  const auth = inject(AuthService);
  const router = inject(Router);
  const required = (route.data?.['roles'] as UserRole[] | undefined) ?? [];
  if (required.length === 0) return true;
  if (auth.hasRole(required)) return true;
  router.navigate(['/app/dashboard']);
  return false;
};
