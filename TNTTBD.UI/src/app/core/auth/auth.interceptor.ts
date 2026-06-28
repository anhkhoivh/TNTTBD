import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { BehaviorSubject, catchError, filter, switchMap, take, throwError } from 'rxjs';
import { AuthService } from './auth.service';

let isRefreshing = false;
const refreshSubject = new BehaviorSubject<string | null>(null);

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AuthService);
  const token = auth.getToken();

  const authedReq = token
    ? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } })
    : req;

  return next(authedReq).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status !== 401 || req.url.includes('/auth/refresh') || req.url.includes('/auth/login')) {
        return throwError(() => error);
      }

      if (!isRefreshing) {
        isRefreshing = true;
        refreshSubject.next(null);

        return auth.refreshAccessToken().pipe(
          switchMap((res) => {
            isRefreshing = false;
            refreshSubject.next(res.token);
            return next(req.clone({ setHeaders: { Authorization: `Bearer ${res.token}` } }));
          }),
          catchError((refreshError) => {
            isRefreshing = false;
            auth.logout();
            return throwError(() => refreshError);
          }),
        );
      }

      // Queue requests while refresh is in-flight
      return refreshSubject.pipe(
        filter((t) => t !== null),
        take(1),
        switchMap((newToken) =>
          next(req.clone({ setHeaders: { Authorization: `Bearer ${newToken}` } })),
        ),
      );
    }),
  );
};
