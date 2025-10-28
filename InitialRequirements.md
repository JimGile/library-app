# Library Application Initial Requirements

## Initial Requirements

I want to create a library application that allows users to view and reserve books. The application has the following initial requirements:

- As a user, I want to be able to view a home page with general information about the library.
- As a user, I want to be able to view a paged list of all books and be able to search and sort by title, author, or category.
- As a user, I want to be able to view the details of a book, including its title, author, category, description, and availability.
- As a user, I want to be able to view an About page with detailed information about the library.
- As a user, I want to be able to view a list of FAQ's regarding the library's policies and procedures.
- As a user, I want to be able to register as a new member with email and password.
- As a registered member, I want to be able to log in to the system with email and password.
- As a logged-in user, I want to be able to view a paged list of available books and be able to search and sort by title, author, or category.
- As a logged-in user, I want to be able to view the details of a book, including its title, author, category, description, and availability.
- As a logged-in user, I want to be able to reserve a book for a period of 14 days.
- As a logged-in user, I am not able to reserve a book that is already reserved by another user.
- As a logged-in user, I am not able to have more than 3 books reserved at a time.
- As a logged-in user, I want to be able to view a paged list of all of my reservations for sorted by due date descending with a link to return the book.
- As a logged-in user, I want to be able to return a reserved book.
- As a logged-in user who has returned an over due book, I want the system to charge my membership account a late fee of $5 per day.
- As a logged-in user, I want to be able to update my member information.
- As an admin, I want to be able to log in to the system with specific credentials.
- As a logged-in admin, I want to be able to create new books.
- As a logged-in admin, I want to be able to update existing books.
- As a logged-in admin, I want to be able to delete existing books.
- As a logged-in admin, I want to be able to view a paged list of all categories and be able to search and sort by name.
- As a logged-in admin, I want to be able to create new categories.
- As a logged-in admin, I want to be able to update existing categories.
- As a logged-in admin, I want to be able to delete existing categories.
- As a logged-in admin, I want to be able to view a paged list of all members and be able to search and sort by name or email.
- As a logged-in admin, I want to be able to view a paged list of all of my reservations for sorted by due date descending with a link to update the reservation.
- As a logged-in admin, I want to be able to update an existing reservation.

## Data Model

The data model for the library application includes the following entities:

- Book (title, author, category, description, availability)
- Category (name, description)
- Member (name, email, password, membership type, membership start date, membership end date, membership status, membership balance)
- Reservation (book, member, start date, due date, return date, status, late fee)

The data model for the library application includes the following relationships:

- Book belongs to Category
- Member has many Reservations
- Book has many Reservations

The Book and Category entities should be pre-populated with data.
