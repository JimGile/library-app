# Implementation Plan: [FEATURE]

**Branch**: `[###-feature-name]` | **Date**: [DATE] | **Spec**: [link]
**Input**: Feature specification from `/specs/[###-feature-name]/spec.md`

**Note**: This template is filled in by the `/speckit.plan` command. See `.specify/templates/commands/plan.md` for the execution workflow.

## Summary

Implement the Library application feature set (browse/search/sort books, member registration/login, reservation lifecycle, admin CRUD for books/categories/members, and informational pages) as a web application using a Django backend and a React frontend styled with Bootstrap. The plan delivers a two-project layout: a Django REST API (backend) and a React SPA (frontend). Server-side filtering, paging and sorting are implemented in the backend API; the frontend consumes the API and renders responsive UI components.

Primary approach:

- Backend: Django (LTS) exposing REST endpoints using Django REST Framework (DRF) for books, categories, members and reservations. Implement transactional reservation logic and business rules (3-reservation limit, 14-day due, $5/day late fee).

- Frontend: React 18 with TypeScript (optional) using Bootstrap 5 for styling. Pages mirror the spec: Home, Catalog (BookList), BookDetails, Categories, MyReservations, Admin pages.

- Data: PostgreSQL for production; SQLite supported for local/dev and CI. ORM models follow spec entities (Book, Category, Member, Reservation).

## Technical Context

**Language/Version**: Python 3.11 (development/CI), Node.js 18+ for frontend tooling.  
**Web Framework**: Django 4.2 LTS for backend, Django REST Framework (DRF) for API.  
**Frontend**: React 18 (create-react-app or Vite) with TypeScript recommended; Bootstrap 5 for styling and layout.  
**Primary Dependencies**:

- Backend: Django, djangorestframework, psycopg2-binary (Postgres), django-environ (env/config), pytest-django for tests.

- Frontend: react, react-dom, react-router, axios or fetch wrapper, bootstrap, react-bootstrap (optional), testing-library/react, vitest/jest.
**Storage**: PostgreSQL for production; SQLite for local development and simple CI runs. Use Django migrations for schema management.
**Authentication**: Django's auth system for members and admin; token-based authentication (DRF Token or JWT) for SPA sessions. (Default: DRF Token for initial scope; mark as configurable.)
**Testing**: pytest / pytest-django for backend tests, React Testing Library + Vitest/Jest for frontend tests, and simple end-to-end smoke tests with Playwright or Cypress (optional initial scope).
**Target Platform**: Linux server (Docker container) for production; developer machines (Windows/macOS/Linux). Frontend served as static assets behind CDN or via the Django app in early stages.
**Project Type**: Web application (backend API + SPA frontend).
**Performance Goals**: Page loads < 2s for catalog with up to 1,000 books; API responses for paginated book lists under 200ms p95 on moderate hardware (single small instance) — these are engineering targets to guide caching and query optimization.
**Constraints**: Data consistency for reservations must be transactional; enforce business rules server-side to avoid race conditions. Keep initial scope small (MVP) per constitution principle II.
**Scale/Scope**: Initial MVP supports up to 10k users/month and 10k books; design for straightforward horizontal scaling later.

## Constitution Check

Gate assessment (quick):

- Gate 1 (Specification Complete): PASS — The spec under `specs/003-library-app/spec.md` contains prioritized user stories with acceptance scenarios and measurable success criteria, edge cases and assumptions. No [NEEDS CLARIFICATION] markers present.  
- Gate 2 (Plan Validated - initial): PARTIAL — Technical Context has been populated with sensible defaults (Django/React/Postgres). Any deviations (e.g., JWT vs DRF Token) are recorded as configuration choices; no blockers exist that would prevent Phase 0 research.  
- Gate 3 (Implementation): N/A at plan stage.  
- Gate 4 (Ready for Delivery): N/A.

Conclusion: No constitution violations that block Phase 0 research. Proceed to Phase 0.

## Project Structure

### Documentation (this feature)

```text
specs/[###-feature]/
├── plan.md              # This file (/speckit.plan command output)
├── research.md          # Phase 0 output (/speckit.plan command)
├── data-model.md        # Phase 1 output (/speckit.plan command)
├── quickstart.md        # Phase 1 output (/speckit.plan command)
├── contracts/           # Phase 1 output (/speckit.plan command)
└── tasks.md             # Phase 2 output (/speckit.tasks command - NOT created by /speckit.plan)
```

### Source Code (repository root)
<!--
  ACTION REQUIRED: Replace the placeholder tree below with the concrete layout
  for this feature. Delete unused options and expand the chosen structure with
  real paths (e.g., apps/admin, packages/something). The delivered plan must
  not include Option labels.
-->

```text
# [REMOVE IF UNUSED] Option 1: Single project (DEFAULT)
src/
├── models/
├── services/
├── cli/
└── lib/

tests/
├── contract/
├── integration/
└── unit/

# [REMOVE IF UNUSED] Option 2: Web application (when "frontend" + "backend" detected)
backend/
├── src/
│   ├── models/
│   ├── services/
│   └── api/
└── tests/

frontend/
├── src/
│   ├── components/
│   ├── pages/
│   └── services/
└── tests/

# [REMOVE IF UNUSED] Option 3: Mobile + API (when "iOS/Android" detected)
api/
└── [same as backend above]

ios/ or android/
└── [platform-specific structure: feature modules, UI flows, platform tests]
```

**Structure Decision**: Use Option 2: Web application with separate backend and frontend projects.

Concrete layout (relative to repo root):

``text
backend/                 # Django project
├── manage.py
├── backend/             # Django settings and WSGI/ASGI app
│   ├── settings/
│   ├── urls.py
│   └── wsgi.py
├── apps/
│   ├── books/           # models, serializers, views, urls, tests
│   ├── members/         # member registration, auth, profile
│   ├── reservations/    # reservation business logic and APIs
│   └── admin/           # admin utilities and scripts
└── requirements.txt

frontend/                # React SPA
├── package.json
├── src/
│   ├── components/
│   ├── pages/
│   ├── services/        # API client wrappers
│   └── styles/
└── vite.config.js or react-scripts

tests/
├── backend/             # integration and unit tests for Django (pytest)
└── frontend/            # unit and component tests for React

```

Rationale: clear separation of concerns, allows independent development and deployment of API and frontend, aligns with the requirement for server-side filtering/paging/sorting.

## Complexity Tracking

No constitution violations requiring justification were identified. The chosen architecture (Django + React) follows the constitution principle II (Keep It Simple) by leveraging standard, widely-understood tools and keeping the MVP scope limited.

