# Data Model: Library Application

**Date**: October 27, 2025
**Feature**: 002-library-app
**Purpose**: Define the data entities, relationships, and validation rules for the library application

## Entities

### Book

Represents a book available in the library catalog.

**Fields**:

- `Id` (int, Primary Key): Unique identifier
- `Title` (string, Required, Max 200): Book title
- `Author` (string, Required, Max 100): Book author
- `Description` (string, Optional, Max 1000): Book description
- `IsAvailable` (bool): Current availability status
- `CategoryId` (int, Foreign Key): Reference to Category

**Relationships**:

- Belongs to Category (many-to-one)
- Has many Reservations (one-to-many)

**Validation Rules**:

- Title and Author are required
- Title max length 200 characters
- Author max length 100 characters
- Description max length 1000 characters (optional)

### Category

Represents a category for organizing books.

**Fields**:

- `Id` (int, Primary Key): Unique identifier
- `Name` (string, Required, Max 50, Unique): Category name
- `Description` (string, Optional, Max 200): Category description

**Relationships**:

- Has many Books (one-to-many)

**Validation Rules**:

- Name is required and unique
- Name max length 50 characters
- Description max length 200 characters (optional)

### Member

Represents a library member who can borrow books.

**Fields**:

- `Id` (int, Primary Key): Unique identifier
- `Name` (string, Required, Max 100): Member full name
- `Email` (string, Required, Max 100, Unique): Member email address
- `PasswordHash` (string, Required): Hashed password
- `MembershipType` (string, Required): Type of membership
- `MembershipStartDate` (DateTime, Required): When membership started
- `MembershipEndDate` (DateTime, Optional): When membership expires
- `MembershipStatus` (string, Required): Active/Inactive status
- `MembershipBalance` (decimal, Required): Current balance (positive for fees owed)

**Relationships**:

- Has many Reservations (one-to-many)

**Validation Rules**:

- Name, Email, PasswordHash are required
- Email must be valid format and unique
- Name max length 100 characters
- Email max length 100 characters
- MembershipBalance defaults to 0
- MembershipStatus must be "Active" or "Inactive"

### Reservation

Represents a book reservation/loan by a member.

**Fields**:

- `Id` (int, Primary Key): Unique identifier
- `BookId` (int, Foreign Key): Reference to Book
- `MemberId` (int, Foreign Key): Reference to Member
- `StartDate` (DateTime, Required): When reservation started
- `DueDate` (DateTime, Required): When book is due (StartDate + 14 days)
- `ReturnDate` (DateTime, Optional): When book was returned
- `Status` (string, Required): Reservation status
- `LateFee` (decimal, Optional): Accumulated late fee

**Relationships**:

- Belongs to Book (many-to-one)
- Belongs to Member (many-to-one)

**Validation Rules**:

- StartDate and DueDate are required
- DueDate must be exactly 14 days after StartDate
- Status must be "Active", "Returned", or "Overdue"
- LateFee calculated as $5 per day overdue when returned
- Cannot have overlapping active reservations for same book
- Member cannot have more than 3 active reservations

## State Transitions

### Reservation Status

- **Active**: Initial state when reservation is created
  - Transitions to: Returned (when book returned on time), Overdue (when due date passes)
- **Returned**: Final state when book is returned
  - No further transitions
- **Overdue**: When due date passes without return
  - Transitions to: Returned (when book finally returned, late fee applied)

## Business Rules

1. **Reservation Limits**:
   - Member cannot have more than 3 active reservations simultaneously
   - Cannot reserve a book that is already reserved by another member

2. **Late Fees**:
   - Calculated as $5 per day overdue
   - Applied to MembershipBalance when book is returned overdue
   - Only calculated on actual return date

3. **Availability**:
   - Book.IsAvailable = true when no active reservations
   - Book.IsAvailable = false when has active reservation

4. **Membership**:
   - Only active members can create reservations
   - Membership balance affects borrowing privileges (future enhancement)

## Seed Data Requirements

- **Categories**: Pre-populate with common book categories (Fiction, Non-Fiction, Science, History, etc.)
- **Books**: Pre-populate with sample books across categories
- **Members**: No seed data (created via registration)
- **Reservations**: No seed data (created via app usage)
