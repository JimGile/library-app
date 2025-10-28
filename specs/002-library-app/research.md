# Research Findings: Library Application

**Date**: October 27, 2025
**Feature**: 002-library-app
**Purpose**: Resolve technical unknowns identified in plan.md

## Database Choice and Migration Strategy

**Decision**: Use SQLite for development and SQL Server for production with Entity Framework Core migrations.

**Rationale**:

- SQLite provides file-based storage that's perfect for development and testing without external dependencies
- SQL Server offers enterprise-grade performance and features for production deployment
- Entity Framework Core migrations enable seamless schema updates across environments
- Supports the scale requirements (1000+ users, 10k+ books) in production

**Alternatives Considered**:

- PostgreSQL: Excellent open-source option, but SQL Server is more common in .NET ecosystems
- In-memory database: Too limited for production and data persistence requirements
- No database: Not viable for the data model with relationships and user management

**Migration Strategy**:

- Use EF Core code-first migrations for schema versioning
- Automated migration application in production deployments
- Seed data for initial Book and Category entities as specified

## Offline Requirements Scope

**Decision**: Implement basic offline reading capabilities for cached book data, but require online connectivity for all reservation operations.

**Rationale**:

- Blazor Hybrid supports Progressive Web App (PWA) features for caching
- Users can browse books offline after initial load, improving mobile experience
- Reservation operations (core business logic) require real-time validation and updates
- Balances user experience with data integrity requirements
- Aligns with responsive design goals for mobile readiness

**Alternatives Considered**:

- Full offline capability: Would require complex sync mechanisms and conflict resolution, violating YAGNI principle
- No offline support: Poor mobile experience, especially for book browsing
- Limited offline with queue: Too complex for current scope, can be added later if needed

**Implementation Approach**:

- Use Blazor's PWA template features for service worker caching
- Cache book catalog data locally
- Show clear messaging when offline operations are attempted
- Auto-sync when connectivity returns
