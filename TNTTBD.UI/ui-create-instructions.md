# UI Implementation Plan (Prompt)

## Current Mode: PLAN

### Context
- App: TNTTBD.UI (Angular 21, Material, Signals, Responsive)
- Backend: TNTTBD.Services (API, CQRS, DDD)
- This file tracks the current UI implementation plan and prompt for the agent.

---

## Form Modernization Initiative (May 29, 2026)

### Goal
Standardize all forms in the UI codebase to use Angular Reactive Forms (FormBuilder, FormGroup, FormArray), replacing any remaining ngModel/template-driven patterns. Add comprehensive validation and shared utilities for maintainability and testability.

### Scope
- All form components (dialogs, pages, ControlValueAccessor inputs)
- Remove ngModel from exam-results-dialog (and any others)
- Add shared utilities for date conversion, validators, and form helpers
- Add reusable error display component
- Add unit tests and documentation

### Decisions
- Use FormArray<FormGroup> for multi-row forms (e.g., exam-results-dialog)
- Add custom validators for score, date, and cross-field logic
- Use signals for UI state only (loading, saving), not for form values
- No async validators in Phase 1

### Phases
1. Create shared utilities (date-form.util.ts, validators.util.ts, form-helpers.ts)
2. Refactor simple dialogs (class-form, year-form, session-form, exam-form)
3. Refactor complex forms (youth-form, register, catechist-form, full-name-input, patron-saint-input)
4. Refactor exam-results-dialog (ngModel → FormArray)
5. Add tests and FORMS.md documentation
6. Cleanup and deduplicate

### Status
- Phase 1: Completed (utilities created: date-form.util.ts, validators.util.ts, form-helpers.ts, form-errors-display.component.ts)
- Phase 4: Completed (exam-results-dialog migrated to FormArray, login migrated to ReactiveFormsModule)
- Next: Phase 2 (refactor simple dialogs) when ready

---

## Backend Integration Status (May 29, 2026)

### Completed
- Exam feature fully implemented: ExamController with CRUD + SaveResults endpoint
  - POST /api/exams — Create exam
  - GET /api/exams — List exams (paginated, ?classId)
  - GET /api/exams/detail — Get exam by ID with results
  - PATCH /api/exams — Update exam
  - DELETE /api/exams — Remove exam
  - POST /api/exams/results — Save exam results (score, isDone, remark per student)
- All 7 feature areas now have backend endpoints (Years, Classes, Sessions, Youth, Catechists, Enrollments, Exams)

### Still Missing
- CatechismClassCatechist (assign catechists to classes) — no controller yet
- Youth Registration (self-service) — no endpoint yet
- Photo Upload — IFileStorage exists but not exposed via API
- User Management — no application layer
- SSO Authentication — basic login only

---

## Instructions
- Keep this file up to date with the current UI plan and major architectural decisions.
- Use ui-create-details.md for feature-level requirements and design notes.
- Use services-plan.md for backend API planning.
