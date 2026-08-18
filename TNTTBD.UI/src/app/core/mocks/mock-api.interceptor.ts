import { HttpInterceptorFn, HttpResponse } from '@angular/common/http';
import { Observable, of, delay } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  mockUsers,
  mockYouth,
  mockCatechists,
  mockContacts,
  mockYouthContacts,
  mockYears,
  mockClasses,
  mockClassYouth,
  mockSessions,
  mockAttendances,
  mockExams,
} from './mock-data';
import { UserRole } from '../../shared/enums';

function ok<T>(body: T): Observable<HttpResponse<T>> {
  return of(new HttpResponse<T>({ status: 200, body })).pipe(delay(150));
}

function notFound(): Observable<HttpResponse<unknown>> {
  return of(new HttpResponse({ status: 404, body: { error: 'Not found' } })).pipe(delay(100));
}

function paginate<T>(items: T[]) {
  return { items, total: items.length, page: 1, pageSize: 50 };
}

function makeToken(role: UserRole): string {
  const payload = btoa(JSON.stringify({ role, exp: Date.now() / 1000 + 3600 }));
  return `mock.${payload}.signature`;
}

function loginByEmail(email: string) {
  const user = mockUsers.find((u) => u.email.toLowerCase() === email.toLowerCase()) ?? mockUsers[0];
  return { token: makeToken(user.userRole), refreshToken: 'mock-refresh', user };
}

/**
 * Mock backend API.
 * Convention: NO IDs in URL paths. IDs go in query (GET) or body (POST/PATCH/DELETE).
 * Base path: environment.apiBaseUrl (e.g. /api)
 */
export const mockApiInterceptor: HttpInterceptorFn = (req, next) => {
  if (!environment.useMocks) return next(req);

  const base = environment.apiBaseUrl.replace(/\/+$/, '');
  if (!req.url.startsWith(base)) return next(req);

  const path = req.url.substring(base.length).split('?')[0];
  const url = new URL(req.urlWithParams, 'http://localhost');
  const qs = url.searchParams;
  const method = req.method.toUpperCase();
  const body = (req.body as Record<string, unknown> | null) ?? null;

  // ===== Auth =====
  if (path === '/auth/login' && method === 'POST') {
    return ok(loginByEmail((body?.['email'] as string) ?? mockUsers[0].email));
  }
  if (path === '/auth/sso-callback' && method === 'POST') {
    const provider = (body?.['provider'] as string) ?? 'google';
    const role = provider === 'microsoft' ? UserRole.Catechist : UserRole.EucharisticYouthContact;
    const user = mockUsers.find((u) => u.userRole === role) ?? mockUsers[0];
    return ok({ token: makeToken(user.userRole), refreshToken: 'mock-refresh', user });
  }
  if (path.startsWith('/auth/sso/url') && method === 'GET') {
    return ok({ url: '/auth/callback?code=mock-code&state=' + (qs.get('state') ?? 'mock') });
  }
  if (path === '/auth/me' && method === 'GET') {
    return ok(mockUsers[0]);
  }
  if (path === '/auth/logout' && method === 'POST') {
    return ok({ ok: true });
  }

  // ===== Users =====
  if (path === '/users' && method === 'GET') {
    return ok(paginate(mockUsers));
  }
  if (path === '/users/detail' && method === 'GET') {
    const item = mockUsers.find((u) => u.id === qs.get('id'));
    return item ? ok(item) : notFound();
  }

  // ===== Youths =====
  if (path === '/youths' && method === 'GET') {
    const id = qs.get('id');
    if (id) {
      const item = mockYouth.find((y) => y.id === id);
      return item ? ok(item) : notFound();
    }
    const search = (qs.get('search') ?? '').toLowerCase();
    const list = search
      ? mockYouth.filter((y) =>
          [y.code, y.fullName.givenName, y.fullName.familyName, y.fullName.christName]
            .filter(Boolean)
            .some((v) => (v as string).toLowerCase().includes(search)),
        )
      : mockYouth;
    return ok(paginate(list));
  }
  if (path === '/youths/detail' && method === 'GET') {
    const item = mockYouth.find((y) => y.id === qs.get('id'));
    return item ? ok(item) : notFound();
  }
  if (path === '/youths/enrollments' && method === 'GET') {
    const youthId = qs.get('youthId');
    const items = youthId
      ? mockClassYouth.filter((c) => c.eucharisticYouthId === youthId)
      : mockClassYouth;
    return ok(items);
  }
  if (path === '/youths/contacts' && method === 'GET') {
    const youthId = qs.get('youthId');
    const items = youthId
      ? mockYouthContacts.filter((c) => c.eucharisticYouthId === youthId)
      : mockYouthContacts;
    return ok(items);
  }

  // ===== Catechists =====
  if (path === '/catechists' && method === 'GET') {
    const id = qs.get('id');
    if (id) {
      const item = mockCatechists.find((c) => c.id === id);
      return item ? ok(item) : notFound();
    }
    return ok(paginate(mockCatechists));
  }
  if (path === '/catechists' && method === 'POST') {
    return ok({ id: crypto.randomUUID(), ...body });
  }
  if (path === '/catechists' && method === 'PATCH') {
    return ok(body);
  }
  if (path === '/catechists/detail' && method === 'GET') {
    const item = mockCatechists.find((c) => c.id === qs.get('id'));
    return item ? ok(item) : notFound();
  }

  // ===== Contacts =====
  if (path === '/contacts' && method === 'GET') {
    return ok(paginate(mockContacts));
  }

  // ===== Catechism years =====
  if (path === '/catechism-years' && method === 'GET') {
    return ok(paginate(mockYears));
  }

  // ===== Classes =====
  if (path === '/classes' && method === 'GET') {
    const yearId = qs.get('yearId');
    const items = yearId ? mockClasses.filter((c) => c.catechismYearId === yearId) : mockClasses;
    return ok(paginate(items));
  }
  if (path === '/classes/detail' && method === 'GET') {
    const item = mockClasses.find((c) => c.id === qs.get('id'));
    return item ? ok(item) : notFound();
  }

  // ===== Sessions =====
  if (path === '/sessions' && method === 'GET') {
    const classId = qs.get('classId');
    const items = classId
      ? mockSessions.filter((s) => s.catechismClassId === classId)
      : mockSessions;
    return ok(paginate(items));
  }
  if (path === '/sessions/detail' && method === 'GET') {
    const item = mockSessions.find((s) => s.id === qs.get('id'));
    return item ? ok(item) : notFound();
  }
  if (path === '/sessions/attendances' && method === 'GET') {
    return ok(paginate(mockAttendances));
  }
  if (path === '/sessions/attendance-sheet' && method === 'GET') {
    return ok(mockAttendances);
  }

  // ===== Class-Youth (enrollments) =====
  if (path === '/class-youth' && method === 'GET') {
    const classId = qs.get('classId');
    const items = classId
      ? mockClassYouth.filter((c) => c.catechismClassId === classId)
      : mockClassYouth;
    return ok(paginate(items));
  }
  if (path === '/class-youth' && method === 'POST') {
    return ok({ id: crypto.randomUUID(), ...body });
  }

  // ===== Exams =====
  if (path === '/exams' && method === 'GET') {
    const classId = qs.get('classId');
    const items = classId ? mockExams.filter((e) => e.catechismClassId === classId) : mockExams;
    return ok(paginate(items));
  }
  if (path === '/exams/detail' && method === 'GET') {
    const item = mockExams.find((e) => e.id === qs.get('id'));
    return item ? ok(item) : notFound();
  }
  if (path === '/exams/my-results' && method === 'GET') {
    const youthId = qs.get('youthId');
    const enrollmentIds = mockClassYouth
      .filter((e) => e.eucharisticYouthId === youthId)
      .map((e) => e.id);
    const results = mockExams.flatMap((ex) =>
      (ex.examResults ?? [])
        .filter((r) => enrollmentIds.includes(r.catechismClassEucharisticYouthId))
        .map((r) => ({ exam: ex, result: r })),
    );
    return ok(paginate(results));
  }

  // ===== Write fallthrough =====
  if (method === 'POST' || method === 'PATCH' || method === 'DELETE') {
    return ok({ ok: true, mock: true });
  }

  return next(req);
};
