import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, finalize, throwError } from 'rxjs';
import { NotificationService } from './services/notification.service';
import { LoadingService } from './services/loading.service';
import { TranslationService } from './i18n/translation.service';

export const errorLoadingInterceptor: HttpInterceptorFn = (req, next) => {
  const notify = inject(NotificationService);
  const loading = inject(LoadingService);
  const t = inject(TranslationService);

  const isAsset = req.url.includes('/assets/');
  if (!isAsset) loading.begin();

  return next(req).pipe(
    catchError((err: HttpErrorResponse) => {
      if (!isAsset) {
        const status = err.status;
        let key = 'error.generic';
        if (status === 0) key = 'error.network';
        else if (status === 401) key = 'error.unauthorized';
        else if (status === 403) key = 'error.forbidden';
        else if (status === 404) key = 'error.notFound';
        else if (status >= 500) key = 'error.server';
        notify.error(t.t(key));
      }
      return throwError(() => err);
    }),
    finalize(() => {
      if (!isAsset) loading.end();
    }),
  );
};
