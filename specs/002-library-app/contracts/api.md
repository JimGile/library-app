# API Contracts: Library Application

**Date**: October 27, 2025
**Feature**: 002-library-app
**Format**: REST API with JSON responses

## Authentication

- **Type**: JWT Bearer tokens
- **Header**: `Authorization: Bearer {token}`
- **Registration/Login**: Public endpoints
- **Protected**: Member and Admin endpoints require authentication

## Public Endpoints

### Books

#### GET /api/books

List books with optional filtering and pagination.

**Query Parameters**:

- `page` (int, optional): Page number (default: 1)
- `pageSize` (int, optional): Items per page (default: 20, max: 100)
- `search` (string, optional): Search in title, author, category
- `sortBy` (string, optional): Sort field (title, author, category)
- `sortOrder` (string, optional): asc or desc (default: asc)

**Response**: 200 OK

```json
{
  "items": [
    {
      "id": 1,
      "title": "Book Title",
      "author": "Author Name",
      "category": {
        "id": 1,
        "name": "Fiction"
      },
      "description": "Book description",
      "isAvailable": true
    }
  ],
  "totalCount": 150,
  "page": 1,
  "pageSize": 20,
  "totalPages": 8
}
```

#### GET /api/books/{id}

Get detailed book information.

**Path Parameters**:

- `id` (int): Book ID

**Response**: 200 OK

```json
{
  "id": 1,
  "title": "Book Title",
  "author": "Author Name",
  "category": {
    "id": 1,
    "name": "Fiction",
    "description": "Fiction books"
  },
  "description": "Detailed book description",
  "isAvailable": true
}
```

**Response**: 404 Not Found (if book doesn't exist)

### Categories

#### GET /api/categories

List all categories.

**Response**: 200 OK

```json
[
  {
    "id": 1,
    "name": "Fiction",
    "description": "Fiction books"
  }
]
```

### Static Pages

#### GET /api/pages/home

Get home page content.

**Response**: 200 OK

```json
{
  "title": "Welcome to Our Library",
  "content": "Library description and information"
}
```

#### GET /api/pages/about

Get about page content.

**Response**: 200 OK

```json
{
  "title": "About Our Library",
  "content": "Detailed library information"
}
```

#### GET /api/pages/faq

Get FAQ content.

**Response**: 200 OK

```json
{
  "title": "Frequently Asked Questions",
  "faqs": [
    {
      "question": "How do I register?",
      "answer": "Visit the registration page..."
    }
  ]
}
```

## Member Endpoints

### Auth

#### POST /api/auth/register

Register a new member.

**Request**:

```json
{
  "name": "John Doe",
  "email": "john@example.com",
  "password": "securepassword"
}
```

**Response**: 201 Created

```json
{
  "id": 1,
  "name": "John Doe",
  "email": "john@example.com",
  "membershipType": "Standard",
  "membershipStartDate": "2025-10-27T00:00:00Z",
  "membershipStatus": "Active",
  "membershipBalance": 0
}
```

**Response**: 400 Bad Request (validation errors)

#### POST /api/auth/login

Authenticate member.

**Request**:

```json
{
  "email": "john@example.com",
  "password": "securepassword"
}
```

**Response**: 200 OK

```json
{
  "token": "jwt.token.here",
  "member": {
    "id": 1,
    "name": "John Doe",
    "email": "john@example.com"
  }
}
```

### Reservations

#### GET /api/reservations

Get member's reservations.

**Query Parameters**:

- `page` (int, optional): Page number
- `pageSize` (int, optional): Items per page

**Response**: 200 OK

```json
{
  "items": [
    {
      "id": 1,
      "book": {
        "id": 1,
        "title": "Book Title",
        "author": "Author Name"
      },
      "startDate": "2025-10-27T00:00:00Z",
      "dueDate": "2025-11-10T00:00:00Z",
      "status": "Active",
      "lateFee": 0
    }
  ],
  "totalCount": 2
}
```

#### POST /api/reservations

Create a new reservation.

**Request**:

```json
{
  "bookId": 1
}
```

**Response**: 201 Created

```json
{
  "id": 1,
  "bookId": 1,
  "memberId": 1,
  "startDate": "2025-10-27T00:00:00Z",
  "dueDate": "2025-11-10T00:00:00Z",
  "status": "Active"
}
```

**Response**: 400 Bad Request (book not available, max reservations reached)

#### PUT /api/reservations/{id}/return

Return a reserved book.

**Path Parameters**:

- `id` (int): Reservation ID

**Response**: 200 OK

```json
{
  "id": 1,
  "returnDate": "2025-10-28T00:00:00Z",
  "status": "Returned",
  "lateFee": 0
}
```

### Profile

#### PUT /api/members/{id}

Update member information.

**Path Parameters**:

- `id` (int): Member ID (must match authenticated user)

**Request**:

```json
{
  "name": "Updated Name",
  "email": "newemail@example.com"
}
```

**Response**: 200 OK

```json
{
  "id": 1,
  "name": "Updated Name",
  "email": "newemail@example.com"
}
```

## Admin Endpoints

### Books Management

#### GET /api/admin/books

List all books for admin (with additional fields).

**Query Parameters**: Same as public books endpoint

**Response**: 200 OK (includes all book fields)

#### POST /api/books

Create a new book.

**Request**:

```json
{
  "title": "New Book",
  "author": "Author Name",
  "categoryId": 1,
  "description": "Book description",
  "isAvailable": true
}
```

**Response**: 201 Created (book object)

#### PUT /api/books/{id}

Update an existing book.

**Request**: Same as create, all fields optional

**Response**: 200 OK (updated book object)

#### DELETE /api/books/{id}

Delete a book.

**Response**: 204 No Content

### Categories Management

#### GET /api/admin/categories

List all categories for admin.

**Response**: 200 OK (category array)

#### POST /api/categories

Create a new category.

**Request**:

```json
{
  "name": "New Category",
  "description": "Category description"
}
```

**Response**: 201 Created (category object)

#### PUT /api/categories/{id}

Update a category.

**Response**: 200 OK (updated category object)

#### DELETE /api/categories/{id}

Delete a category.

**Response**: 204 No Content

### Members Management

#### GET /api/admin/members

List all members.

**Query Parameters**:

- `page`, `pageSize`, `search` (by name/email), `sortBy`, `sortOrder`

**Response**: 200 OK

```json
{
  "items": [
    {
      "id": 1,
      "name": "John Doe",
      "email": "john@example.com",
      "membershipType": "Standard",
      "membershipStartDate": "2025-10-27T00:00:00Z",
      "membershipEndDate": null,
      "membershipStatus": "Active",
      "membershipBalance": 0
    }
  ],
  "totalCount": 50
}
```

### Reservations Management

#### GET /api/admin/reservations

List all reservations.

**Query Parameters**: page, pageSize, sortBy (dueDate), sortOrder

**Response**: 200 OK (reservations array with member details)

#### PUT /api/admin/reservations/{id}

Update a reservation.

**Request**:

```json
{
  "status": "Returned",
  "returnDate": "2025-10-28T00:00:00Z"
}
```

**Response**: 200 OK (updated reservation object)

## Error Responses

All endpoints may return:

**401 Unauthorized**: Invalid or missing authentication
**403 Forbidden**: Insufficient permissions
**404 Not Found**: Resource not found
**500 Internal Server Error**: Server error

Error format:

```json
{
  "error": "Error message",
  "details": "Additional information"
}
```

