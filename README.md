# Library App (scaffold)

This repository contains a .NET-based library app and a proposed Django+React feature implementation in `backend/` and `frontend/` for a cross-stack prototype.

Quickstart (Phase 1 scaffolding):

- Backend (Python/Django):

  - Create a virtualenv and install requirements:

```powershell
python -m venv .venv
.\.venv\Scripts\Activate.ps1
pip install -r backend/requirements.txt
```

  - Run migrations and start the dev server:

```powershell
python backend\manage.py migrate
python backend\manage.py runserver
```

- Frontend (React/Vite):

  - From `frontend/` run:

```powershell
cd frontend
npm install
npm run dev
```

More detailed instructions will be provided in `specs/003-library-app/quickstart.md` once Phase 2 is implemented.
