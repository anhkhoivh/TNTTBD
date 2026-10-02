import { Routes } from '@angular/router';
import { authGuard, roleGuard } from './core/auth/auth.guard';
import { UserRole } from './shared/enums';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./features/landing/landing').then((m) => m.Landing),
  },
  {
    path: 'auth/login',
    loadComponent: () => import('./features/auth/login').then((m) => m.Login),
  },
  {
    path: 'auth/callback',
    loadComponent: () => import('./features/auth/callback').then((m) => m.AuthCallback),
  },
  {
    path: 'register-youth',
    loadComponent: () =>
      import('./features/registration/register').then((m) => m.RegisterPage),
  },
  {
    path: 'app',
    canActivate: [authGuard],
    loadComponent: () => import('./core/layout/shell').then((m) => m.Shell),
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'dashboard' },
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./features/dashboard/dashboard').then((m) => m.Dashboard),
      },
      {
        path: 'youth',
        canActivate: [roleGuard],
        data: { roles: [UserRole.Admin, UserRole.Catechist] },
        loadComponent: () =>
          import('./features/youth/youth-list/youth-list').then((m) => m.YouthList),
      },
      {
        path: 'youth/edit',
        canActivate: [roleGuard],
        data: { roles: [UserRole.Admin, UserRole.Catechist] },
        loadComponent: () =>
          import('./features/youth/youth-form/youth-form').then((m) => m.YouthForm),
      },
      {
        path: 'youth/detail',
        canActivate: [roleGuard],
        data: {
          roles: [
            UserRole.Admin,
            UserRole.Catechist,
            UserRole.EucharisticYouthContact,
            UserRole.EucharisticYouth,
          ],
        },
        loadComponent: () =>
          import('./features/youth/youth-detail/youth-detail').then((m) => m.YouthDetail),
      },
      {
        path: 'youth/report',
        canActivate: [roleGuard],
        data: {
          roles: [
            UserRole.Admin,
            UserRole.Catechist,
            UserRole.EucharisticYouthContact,
            UserRole.EucharisticYouth,
          ],
        },
        loadComponent: () =>
          import('./features/youth/youth-report/youth-report').then((m) => m.YouthReportPage),
      },
      {
        path: 'catechists',
        canActivate: [roleGuard],
        data: { roles: [UserRole.Admin, UserRole.Catechist] },
        loadComponent: () =>
          import('./features/catechist/catechist-list/catechist-list').then((m) => m.CatechistList),
      },
      {
        path: 'catechists/detail',
        canActivate: [roleGuard],
        data: { roles: [UserRole.Admin, UserRole.Catechist] },
        loadComponent: () =>
          import('./features/catechist/catechist-detail/catechist-detail').then((m) => m.CatechistDetail),
      },
      {
        path: 'catechists/edit',
        canActivate: [roleGuard],
        data: { roles: [UserRole.Admin, UserRole.Catechist] },
        loadComponent: () =>
          import('./features/catechist/catechist-form/catechist-form').then((m) => m.CatechistForm),
      },
      {
        path: 'contacts',
        canActivate: [roleGuard],
        data: { roles: [UserRole.Admin, UserRole.Catechist] },
        loadComponent: () =>
          import('./features/contacts/contact-list/contact-list').then((m) => m.ContactList),
      },
      {
        path: 'contacts/edit',
        canActivate: [roleGuard],
        data: {
          roles: [
            UserRole.Admin,
            UserRole.Catechist,
            UserRole.EucharisticYouthContact,
            UserRole.EucharisticYouth,
          ],
        },
        loadComponent: () =>
          import('./features/contacts/contact-form/contact-form').then((m) => m.ContactForm),
      },
      {
        path: 'contacts/detail',
        canActivate: [roleGuard],
        data: {
          roles: [
            UserRole.Admin,
            UserRole.Catechist,
            UserRole.EucharisticYouthContact,
            UserRole.EucharisticYouth,
          ],
        },
        loadComponent: () =>
          import('./features/contacts/contact-detail/contact-detail').then((m) => m.ContactDetail),
      },
      {
        path: 'classes',
        loadComponent: () =>
          import('./features/classes/class-list/class-list').then((m) => m.ClassList),
      },
      {
        path: 'classes/detail',
        loadComponent: () =>
          import('./features/classes/class-detail/class-detail').then((m) => m.ClassDetail),
      },
      {
        path: 'classes/migrate',
        canActivate: [roleGuard],
        data: { roles: [UserRole.Admin, UserRole.Catechist] },
        loadComponent: () =>
          import('./features/classes/class-migration/class-migration').then((m) => m.ClassMigration),
      },
      {
        path: 'classes/attendance',
        canActivate: [roleGuard],
        data: { roles: [UserRole.Admin, UserRole.Catechist] },
        loadComponent: () =>
          import('./features/classes/class-attendance/class-attendance').then((m) => m.ClassAttendance),
      },
      {
        path: 'classes/exam-results',
        canActivate: [roleGuard],
        data: { roles: [UserRole.Admin, UserRole.Catechist] },
        loadComponent: () =>
          import('./features/classes/class-exam-results/class-exam-results').then((m) => m.ClassExamResults),
      },
      {
        path: 'classes/report',
        canActivate: [roleGuard],
        data: { roles: [UserRole.Admin, UserRole.Catechist] },
        loadComponent: () =>
          import('./features/classes/class-report/class-report').then((m) => m.ClassReportPage),
      },
      {
        path: 'admin/users',
        canActivate: [roleGuard],
        data: { roles: [UserRole.Admin] },
        loadComponent: () =>
          import('./features/admin/admin').then((m) => m.AdminPage),
      },
    ],
  },
  {
    path: 'not-found',
    loadComponent: () =>
      import('./features/not-found/not-found').then((m) => m.NotFound),
  },
  { path: '**', redirectTo: 'not-found' },
];
