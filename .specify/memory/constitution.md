<!--
Sync Impact Report
==================
Version change: none → 1.0.0
Type: Initial creation (MINOR - new governance document)
Rationale: First formal constitution for personal development projects based on existing
personal-constitution.md principles plus additional spec-driven development principles.

Added sections:
- All core principles (5 principles total)
- Development workflow guidelines
- Quality gates section
- Governance rules

Templates requiring updates:
- ✅ plan-template.md (Constitution Check section already present)
- ✅ spec-template.md (Requirements section aligns with principle I)
- ✅ tasks-template.md (Task organization reflects principle IV modularity)
- ✅ Commands (no agent-specific updates needed - generic guidance maintained)

Follow-up TODOs: None
-->

# Spec Kit Personal Development Constitution

## Core Principles

### I. The Spec is the Source of Truth

The specification document (`spec.md`) is the authoritative definition of what the feature
MUST accomplish and why. All implementation decisions, code changes, and refactoring efforts
MUST originate from updates to the specification, never the reverse.

**Rules**:

- New ideas and changes MUST first update the spec
- Spec changes MUST be followed by plan updates
- Plan updates MUST be followed by task updates
- Code MUST be implemented only after tasks are defined
- The sequence is immutable: spec → plan → tasks → code

**Rationale**: Keeping the specification synchronized with implementation intent prevents
drift between what was intended and what was built. This creates a living document that
serves as both design artifact and historical record, making onboarding, debugging, and
future enhancements dramatically faster and more reliable.

### II. Keep It Simple (YAGNI)

Build only what is needed for the current specification. Features, abstractions, and
architectural complexity for hypothetical future requirements are explicitly prohibited
unless documented in the spec as current requirements.

**Rules**:

- Implement ONLY features defined in the current spec
- Add abstractions ONLY when multiple concrete uses exist
- Reject speculative "what if" features
- Choose the simplest solution that satisfies requirements
- Complexity MUST be justified in writing before implementation

**Rationale**: Premature optimization and speculative features increase maintenance burden,
extend development time, and create code that is harder to understand. The simplest
solution is the best starting point and can be evolved when real needs emerge.

### III. Test for Confidence

Write tests for critical logic to create a safety net for refactoring and feature additions.
The goal is pragmatic confidence, not arbitrary coverage metrics.

**Rules**:

- Write tests for business logic and critical paths
- Test MUST prove the code works as specified
- Focus on integration and contract tests over unit tests
- Coverage targets are guidance, not mandates (aim for critical paths)
- Tests MUST be maintainable and provide clear failure messages

**Rationale**: A good test suite proves the implementation matches the specification,
catches regressions early, and enables fearless refactoring. This accelerates future
development and reduces debugging time dramatically.

### IV. Build in Modules

Structure code in small, independent, loosely-coupled components. Every function, class,
and service MUST have a single, well-defined responsibility.

**Rules**:

- Each module MUST have one clear responsibility
- Modules MUST be independently testable
- Dependencies between modules MUST be explicit and minimal
- Shared code MUST be extracted only after 3+ uses (rule of three)
- Module boundaries MUST align with domain concepts

**Rationale**: Modular architecture makes code easier to understand, test, debug, and reuse.
Single responsibility modules can be modified independently without cascading changes
throughout the codebase.

### V. Clean Code Standards

Write code optimized for human comprehension with consistent style and robust error handling.

**Rules**:

- Use clear, descriptive names for all identifiers
- Enforce consistent style via automated formatters (e.g., prettier, black, rustfmt)
- Handle errors explicitly - no silent failures
- Add comments only for "why", never for "what" (code should be self-documenting)
- Maintain `.gitignore` appropriate for the tech stack

**Rationale**: Code is read far more often than it is written. Readable, consistent code
reduces cognitive load, accelerates onboarding, and prevents bugs through clarity.

## Development Workflow

### Specification Phase

1. Use `/speckit.constitution` to establish or update governing principles
2. Use `/speckit.specify` to define WHAT you want to build and WHY
3. Use `/speckit.clarify` to resolve underspecified requirements BEFORE planning
4. Validate acceptance criteria are testable and complete

### Planning Phase

1. Use `/speckit.plan` to define HOW (tech stack, architecture, dependencies)
2. Ensure Technical Context section has NO "NEEDS CLARIFICATION" markers
3. Complete Constitution Check and document any required complexity justifications
4. Have agent validate the plan for completeness before proceeding

### Task Phase

1. Use `/speckit.tasks` to break plan into ordered, actionable tasks
2. Ensure tasks are grouped by user story for independent delivery
3. Verify foundational tasks are identified and will block story work appropriately
4. Confirm parallel execution markers `[P]` are used where dependencies allow

### Implementation Phase

1. Use `/speckit.implement` to execute tasks in dependency order
2. Write tests FIRST if TDD is specified (tests should fail before implementation)
3. Commit after each task or logical group of tasks
4. Validate at checkpoints that user stories work independently
5. Stop and validate at any story boundary before proceeding to next priority

### Quality Phase

1. Verify all acceptance criteria from spec are met
2. Run all tests and ensure they pass
3. Check error handling for graceful degradation
4. Validate against constitution principles (especially I-V)
5. Test the quickstart.md instructions to ensure reproducibility

## Quality Gates

### Gate 1: Specification Complete

- [ ] All user stories have clear acceptance scenarios
- [ ] Functional requirements are specific and testable
- [ ] Success criteria are measurable
- [ ] Edge cases are identified
- [ ] NO "NEEDS CLARIFICATION" markers remain

### Gate 2: Plan Validated

- [ ] Constitution Check passes or violations are justified
- [ ] Technical Context has no unclear items
- [ ] Project structure is documented and appropriate
- [ ] Dependencies and versions are specified
- [ ] Agent has validated plan completeness

### Gate 3: Implementation Complete

- [ ] All tasks marked complete
- [ ] Tests pass (if tests were requested)
- [ ] Application runs via documented commands
- [ ] No silent error handling or unhandled exceptions
- [ ] Code follows clean code standards (Principle V)

### Gate 4: Ready for Delivery

- [ ] All acceptance criteria met
- [ ] quickstart.md validated by following from scratch
- [ ] Constitution principles verified (especially I-V)
- [ ] Documentation updated (README, comments where needed)
- [ ] `.gitignore` appropriate for tech stack

## Governance

### Amendment Procedure

1. Identify principle addition, modification, or removal
2. Document rationale and impact on existing projects
3. Update constitution with versioning (see below)
4. Update template files (plan, spec, tasks, commands) to align
5. Commit with message: `docs: amend constitution to vX.Y.Z (description)`

### Versioning Policy

Constitution version follows semantic versioning:

- **MAJOR**: Backward-incompatible changes (principle removal, redefinition)
- **MINOR**: New principle added or material expansion of guidance
- **PATCH**: Clarifications, wording improvements, typo fixes

### Compliance Review

- All specifications MUST reference this constitution
- All plans MUST include Constitution Check section
- Agents MUST verify compliance before implementation
- Violations MUST be documented and justified in Complexity Tracking

### Runtime Guidance

During active development, agents SHOULD reference:

- This constitution for non-negotiable principles
- `plan.md` for feature-specific technical decisions
- `spec.md` for requirements and acceptance criteria
- `tasks.md` for execution sequence and dependencies

**Version**: 1.0.0 | **Ratified**: 2025-10-25 | **Last Amended**: 2025-10-25
