# Plan: TNTTBD.UI Angular Frontend

Build an Angular 21 SPA with Material 3 for managing a parish Eucharistic Youth Movement. Signal-based state, i18n (vi/en), light/dark theme, role-based access, top navbar layout. Deployed alongside .NET API.

---

## Implementation Status

| Phase | Status |
|------|------|
| Phase 1 — Scaffolding & Core Infrastructure | ✅ Complete |
| Phase 2 — Layout & Navigation Shell | ✅ Complete |
| Phase 3 — Shared Components & Models | ✅ Complete — models, pipes, FullNameInput, PatronSaintInput, PhotoUpload, ConfirmDialog, StatusBadge, DataTable, year/class/session/exam form dialogs |
| Phase 4 — Authentication | ✅ Complete (mock SSO + mock login; real OAuth deferred to backend) |
| Phase 5 — Feature Modules | ✅ Complete (5.1 Landing, 5.2 role-aware Dashboard, 5.3 Youth list/detail/form, 5.4 Class list/detail with CRUD dialogs, 5.5 Attendance, 5.6 Exams + results, 5.7 Guest registration stepper, 5.8 Admin/Users, 5.9 Catechist list/detail) |
| Phase 6 — Polish & Production | ✅ Complete (6.1 global error interceptor + 404, 6.2 loading service/skeletons, 6.3 ~~build output → `wwwroot`~~ replaced by Phase 7) |
| Bug Fixes — Detail Pages | ✅ Fixed: (1) `catchError` added to forkJoin in catechist-detail and youth-detail so API errors show empty state instead of sticking on "Loading…"; (2) youth-detail removed redundant `/contacts` request — contact data now embedded in `EucharisticYouthContact.contact` from mock; (3) contacts tab uses `yc.contact` directly; (4) content-projection fix: `matListItemTitle`/`matListItemLine` moved to top-level of `mat-list-item` (not inside `@if`) with conditional content rendered inside the span/ng-container; (5) class-history and assignment tabs now show fallback ID when class lookup fails; (6) `enrollmentStatusKey`/`relationKey` helpers simplified to template-literal (`\`enrollment.status.${s}\``) |
| Phase 7 — Decouple Frontend from API | ✅ Complete — Angular outputs to `dist/TNTTBD.UI` (independent deployment); CORS added to ASP.NET Core `Program.cs` (origins from `appsettings.json`); per-environment API URLs (dev/staging/prod); `proxy.conf.json` for local dev DX; `wwwroot` removed from API project |
| Phase 8 — Catechist One-Class-Per-Year + Lazy Tabs | ✅ Complete |
| Phase 9 — Patron Saints, Catechist Form & Profile Tabs | ✅ Complete |
| Phase 10 — Remove Attendance/Exams Pages + Role-Based UX | ✅ Complete |

Build verified: `npm run build` outputs to `dist/TNTTBD.UI`. `ng serve` proxies `/api` → `http://localhost:5062`. Mocks active via `environment.useMocks: true`.

---

## Phase 1: Project Scaffolding & Core Infrastructure

| Step | Task | Notes |
|------|------|-------|
| 1.1 | Project init | `ng new TNTTBD.UI`, SCSS, standalone, configure output → `wwwroot` for .NET serving, dev proxy to API |
| 1.2 | Material 3 theme | Custom M3 palette (Blue #1976d2 primary, Amber #FFC107 accent), dark variant, typography |
| 1.3 | Folder structure | `core/` (auth, i18n, layout, services) → `shared/` (components, models, enums, pipes) → `features/` (lazy-loaded) |
| 1.4 | Core services | `ApiService`, `AuthService` (Signal-based currentUser), `ThemeService` (darkMode signal + localStorage), `TranslationService` |
| 1.5 | Auth infrastructure | Functional interceptor (Bearer token), `authGuard`, `roleGuard`, `HasRoleDirective` |
| 1.6 | i18n | Custom lightweight: load JSON files, `TranslationService.t(key)`, `TranslatePipe`, localStorage lang preference |

---

## Phase 2: Layout & Navigation Shell (*depends on Phase 1*)

| Step | Task | Notes |
|------|------|-------|
| 2.1 | Shell component | Material Toolbar: logo, nav links (role-filtered), language/theme toggles, user menu. Hamburger → sidenav on mobile |
| 2.2 | Responsive strategy | Breakpoints: <768 mobile, 768-1024 tablet, >1024 desktop. SCSS mixins + `BreakpointObserver` |
| 2.3 | Route structure | Public: `/`, `/auth/*`, `/register-youth`. Protected under `/app/*` with lazy-loaded feature children, role guards per route |

**Route map:**
```
/                    → Landing (public)
/auth/login          → Login (public)
/register-youth      → Guest registration (public)
/app/dashboard       → Dashboard (all authenticated)
/app/youth           → Youth list/detail (Admin, Catechist, EucharisticYouthContact, EucharisticYouth) 
/app/catechists      → Catechist list/detail (Admin, Catechist)
/app/classes         → Year/Class/Session management (Admin, Catechist, EucharisticYouth)
/app/attendance      → Mark attendance (Admin, Catechist)
/app/exams           → Exam management & results (Admin, Catechist)
/app/admin/users     → User management (Admin only)
```

---

## Phase 3: Shared Components & Models (*parallel with Phase 2*)

| Step | Task | Notes |
|------|------|-------|
| 3.1 | TypeScript models | Interfaces mirroring all domain DTOs + enums: User, Youth, Catechist, Contact, CatechismYear/Class/Session, Exam/ExamResult, enrollments, FullName, PatronSaint, PaginatedResult<T> |
| 3.2 | Shared components | `FullNameInputComponent`, `PhotoUploadComponent` (file + camera), `ConfirmDialogComponent`, `DataTableComponent<T>`, `StatusBadgeComponent`, `PatronSaintInputComponent` |
| 3.3 | Directives & Pipes | `HasRoleDirective`, `FullNamePipe` (→ "Tên Thánh Họ Đệm Tên"), `TranslatePipe`, `DateLocalePipe` |

---

## Phase 4: Authentication Feature (*depends on Phase 1.5*)

| Step | Task | Notes |
|------|------|-------|
| 4.1 | Login page | Email/password form + Google/Microsoft SSO buttons |
| 4.2 | SSO callback page | Handle OAuth redirect, exchange code for JWT |
| 4.3 | AuthService integration | `login()`, `loginWithSSO()`, `handleCallback()`, `logout()`, auto-populate `currentUser` signal on app init |

---

## Phase 5: Feature Modules

| Step | Task | Depends on |
|------|------|-----------|
| 5.1 | Landing page | Phase 2 (parallel with 4) — intro text, sign-in button, register link |
| 5.2 | Dashboard | Phase 4 — role-based cards. Catechist sees own class assignments + roles (Main/Member/Assistance). Youth/Contact sees enrolled class + next session. Admin sees totals + quick links |
| 5.3 | Youth management | Phase 3 — List (table + search) + Detail (tabbed: profile, milestones, photos, contacts, class history) + Create/Edit form |
| 5.4 | Class management | Phase 3 — Year list → Class detail (tabbed: info, enrolled youth, catechists, sessions) + CRUD dialogs |
| 5.5 | Attendance marking | Phase 5.4 — Select class → select/create session → full student list with Present/Absent/Excused toggle buttons, sticky save on mobile |
| 5.6 | Exam & results | Phase 5.4 — Within class detail: Exams tab lists exams (CRUD: title, examDate). Exam detail page: table of enrolled youth with score input, isDone checkbox, remark field; bulk save. Read-only view for Youth/Contact showing their own results |
| 5.7 | Guest registration | Phase 3 (parallel with 4) — Material stepper: Youth info → Contacts (Father+Mother default) → Account linking → Review & submit |
| 5.8 | Admin/Users | Phase 4 — User table + Create/Edit dialog (email, role, link entity, active toggle) |
| 5.9 | Catechist management | Phase 3 (parallel with 5.3) — List + Detail (profile, photos, class assignments) |

---

## Phase 6: Polish & Production (*depends on Phase 5*)

| Step | Task |
|------|------|
| 6.1 | Error handling — global HTTP interceptor → snackbar, 404 page, inline form validation |
| 6.2 | Loading states — skeleton loaders, button spinners |
| 6.3 | Build config — production build → `wwwroot`, .NET serves SPA fallback |

---

## Phase 8: Catechist One-Class-Per-Year + Enrollment History + Lazy-Load Tabs

**Business rule:** A catechist cannot be registered to more than one class in the same catechism year (regardless of role).

**Backend changes (Domain + Infrastructure):**
| Step | Task |
|------|------|
| 8.1 | `CatechismClassCatechist` domain entity — added `EnrolledAt`, `LeftAt`, `Status` (reuses `EnrollmentStatus`), `Note` + `EndEnrollment()` method |
| 8.2 | `CatechismClassCatechistEntity` — added matching persistence columns |
| 8.3 | `CatechismClassCatechistMapper` — maps all new fields in `ToDomain()`, `ToEntity()`, `UpdateEntity()` |
| 8.4 | Fixed FK bug in `CatechismClassCatechistEntityConfiguration` — `CatechistEntity` FK was incorrectly pointing to `CatechismClassId` instead of `CatechistId` |
| 8.5 | Fixed pre-existing bug in `CatechismClassEucharisticYouthMapper.ToDomain()` — was missing temporal args in constructor call |

**Frontend changes:**
| Step | Task |
|------|------|
| 8.6 | `CatechismClassCatechist` TS interface — added `enrolledAt`, `leftAt`, `status`, `note` |
| 8.7 | Mock data — enforced one-class-per-year (removed duplicate assignments); added historical entries with `Completed` status |
| 8.8 | **Catechist detail** — lazy-loads assignments/history tab on click; shows `enrolledAt`, `leftAt`, `status` per assignment (mirrors youth class-history) |
| 8.9 | **Youth detail** — lazy-loads contacts tab and class-history tab on click; milestones+photos remain embedded |
| 8.10 | **Class detail** — lazy-loads all 4 tabs independently (enrollments, catechists, sessions, exams); CRUD refreshes only the affected tab |
| 8.11 | Added i18n key `catechist.classHistory` (en/vi) |

**Backend gaps remaining (Phase 8):**
- [ ] `ICatechismClassCatechistRepository` with `HasActiveAssignmentInYear(catechistId, yearId)` validation
- [ ] `AddCatechistToClassCommand` handler — query `CatechismClass → CatechismYear`, reject if duplicate year
- [ ] EF migration for new `CatechismClassCatechistEntity` columns
- [ ] Wire up `CatechismClassCatechistController` endpoints (POST/PATCH/DELETE)

---

## Relevant Files (Backend Reference)
- `TNTTBD.Services/TNTTBD.Services.Domain/Catechism/Members/` — EucharisticYouth, Catechist, Contact, Member base
- `TNTTBD.Services/TNTTBD.Services.Domain/Catechism/` — CatechismYear, CatechismClass, CatechismSession, Exam, attendance/results
- `TNTTBD.Services/TNTTBD.Services.Domain/Catechism/JoinModels/` — enrollment & assignment entities
- `TNTTBD.Services/TNTTBD.Services.Domain/Enums/` — all domain enums
- `TNTTBD.Services/TNTTBD.Services.Domain/ValueObjects/` — FullName, PatronSaint
- `TNTTBD.Services/TNTTBD.Services.Api/Controllers/` — existing CQRS controller patterns

---

## Verification
1. `ng build` compiles without errors
2. `ng test` — unit tests for AuthService, YouthService, shared components
3. Manual: navigate all routes per role, verify guards block unauthorized
4. Manual: attendance marking flow on mobile viewport
5. Manual: guest registration stepper with validation
6. Manual: language toggle vi↔en, verify translations
7. Manual: dark/light theme toggle
8. Lighthouse: accessibility ≥ 90, performance ≥ 80
9. Phase 8: `ng build` + `dotnet build` pass after all changes
10. Phase 8: catechist detail → assignments tab only loads on click
11. Phase 8: youth detail → contacts & class history tabs only load on click
12. Phase 8: class detail → each tab loads independently on click
13. Phase 8: mock data respects one-class-per-year constraint

---

## Decisions
- **Signals + Service** (no NgRx) — appropriate for this app scale
- **Custom i18n** (no @ngx-translate) — lighter, Signal-native, only 2 languages
- **No PWA** for v1
- **Lazy loading** per feature for small initial bundle
- **Same-server deployment** — output to .NET `wwwroot`
- **Mock backend during UI development** — services hit in-memory fakes via an `HttpInterceptor` toggle (env flag `useMocks: true`). All real HTTP calls remain unchanged; only the interceptor short-circuits to canned data. Swap to real API by flipping the flag.
- **No upload size/type limits** in v1 — frontend sends file as-is via multipart; backend will enforce later.
- **Mock SSO flow** — `loginWithSSO(provider)` returns a fake JWT for a seeded user (one per role) so role-based UI can be exercised without real OAuth. Real Google/Microsoft integration deferred.
- **Locale & timezone** — default locale `vi-VN` (UTC+7), switchable to `en-US`. Register Angular locales (`registerLocaleData(localeVi)`) and use `LOCALE_ID` provider tied to the language signal. Material date adapter set to native with locale.
- **Catechist one-class-per-year** — strictly one class regardless of role. Backend validates on add; frontend shows warning in assign dialog. `EnrollmentStatus` enum reused for catechist lifecycle.
- **Lazy-load detail tabs** — all detail pages (catechist, youth, class) defer non-embedded data to tab-click. Embedded data (photos, milestones inside entity response) loads with the initial entity fetch. Pattern: `signal<T | null>` + `onTabChange(index)` + load-once flag.
- **API convention — no IDs in URL path.** All IDs (and other params) travel in the request payload (body for POST/PATCH/DELETE, query string for GET). Matches existing backend pattern (e.g., `GET /api/classes` with `GetCatechismClassByIdQuery` query binding, `PATCH /api/classes` with command body). Frontend services follow this for every entity.
- **Photo URLs** — backend `PhotoReference.storageKey` is a relative path only. Frontend prepends an absolute prefix from `environment.photoBaseUrl` (e.g., `https://cdn.tnttbd.example/photos/` in prod, `/mock-photos/` for mocks). A helper `photoUrl(storageKey: string): string` lives in `core/services/photo.service.ts`. Mocks return seeded storageKeys that resolve to placeholder images bundled in `assets/mock-photos/`.

---

## Date/Time Handling

### Two distinct types from backend
| Backend type | Used for | Frontend handling |
|---|---|---|
| `DateOnly` | DOB, milestone date, exam date, year start/end, baptism date | Display as `dd/MM/yyyy` (vi) / `MM/dd/yyyy` (en). Send to API as `'YYYY-MM-DD'` string. No timezone conversion ever. |
| `DateTimeOffset` | Session dateTime, attendance markedTime, photo uploadedAt, enrollment enrolledAt | Display in user's local time (auto via JS `Date`). Send as full ISO 8601 with offset (e.g., `2026-05-21T19:30:00+07:00`). |

### Frontend conventions
- Custom `DateLocalePipe` — wraps Angular `DatePipe` with active locale, defaults: `'shortDate'` for `DateOnly`, `'short'` for `DateTimeOffset`
- All `DateOnly` fields typed as `string` in TS models (e.g., `dateOfBirth: string`) with `'YYYY-MM-DD'` format — never converted to JS `Date` (avoids accidental TZ shift)
- All `DateTimeOffset` fields typed as `string` (ISO 8601) in models; converted to `Date` only at display time
- Material `MatDatepicker` configured with `MAT_DATE_LOCALE: 'vi-VN'` (or active locale signal); custom `NativeDateAdapter` override formats input as `dd/MM/yyyy`
- HTTP interceptor or DTO mapper to normalize: if backend ever returns `DateTime` without offset, treat as UTC

### Backend gaps to fill later (timezone-related)
- [ ] Confirm all timestamp columns in DB are stored as `timestamptz` (Postgres) / `datetimeoffset` (SQL Server) — preserves offset
- [ ] EF Core configuration: ensure `DateTimeOffset` properties don't get demoted to `DateTime` in projections/DTOs
- [ ] Server-side date math (e.g., "sessions this week") should be done in VN timezone (UTC+7), not server-local — use `TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")` or store parish TZ in config
- [ ] DTOs serializing `DateOnly` should output `'YYYY-MM-DD'` string (System.Text.Json default in .NET 8+), not a `DateTime`-like object
- [ ] Consider running the API server in UTC (or VN time) consistently; document the choice

---

## Mock Strategy (UI-only Development)

### Mock Interceptor
- File: `core/mocks/mock-api.interceptor.ts`
- Activates when `environment.useMocks === true`
- Intercepts requests matching API base URL, returns `of(mockData).pipe(delay(200))` for realism
- Falls through to real HTTP when no matching mock route

### Mock Data
- File: `core/mocks/mock-data.ts` — seeded in-memory arrays for: users (one per role), youths (~10), catechists (~3), 1 catechism year, 2 classes, sessions, enrollments
- File: `core/mocks/mock-handlers.ts` — route handler map: `'GET /api/youths' → (req) => paginate(mockYouths, req)`, etc.

### Assumed API Surface (TO BE FILLED ON BACKEND)
Endpoints the UI will call — backend currently missing some of these.
**Convention:** No IDs in URL paths. IDs go in query string (GET) or request body (POST/PATCH/DELETE).

**Auth**
- `POST /api/auth/login` body: `{ email, password }` → `{ token, user }`
- `POST /api/auth/sso-callback` body: `{ provider, code }` → `{ token, user }`
- `GET /api/auth/me` → current user
- `POST /api/auth/logout`

**Youth** (`/api/youths`)
- `GET /api/youths?search=&page=&pageSize=` → paginated list
- `GET /api/youths/detail?id=...` → full detail
- `POST /api/youths` body: full create payload
- `PATCH /api/youths` body: `{ id, ...fields }`
- `DELETE /api/youths` body: `{ id }`
- `GET /api/youths/enrollments?youthId=...` → class history across years
- `POST /api/youths/photos` multipart: `{ youthId, file, purpose }`
- `DELETE /api/youths/photos` body: `{ youthId, photoId }`
- `POST /api/youths/milestones` body: `{ youthId, type, date, documentCode, place, note }`
- `PATCH /api/youths/milestones` body: `{ youthId, milestoneId, ...fields }`
- `DELETE /api/youths/milestones` body: `{ youthId, milestoneId }`
- `POST /api/youths/contacts` body: `{ youthId, contact, relationType }`
- `DELETE /api/youths/contacts` body: `{ youthId, contactId }`

**Catechist** (`/api/catechists`) — mirror of youth (list, detail, CRUD, photos) following same convention

**Catechism**
- `GET /api/catechism-years` → list
- `POST /api/catechism-years` / `PATCH /api/catechism-years` / `DELETE /api/catechism-years` (id in body)
- `GET /api/classes?yearId=...` (exists partially)
- `GET /api/classes/detail?id=...`
- `POST /api/classes` / `PATCH /api/classes` / `DELETE /api/classes` (id in body)
- `GET /api/sessions?classId=...`, `GET /api/sessions/detail?id=...`, POST/PATCH/DELETE with id in body (exists partially)
- `POST /api/sessions/attendances` bulk (exists as `MarkAttendanceListCommand`)
- `GET /api/exams?classId=...` → list exams in a class
- `GET /api/exams/detail?id=...` → exam with results
- `POST /api/exams` / `PATCH /api/exams` / `DELETE /api/exams` (id in body)
- `POST /api/exams/results` bulk body: `{ examId, results: [{ catechismClassEucharisticYouthId, score, isDone, remark }] }`
- `PATCH /api/exams/results` body: `{ examId, resultId, score, isDone, remark }`
- `GET /api/exams/my-results?youthId=...` → youth/contact view of own results (MISSING)
- `POST /api/class-youth` body: `{ classId, youthId, ... }` enroll (exists partially)
- `PATCH /api/class-youth` body: `{ id, status, note }` update enrollment
- `DELETE /api/class-youth` body: `{ id }` un-enroll
- `POST /api/class-catechist` body: `{ classId, catechistId, role }` (MISSING)
- `PATCH /api/class-catechist` body: `{ id, role }` change role (MISSING)
- `DELETE /api/class-catechist` body: `{ id }` (MISSING)

**Registration** (public, no auth)
- `POST /api/registration/youth` body: `{ youth: {...}, contacts: [...], accountLink?: {...} }`

**Users (Admin)** (`/api/users`)
- `GET /api/users?search=&page=&pageSize=`
- `GET /api/users/detail?id=...`
- `POST /api/users` / `PATCH /api/users` / `DELETE /api/users` (id in body)
- `POST /api/users/link` body: `{ userId, entityType, entityId }`
- `POST /api/users/activate` body: `{ id }`
- `POST /api/users/deactivate` body: `{ id }`

### Backend Gaps to Fill Later
- [ ] All `/api/auth/*` endpoints (login, SSO callback, me, logout, JWT issuance)
- [ ] All `/api/users/*` endpoints (User aggregate exists in domain, no controller yet)
- [ ] `/api/registration/youth` public endpoint
- [ ] CatechismClassCatechist controller/commands (currently only `CatechismClassEucharisticYouth` exists)
- [ ] Photo upload + delete endpoints + `IFileStorage` implementation wiring
- [ ] Photo serving: expose a public base URL (CDN or `/photos/{storageKey}` route) so frontend can resolve `storageKey` to a real image URL
- [ ] Catechist controller's missing actions (currently only the file exists)
- [ ] CatechismYear controller endpoints (currently only the file exists)
- [ ] EucharisticYouth controller's missing actions
- [ ] Milestone management endpoints under Youth
- [ ] Contact management endpoints under Youth (add/remove contacts)
- [ ] `GET /api/youths/enrollments` for class history
- [ ] Exam controller endpoints (CRUD + bulk result save + per-youth results query)
- [ ] SSO providers registration (Google client ID, Microsoft app registration, redirect URIs)
- [ ] CORS configuration for dev (Angular dev server origin)
- [ ] Confirm all existing & new endpoints follow the "no ID in path" convention

## Scope Boundaries
- **Included v1**: Landing, Auth, Dashboard, Youth, Catechist, Classes, Sessions, Attendance, Exams/results, Registration, Admin/Users, i18n, theming, responsive
- **Excluded v1**: Real-time/WebSocket, PWA/offline, push notifications, reporting/export, bulk import
