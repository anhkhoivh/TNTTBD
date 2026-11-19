# Backend Plan: TNTTBD.Services — Missing Use Cases & APIs

## TL;DR

The frontend expects several API endpoints that don't yet exist in the backend. This plan covers the remaining use cases grouped by feature area: **List/Pagination queries**, **Exam management**, **CatechismClassCatechist (assignment)**, **Contacts**, **User/Auth**, **Registration**, and **Attendance query**. Each section explains the DDD/CQRS concepts and patterns to follow.

---

## Architecture Overview (for learning)

```
API Layer (Controllers) → Application Layer (Commands/Queries + Handlers) → Domain Layer (Aggregates, Entities, Value Objects)
                                                                          ↗ Infrastructure Layer (Repositories, QueryServices, EF Core)
```

- **Commands** = write operations (Create, Update, Delete). Return `Guid` or `void`. Go through the Domain to enforce business rules.
- **Queries** = read operations. Bypass domain logic, use QueryServices that query EF directly and return DTOs.
- **Handlers** = `IRequestHandler` implementations (registered via DI) that orchestrate Commands/Queries.
- **QueryServices** = Infrastructure implementations that read from DB, map to DTOs — used for list/paginated reads where going through the domain is wasteful.
- **Repositories** = Domain-centric persistence (load/save aggregates).

---

## What's Already Implemented

| Area | Commands (Write) | Queries (Read) |
|------|------------------|----------------|
| CatechismYear | Create, Update, Remove | GetById, **GetAll (paginated)** |
| CatechismClass | Create, Update, Remove | GetById, GetClassesByYear, **GetAll (paginated, ?yearId)** |
| CatechismSession | Create, Update, Remove, MarkAttendance, MarkAttendanceList | GetById, GetSessionsByClass, **GetAll (paginated, ?classId)** |
| EucharisticYouth | Create, Update, Remove, Milestones CRUD, PatronSaints CRUD, Photos | GetById, GetPhotos, **GetAll (paginated, ?search)** |
| Catechist | Create, Update, Remove, PatronSaints CRUD, Photos | GetById, GetPhotos, **GetAll (paginated)** |
| ClassEucharisticYouth | Add, Update, Remove, MarkCompleted | GetById, GetByClass, GetByYouth, **GetAll (paginated, ?classId)** |
| Exam | Create, Update, Remove, **SaveResults** | GetById (with results), **GetAll (paginated, ?classId)** |

---

## Phase 1: List/Paginated Queries (Foundation) ✅ DONE

**Why**: The frontend uses `api.list<T>(path)` which expects `PaginatedResult<T>` (`{ items, total, page, pageSize }`). Most existing queries return a single item or a flat `List<T>`. The frontend calls list endpoints for: `/classes`, `/catechism-years`, `/youths`, `/catechists`, `/sessions`, `/class-youth`, `/class-catechist`.

**What to add** (per aggregate that lacks a paginated list endpoint):

### 1.1 — Generic Paginated Response DTO
- File: `TNTTBD.Services.Application/Common/DTOs/PaginatedResult.cs`
- A generic `PaginatedResult<T>` record: `{ List<T> Items, int Total, int Page, int PageSize }`

### 1.2 — CatechismYear List Query
- `Application/CatechismYears/Queries/GetCatechismYearsQuery.cs` — `IRequest<PaginatedResult<CatechismYearDto>>`
- Handler in `Handlers/`
- QueryService method: `GetAllAsync(int page, int pageSize, CancellationToken)`
- Controller: `GET /api/catechism-years` → maps to list query

### 1.3 — CatechismClass List Query (all, not just by year)
- `Application/CatechismClasses/Queries/GetCatechismClassesQuery.cs` — returns paginated list of all classes
- Frontend calls `GET /api/classes` (no filter) to get all classes

### 1.4 — EucharisticYouth List Query
- `Application/EucharisticYouths/Queries/GetEucharisticYouthsQuery.cs`
- QueryService: `GetAllAsync(...)` returning list of `EucharisticYouthDto`
- Controller: `GET /api/youths` (list)

### 1.5 — Catechist List Query
- `Application/Catechists/Queries/GetCatechistsQuery.cs`
- Controller: `GET /api/catechists` (list)

### 1.6 — Session List with filter by class
- Already exists as `GetSessionsByClassQuery`, but controller route may need adjustment to support `GET /api/sessions?classId=...`

### 1.7 — Session Attendance List (read)
- `Application/CatechismSessions/Queries/GetAttendancesBySessionQuery.cs`
- Returns `PaginatedResult<CatechismSessionAttendanceDto>`
- Controller: `GET /api/sessions/attendances?sessionId=...`
- Frontend uses this in the AttendanceDialog to show existing marks

---

## Phase 2: Exam Management ✅ DONE

**Domain model exists** (`Exam`, `ExamResult`). Application layer & API implemented.

### 2.1 — Application Feature Folder
Create: `Application/Exams/` with subfolders: `Commands/`, `Queries/`, `Handlers/`, `DTOs/`, `Interfaces/`

### 2.2 — DTOs
- `ExamDto` — `{ Guid Id, Guid CatechismClassId, DateOnly ExamDate, string Title, List<ExamResultDto> ExamResults }`
- `ExamResultDto` — `{ Guid Id, Guid ExamId, Guid CatechismClassEucharisticYouthId, Guid MarkedByCatechistId, DateTimeOffset MarkedTime, double Score, bool IsDone, string? Remark }`

### 2.3 — Commands
- `CreateExamCommand(Guid CatechismClassId, DateOnly ExamDate, string Title)` → returns `Guid`
- `UpdateExamCommand(Guid Id, DateOnly? ExamDate, string? Title)`
- `RemoveExamCommand(Guid Id)`
- `MarkExamResultsCommand(Guid ExamId, List<ExamResultInput> Results)` — bulk score entry
  - `ExamResultInput` = `{ Guid CatechismClassEucharisticYouthId, double? Score, bool IsDone, string? Remark }`

### 2.4 — Queries
- `GetExamByIdQuery(Guid Id)` → `ExamDto` (with results)
- `GetExamsByClassQuery(Guid ClassId)` → `PaginatedResult<ExamDto>`

### 2.5 — Handlers
- Each command/query gets a handler class (follow existing patterns in `CatechismSessions/Handlers/`)

### 2.6 — Domain Repository
- `IExamRepository` in `Domain/Catechism/Repositories/`
- Infrastructure: `ExamRepository` + EF entity + configuration

### 2.7 — QueryService
- `IExamQueryService` interface in `Application/Exams/Interfaces/`
- `ExamQueryService` in `Infrastructure/Persistence/Catechism/QueryServices/`

### 2.8 — Controller
- `ExamController` at route `api/exams`
- Endpoints:
  - `POST /api/exams` — create
  - `GET /api/exams/detail?id=` — get by id
  - `GET /api/exams?classId=` — list by class
  - `PATCH /api/exams` — update
  - `DELETE /api/exams?id=` — remove
  - `POST /api/exams/results` — bulk mark results

---

## Phase 3: CatechismClassCatechist Management ✅ DONE

**Domain model exists** (`CatechismClassCatechist`). Application layer & controller implemented.

### 3.1 — Application Feature Folder
Create: `Application/CatechismClassCatechists/` with `Commands/`, `Queries/`, `Handlers/`, `DTOs/`, `Interfaces/`

### 3.2 — DTOs
- `CatechismClassCatechistDto` — mirrors the frontend model

### 3.3 — Commands
- `AddCatechistToClassCommand(Guid CatechistId, Guid CatechismClassId, CatechismClassCatechistRole Role)`
- `UpdateCatechismClassCatechistCommand(Guid Id, CatechismClassCatechistRole? Role, EnrollmentStatus? Status, string? Note)`
- `RemoveCatechistFromClassCommand(Guid Id)`

### 3.4 — Queries
- `GetCatechismClassCatechistsByClassQuery(Guid ClassId)` → list
- `GetCatechismClassCatechistsQuery()` → paginated list (frontend calls unfiltered for catechist-own-class detection)

### 3.5 — Repository & QueryService
- `ICatechismClassCatechistRepository` in Domain
- `CatechismClassCatechistQueryService` in Infrastructure

### 3.6 — Controller
- `CatechismClassCatechistController` at route `api/class-catechist`
- Endpoints:
  - `POST` — add catechist to class
  - `GET` — list (optional `?classId=` filter)
  - `PATCH` — update role/status
  - `DELETE` — remove

---

## Phase 4: Contacts ✅ DONE

**Domain model exists** (`Contact`, `EucharisticYouthContact`). Application layer & controller implemented.

### 4.1 — Application Feature Folder
Create: `Application/Contacts/` with subfolders

### 4.2 — DTOs
- `ContactDto` — `{ Guid Id, FullNameDto FullName, string? PhoneNumber, string? Email }`
- `EucharisticYouthContactDto` — `{ Guid Id, Guid EucharisticYouthId, Guid ContactId, ContactRelationType ContactRelationType, ContactDto? Contact }`

### 4.3 — Commands
- `CreateContactCommand(FullNameDto FullName, string? PhoneNumber, string? Email)` → `Guid`
- `LinkContactToYouthCommand(Guid EucharisticYouthId, Guid ContactId, ContactRelationType RelationType)` → `Guid`
- `UnlinkContactFromYouthCommand(Guid Id)`

### 4.4 — Queries
- `GetContactsByYouthQuery(Guid YouthId)` → `PaginatedResult<EucharisticYouthContactDto>` (with nested Contact)

### 4.5 — Controller
- Route: `api/youths/contacts`
- `GET /api/youths/contacts?youthId=` — list contacts of a youth
- `POST /api/youths/contacts` — link
- `DELETE /api/youths/contacts?id=` — unlink

---

## Phase 5: Authentication & User Management ✅ DONE

**Implemented**:
- `POST /api/auth/login` → `{ token, refreshToken, user }`
- `POST /api/auth/refresh` → refresh token rotation
- `POST /api/auth/logout` → revoke all refresh tokens
- `GET /api/auth/me` → current user info
- `POST /api/auth/sso-callback` → OAuth code exchange (Google + Microsoft)
- `GET /api/auth/sso/url?provider=` → OAuth authorization URL
- `GET /api/users` → `PaginatedResult<User>` (Admin only)
- JWT HS256, 60min access + 7-day refresh with rotation
- Frontend auth interceptor with 401 retry + concurrent request queuing
- Frontend SSO flow with popup redirect

### 5.1 — Expand User Domain Model
- File: refactor `TNTTBD.Services.Domain/User.cs` (move to `Catechism/` folder or keep at root)
- Add properties: `Id`, `Email`, `UserName`, `IsActive`, `CatechistId?`, `EucharisticYouthId?`, `ContactId?`
- Add `IUserRepository` in `Domain/Catechism/Repositories/`

### 5.2 — Auth Application Feature
Create: `Application/Auth/`
- `LoginCommand(string Email, string Password)` → `LoginResponseDto { string Token, UserDto User }`
- `SsoCallbackCommand(string Provider, string Code)` → `LoginResponseDto`
- JWT token generation service interface: `IJwtTokenService` in `Application/Abstractions/`

### 5.3 — User Management Application Feature
Create: `Application/Users/`
- `GetUsersQuery()` → `PaginatedResult<UserDto>`
- `CreateUserCommand`, `UpdateUserCommand`, `DeactivateUserCommand`
- `UserDto` — mirrors frontend `User` model

### 5.4 — Infrastructure: JWT
- `JwtTokenService` implementation in Infrastructure
- Middleware or filter for JWT validation
- Add authentication to `Program.cs`

### 5.5 — Controllers
- `AuthController` at route `api/auth`
  - `POST /api/auth/login`
  - `POST /api/auth/sso-callback`
- `UserController` at route `api/users`
  - `GET /api/users` — list (Admin only)
  - `POST /api/users` — create
  - `PATCH /api/users` — update
  - `DELETE /api/users?id=` — deactivate

### 5.6 — Authorization
- Add `[Authorize]` attributes to controllers
- Role-based authorization policies: Admin, Catechist, etc.

---

## Phase 6: Youth Registration ✅ DONE

**Frontend POSTs to** `POST /api/registration/youth`. Handler creates Youth + Contacts + links.
```
{
  youth: { fullName, dateOfBirth, isMale, address, baptism? },
  contacts: [{ givenName, familyName, relation, phoneNumber, email }],
  accountEmail?: string
}
```

### 6.1 — Application Feature
Create: `Application/Registration/`
- `RegisterYouthCommand` — composite command that:
  1. Creates an `EucharisticYouth` (with auto-generated code via `EucharisticYouthRunningNumber`)
  2. Optionally adds Baptism milestone
  3. Creates `Contact` records
  4. Links them via `EucharisticYouthContact`
  5. Optionally creates a User with `EucharisticYouthContact` role linked to the contact

### 6.2 — Controller
- `RegistrationController` at route `api/registration`
- `POST /api/registration/youth` — no auth required (public)

---

## Phase 7: Enrichment (Nice-to-have / Future)

- **Dashboard stats endpoint**: `GET /api/dashboard` — attendance rates, upcoming sessions
- **Admin user CRUD UI** — backend endpoints ready, frontend not built
- **Photo upload** — component exists but not wired into form templates
- **~15 backend CRUD endpoints** for milestones, patron saints, photos, year/class management — frontend features not yet built
- **UpdateCatechist/UpdateEucharisticYouth handlers** accept PatronSaints/Milestones in command but delegate to individual management endpoints
- **Validation**: Add FluentValidation behaviors (the `TransactionBehavior` already exists)
- **Error localization**: Already partially built (`en.json`, `vi.json` in API Resources)

---

## Integration Audit ✅ DONE (All builds pass)

The following integration issues were identified and fixed:
1. Detail endpoints: `/youths/detail`, `/catechists/detail` (FE was calling wrong paths)
2. Attendance GET query: new backend query/handler/controller for reading session attendances
3. Logout: FE now calls `POST /auth/logout` before clearing local state
4. Token refresh: Full 401 retry interceptor with concurrent request queuing
5. Response shapes: `/youths/contacts` and `/youths/enrollments` return `List<T>` not paginated
6. Body mismatches: 8 fixes — all request bodies now use correct field names matching backend commands
   - catechist-form: flat fields + `catechistId` on update
   - youth-form: flat fields + `eucharisticYouthId` on update + contacts structure
   - session-form-dialog: `catechismSessionId` + `catechismClassId`
   - class-detail: `catechismSessionId` for delete
   - class-list: `catechismClassId` for delete
   - attendance-dialog: `markedByCatechistId` + `attendanceItems`

---

## Relevant Files (Reference for Patterns)

| File | Use as template for |
|------|---------------------|
| `Application/CatechismSessions/Commands/CreateCatechismSessionCommand.cs` | Command record pattern |
| `Application/CatechismSessions/Handlers/` | Handler implementation pattern |
| `Application/CatechismClasses/Interfaces/ICatechismClassQueryService.cs` | QueryService interface pattern |
| `Infrastructure/Persistence/Catechism/QueryServices/CatechismClassQueryService.cs` | QueryService EF implementation |
| `Infrastructure/Persistence/Catechism/Repositories/CatechismSessionRepository.cs` | Repository pattern |
| `Api/Controllers/CatechismSessionController.cs` | Controller pattern (`[FromServices]` handler injection) |
| `Application/Behaviors/TransactionalRequestHandler.cs` | Transactional handler decorator pattern |
| `Domain/Catechism/CatechismSession.cs` | Aggregate with child entity collection |
| `Domain/Common/AggregateRoot.cs` | Base class for aggregates |

---

## Implementation Order & Dependencies

```
Phase 1 (List Queries) — no dependencies, foundational
    ↓
Phase 2 (Exams) — depends on Phase 1 for PaginatedResult<T>
Phase 3 (ClassCatechist) — depends on Phase 1
Phase 4 (Contacts) — depends on Phase 1
    ↓ (all parallel with each other)
Phase 5 (Auth & Users) — can be done in parallel but should be done before Phase 6
    ↓
Phase 6 (Registration) — depends on Phase 4 (Contacts) + Phase 5 (optional user creation)
```

---

## Verification Steps

1. **Per feature**: Write a `.http` file request (like existing `TNTTBD.Services.Api.http`) testing each new endpoint
2. **Integration**: Run the Angular frontend with `proxy.conf.json` pointing to the .NET API — verify each page loads without 404/500
3. **Domain rules**: Unit test domain methods (e.g., `Exam.MarkResult` throws if duplicate, `EucharisticYouth.AddMilestone` requires Baptism first)
4. **E2E flow**: Register a youth → verify Contact + Youth + optional User created

---

## Decisions & Scope

- **Included**: All endpoints the frontend currently calls that return 404
- **Excluded**: Dashboard stats API, advanced search/filtering, file upload (already implemented), notification/email
- **Auth strategy**: JWT with simple email/password + external provider callback (actual OAuth redirect handled by frontend or separate middleware)
- **Pagination**: Default page size 50 (matching frontend's default)

Here's the comprehensive backend plan:

---

## Plan: Backend Missing Use Cases & APIs

The frontend calls several API endpoints that don't yet exist in the backend. This plan covers **6 phases** of work, ordered by dependency. Each section explains the DDD/CQRS pattern to follow.

---

### Architecture Recap

```
API (Controllers) → Application (Commands/Queries + Handlers) → Domain (Aggregates)
                                                               ↗ Infrastructure (Repos, QueryServices, EF)
```

- **Commands** = writes (enforce domain rules via aggregates)
- **Queries** = reads (bypass domain, use QueryServices → DTOs directly)
- **Handlers** = `IRequestHandler` orchestrators
- **QueryServices** = EF reads mapped to DTOs (no domain loading)

---

### What's Already Done

| Area | Write ops | Read ops |
|------|-----------|----------|
| CatechismYear | Create, Update, Remove | GetById |
| CatechismClass | Create, Update, Remove | GetById, GetByYear |
| CatechismSession | Create, Update, Remove, MarkAttendance(s) | GetById, GetByClass |
| EucharisticYouth | Full CRUD + Milestones + PatronSaints + Photos | GetById, GetPhotos |
| Catechist | Full CRUD + PatronSaints + Photos | GetById, GetPhotos |
| ClassEucharisticYouth | Add, Update, Remove, MarkCompleted | GetById, GetByClass, GetByYouth |

---

### Phase 1: Paginated List Queries (Foundation)

Frontend uses `api.list<T>()` expecting `{ items, total, page, pageSize }`. Most current queries return single items or flat lists.

**Steps:**
1. Create `Application/Common/DTOs/PaginatedResult.cs` — generic `PaginatedResult<T>`
2. Add list queries + handlers for: CatechismYears, CatechismClasses (all), EucharisticYouths (all), Catechists (all)
3. Add `GetAttendancesBySessionQuery` — frontend reads existing attendance marks via `GET /sessions/attendances?sessionId=`
4. Update controllers to expose `GET` list endpoints (adjust routes where needed)

**Key files to modify:**
- Each feature's `Queries/` and `Handlers/` folders
- Each QueryService interface + implementation
- Controllers for new list routes

---

### Phase 2: Exam Management (New)

Domain models exist (`Exam`, `ExamResult`) but have **no Application layer or Controller**.

**Steps:**
1. Create folder `Application/Exams/` with `Commands/`, `Queries/`, `Handlers/`, `DTOs/`, `Interfaces/`
2. DTOs: `ExamDto`, `ExamResultDto`
3. Commands: `CreateExamCommand`, `UpdateExamCommand`, `RemoveExamCommand`, `MarkExamResultsCommand` (bulk)
4. Queries: `GetExamByIdQuery`, `GetExamsByClassQuery`
5. Domain: add `IExamRepository`; Infrastructure: `ExamRepository` + EF entity config
6. Infrastructure: `ExamQueryService`
7. Controller: `ExamController` at `api/exams` with routes:
   - `POST /api/exams`, `GET /api/exams?classId=`, `GET /api/exams/detail?id=`, `PATCH /api/exams`, `DELETE /api/exams?id=`, `POST /api/exams/results`

**Pattern reference:** Follow `CatechismSession` — it's the closest analog (aggregate with child collection `_examResults`).

---

### Phase 3: CatechismClassCatechist (New)

Domain model `CatechismClassCatechist` exists but has no dedicated feature.

**Steps:**
1. Create `Application/CatechismClassCatechists/` folder structure
2. Commands: `AddCatechistToClassCommand`, `UpdateCatechismClassCatechistCommand`, `RemoveCatechistFromClassCommand`
3. Queries: `GetCatechismClassCatechistsQuery` (paginated, optional `?classId=` filter)
4. Domain: add `ICatechismClassCatechistRepository`; Infrastructure: repository + query service
5. Controller: `CatechismClassCatechistController` at `api/class-catechist`

---

### Phase 4: Contacts (New)

Domain models `Contact` + `EucharisticYouthContact` exist but have no Application layer.

**Steps:**
1. Create `Application/Contacts/` folder structure
2. DTOs: `ContactDto`, `EucharisticYouthContactDto` (with nested `Contact`)
3. Commands: `CreateContactCommand`, `LinkContactToYouthCommand`, `UnlinkContactFromYouthCommand`
4. Queries: `GetContactsByYouthQuery(Guid YouthId)` → paginated
5. Controller at `api/youths/contacts`:
   - `GET ?youthId=` — list contacts of a youth
   - `POST` — create + link
   - `DELETE ?id=` — unlink

---

### Phase 5: Authentication & Users

Frontend expects `POST /api/auth/login`, `POST /api/auth/sso-callback`, `GET /api/users`.

**Steps:**
1. Expand `User` domain model — add `Id`, `Email`, `UserName`, `IsActive`, `CatechistId?`, `EucharisticYouthId?`, `ContactId?`
2. Create `Application/Auth/` — `LoginCommand`, `SsoCallbackCommand` → `LoginResponseDto { Token, UserDto }`
3. Create `Application/Users/` — `GetUsersQuery`, `CreateUserCommand`, `UpdateUserCommand`
4. Abstraction: `IJwtTokenService` in Application; implementation in Infrastructure
5. Add JWT authentication middleware in `Program.cs`
6. Controllers: `AuthController` (`api/auth`), `UserController` (`api/users`)
7. Add `[Authorize]` + role policies to existing controllers

---

### Phase 6: Youth Registration (New)

Frontend POSTs to `POST /api/registration/youth` (public, no auth).

**Steps:**
1. Create `Application/Registration/RegisterYouthCommand` — composite:
   - Creates `EucharisticYouth` (auto-code via `EucharisticYouthRunningNumber`)
   - Optionally adds Baptism milestone
   - Creates `Contact` records + links via `EucharisticYouthContact`
   - Optionally provisions a User account
2. Controller: `RegistrationController` at `api/registration` — `[AllowAnonymous]`

*Depends on:* Phase 4 (Contacts) + Phase 5 (User creation).

---

### Dependency Graph

```
Phase 1 (Lists) ← foundational, do first
   ↓
Phase 2 (Exams) ┐
Phase 3 (ClassCatechist) ├── parallel with each other
Phase 4 (Contacts) ┘
   ↓
Phase 5 (Auth/Users) ← can parallelize with 2-4, but needed before 6
   ↓
Phase 6 (Registration) ← depends on 4 + 5
```

---

### Verification

1. Add requests to `TNTTBD.Services.Api.http` for each new endpoint
2. Run Angular frontend → verify no 404s on any page/dialog
3. Unit test domain logic: `Exam.MarkResult` duplicate check, `EucharisticYouth.AddMilestone` baptism-first rule
4. E2E: Submit registration → verify Youth + Contacts + optional User created in DB

---

### Decisions

- **Scope IN**: All endpoints the frontend currently calls that return 404
- **Scope OUT**: Dashboard stats API, advanced search, email notifications, file upload (already done)
- **Auth**: JWT with email/password + SSO callback (OAuth redirect handled externally)
- **Pagination default**: page size 50 (matching frontend)
