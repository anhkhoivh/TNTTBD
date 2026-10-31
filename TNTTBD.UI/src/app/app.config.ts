import {
  ApplicationConfig,
  LOCALE_ID,
  inject,
  provideAppInitializer,
  provideBrowserGlobalErrorListeners,
} from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import {
  provideHttpClient,
  withFetch,
  withInterceptors,
} from '@angular/common/http';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideNativeDateAdapter } from '@angular/material/core';
import { registerLocaleData } from '@angular/common';
import localeVi from '@angular/common/locales/vi';
import localeEn from '@angular/common/locales/en';

import { routes } from './app.routes';
import { authInterceptor } from './core/auth/auth.interceptor';
import { errorLoadingInterceptor } from './core/error-loading.interceptor';
import { TranslationService } from './core/i18n/translation.service';

registerLocaleData(localeVi);
registerLocaleData(localeEn);

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes, withComponentInputBinding()),
    provideHttpClient(
      withFetch(),
      withInterceptors([errorLoadingInterceptor, authInterceptor]),
    ),
    provideAnimationsAsync(),
    provideNativeDateAdapter(),
    { provide: LOCALE_ID, useValue: 'vi-VN' },
    provideAppInitializer(async () => {
      const t = inject(TranslationService);
      await t.init();
    }),
  ],
};

