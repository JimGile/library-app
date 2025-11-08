```markdown
# Implementation Tasks: Library - Initial Requirements

Feature: Library App (backend: Django + DRF; frontend: React)
Spec: `specs/003-library-app/spec.md`
Plan: `specs/003-library-app/plan.md`

Notes: Tasks are organized by Phase then by User Story (priority order). Each task follows the required checklist format: "- [ ] T### [P] [USx] Description with file path".

---

## Phase 1 — Setup

- [ ] T001 Create backend Python virtualenv and requirements file at `backend/requirements.txt`
- [ ] T002 Initialize Django project in `backend/` with `manage.py` and core settings at `backend/backend/settings/`
- [ ] T003 [P] Create backend apps: `books`, `members`, `reservations` under `backend/apps/` (create `apps/books/`, `apps/members/`, `apps/reservations/`) 
- [ ] T004 Initialize frontend using Vite or CRA in `frontend/` and add `package.json` at `frontend/package.json`
- [ ] T005 [P] Add basic CI config for backend/frontend (create `.github/workflows/ci-backend.yml` and `.github/workflows/ci-frontend.yml`)
- [ ] T006 Create `README.md` in repo root with quickstart notes referencing `backend/README.md` and `frontend/README.md`

## Phase 2 — Foundational (blocking prerequisites)

- [ ] T007 Create `Book` model in `backend/apps/books/models.py` with fields (Id, Title, Author, Category FK, Description, IsAvailable, CreatedAt) [US1]
- [ ] T008 Create `Category` model in `backend/apps/books/models.py` with fields (Id, Name, Description) [US1]
- [ ] T009 Create `Member` model in `backend/apps/members/models.py` with fields (Id, Name, Email, PasswordHash, Balance, Membership fields) [US2]
- [ ] T010 Create `Reservation` model in `backend/apps/reservations/models.py` with fields (Id, Book FK, Member FK, StartDate, DueDate, ReturnDate nullable, Status, LateFee) [US3]
- [ ] T011 [P] Add Django admin registrations for Book/Category/Member/Reservation in `backend/apps/*/admin.py` [US4]
- [ ] T012 Create initial migrations and apply them locally (`backend/` migrations) — files under `backend/apps/*/migrations/`
- [ ] T013 Implement database seeding script for sample books/categories/members at `backend/scripts/seed_data.py` and wire to README quickstart [US1]
- [ ] T014 Add DRF and basic API router in `backend/apps/api/urls.py` and project `backend/backend/urls.py`
- [ ] T015 Implement base API error handling and transactional utilities at `backend/apps/core/utils.py` (helpers for atomic reservations) [US3]
- [ ] T016 [P] Create frontend base layout, routing and app shell in `frontend/src/App.{tsx,jsx}` and `frontend/src/pages/` with Home placeholder [US1]

## Phase 3 — User Stories (Priority order)

### User Story 1 — Browse and discover books (P1)

- [X] T017 [US1] Implement `BookSerializer` and `CategorySerializer` in `backend/apps/books/serializers.py` [P]
- [X] T018 [US1] Implement paginated Book list API with search/sort filters in `backend/apps/books/views.py` (endpoint: `GET /api/books/`) — support query params `?q=`, `?category=`, `?ordering=title,-author`, `?page=`
- [X] T019 [US1] Add URL route `backend/apps/books/urls.py` mapping `/api/books/` to list view
- [X] T020 [US1] Implement Book detail API `GET /api/books/{id}/` in `backend/apps/books/views.py`
- [X] T021 [US1] Create frontend `BookList` component at `frontend/src/components/BookList/BookList.tsx` that consumes `/api/books/` and supports search, sorting and pagination
- [X] T022 [US1] Create frontend `BookDetails` page at `frontend/src/pages/BookDetails/BookDetails.tsx` which consumes `/api/books/{id}/`
- [X] T023 [P] [US1] Add client-side routing for `/books` and `/books/:id` in `frontend/src/router.tsx` or `frontend/src/App.tsx`
- [X] T024 [US1] Add unit test(s) for Book list API (happy path search and pagination) in `backend/tests/test_books_api.py`

### User Story 2 — Member registration and authentication (P1)

- [ ] T025 [US2] Implement registration API `POST /api/auth/register/` in `backend/apps/members/views.py` and serializer in `backend/apps/members/serializers.py`
- [ ] T026 [US2] Implement login API `POST /api/auth/login/` (DRF Token or JWT) at `backend/apps/members/views.py` and wire tokens to `backend/apps/members/urls.py`
- [ ] T027 [US2] Create frontend `Register` and `Login` pages at `frontend/src/pages/Auth/Register.tsx` and `frontend/src/pages/Auth/Login.tsx` and client auth service at `frontend/src/services/auth.ts`
- [ ] T028 [US2] Add authentication state handling in frontend at `frontend/src/context/AuthContext.tsx` (login/logout, token storage)
- [ ] T029 [US2] Add backend tests for registration/login in `backend/tests/test_auth.py`

### User Story 3 — Reserve and manage borrowed books (P1)

- [ ] T030 [US3] Implement reservation create API `POST /api/reservations/` in `backend/apps/reservations/views.py` with transactional checks (book availability and 3-active-reservation limit); file: `backend/apps/reservations/views.py`
- [ ] T031 [US3] Implement reservation return API `POST /api/reservations/{id}/return/` updating ReturnDate, calculating late fees and updating `Member.balance` in `backend/apps/reservations/views.py`
- [ ] T032 [US3] Add service/utility function for late-fee calculation and reservation status updates in `backend/apps/reservations/services.py` or `backend/apps/reservations/utils.py`
- [ ] T033 [US3] Create frontend `MyReservations` page at `frontend/src/pages/MyReservations/MyReservations.tsx` that consumes `/api/reservations/` for current member
- [ ] T034 [US3] Add Reserve button to `BookDetails` frontend component and call `POST /api/reservations/` in `frontend/src/components/BookDetails/BookDetails.tsx` [P]
- [ ] T035 [US3] Add return action UI on `MyReservations` rows to call `POST /api/reservations/{id}/return/` in `frontend/src/components/MyReservations/ReservationRow.tsx`
- [ ] T036 [US3] Add backend unit/integration tests for reservation creation, 3-reservation limit, and concurrent reservation attempt in `backend/tests/test_reservations.py`

### User Story 4 — Admin book/category/member management (P2)

- [ ] T037 [US4] Implement admin API endpoints for CRUD on books and categories in `backend/apps/books/views_admin.py` (or DRF viewsets) and wire to `backend/apps/books/urls_admin.py`
- [ ] T038 [US4] Create frontend admin pages for Books and Categories at `frontend/src/pages/Admin/Books.tsx` and `frontend/src/pages/Admin/Categories.tsx` (protected by admin auth) [P]
- [ ] T039 [US4] Add admin member listing and search endpoint `GET /api/admin/members/` in `backend/apps/members/views_admin.py`
- [ ] T040 [US4] Add backend tests for admin CRUD operations in `backend/tests/test_admin.py`

### User Story 5 — Informational pages (P3)

- [ ] T041 [US5] Add About and FAQ static pages in frontend at `frontend/src/pages/About.tsx` and `frontend/src/pages/FAQ.tsx`
- [ ] T042 [US5] (Optional) Add static views in backend if serving content from Django at `backend/apps/content/views.py` and `backend/apps/content/urls.py`

## Final Phase — Polish & Cross-Cutting Concerns

- [ ] T043 [P] Add API request client and shared types in `frontend/src/services/api.ts` and `frontend/src/types/`
- [ ] T044 [P] Add logging and monitoring hooks in backend at `backend/backend/middleware/logging.py`
- [ ] T045 Update `specs/003-library-app/quickstart.md` with concrete run steps for local dev (how to run backend and frontend) and include example seed data commands
- [ ] T046 Add end-to-end smoke test(s) (Playwright or Cypress) in `tests/e2e/` that cover core happy path: register → login → browse → reserve → return
- [ ] T047 Run format/linters: `backend/` use `ruff`/`black` (python), `frontend/` run `eslint`/`prettier` and fix obvious issues

### US1 polish — search & pagination

- [X] T021.1 [US1] Add frontend search input and page size selector to `frontend/src/pages/BookList.tsx` (completed)
- [X] T021.2 [US1] Add prev/next pagination controls wired to API (completed)
- [X] T021.3 [US1] Add server-side field filters and ordering support (django-filter + DRF Ordering) (completed)
- [X] T021.4 [US1] Add lightweight list serializer to reduce payload for list endpoints (completed)

## Dependencies and Story Order

1. Phase 1 (T001–T006) must be completed first.
2. Phase 2 (T007–T016) is blocking for most story work; complete migrations and seed data before UI work that depends on data.
3. User stories should proceed in priority order: US1 (T017–T024) → US2 (T025–T029) → US3 (T030–T036) → US4 (T037–T040) → US5 (T041–T042).

## Parallelization Opportunities

- Frontend components (T021, T022, T023, T027, T033, T034, T035) can be developed in parallel once the corresponding backend endpoints exist or are mocked. Marked with [P].
- Backend serializers and routes (T017, T018, T019, T020) can be implemented in parallel with models (T007–T010) once migrations are created and applied.
- Admin UI (T038) and admin API (T037) can be built in parallel.

## Independent Test Criteria (one-liners)

- US1: Catalog page responds with paginated JSON and frontend shows pages; search and sort change results. (Verify: `/api/books/?q=term&page=1&ordering=title` returns expected JSON.)
- US2: New user can POST `/api/auth/register/` and then POST `/api/auth/login/` to obtain auth token.
- US3: Authenticated POST `/api/reservations/` on available book returns 201 and reduces availability; POST `/api/reservations/{id}/return/` updates return date and late fee.
- US4: Admin CRUD endpoints accept create/update/delete and changes reflect in list endpoints.
- US5: Visiting `/about` and `/faq` returns rendered pages.

## Suggested MVP Scope

- Minimum MVP: Complete Phase 1/2 and US1 + US2 + US3 core happy paths (T001–T036). This allows public browsing, registration/login and the reservation lifecycle.

## Format validation

All tasks above follow the required checklist format with Task IDs and Story labels where appropriate. Parallelizable tasks include a `[P]` marker.

---

Deliverable: `specs/003-library-app/tasks.md`

Summary (generated):

- Total tasks: 47
- Tasks per story/phase:
  - Phase 1: 6
  - Phase 2: 10
  - US1: 8
  - US2: 5
  - US3: 7
  - US4: 4
  - US5: 2
  - Final/Polish: 5

Parallel opportunities: frontend components, serializers/routes, admin UI/API (see section above)

Independent test criteria: See section above (one-liners per story).

MVP suggestion: complete through T036 (US3) first.

```
