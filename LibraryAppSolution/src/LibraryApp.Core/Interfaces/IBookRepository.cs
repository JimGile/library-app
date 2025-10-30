using LibraryApp.Core.DTOs;
using LibraryApp.Core.Models;

namespace LibraryApp.Core.Interfaces;

/// <summary>
/// Repository interface for Book entity operations.
/// </summary>
public interface IBookRepository : IRepository<Book>
{
    /// <summary>
    /// Gets a paginated list of books with optional filtering and sorting.
    /// </summary>
    /// <param name="page">Page number (1-based).</param>
    /// <param name="pageSize">Number of items per page.</param>
    /// <param name="search">Search term for title, author, or category.</param>
    /// <param name="sortBy">Field to sort by.</param>
    /// <param name="sortOrder">Sort order (asc or desc).</param>
    /// <returns>Paginated result of books.</returns>
    Task<PaginatedResult<Book>> GetBooksAsync(
        int page = 1,
        int pageSize = 20,
        string? search = null,
        string? sortBy = null,
        string? sortOrder = "asc");

    /// <summary>
    /// Gets all books by category.
    /// </summary>
    /// <param name="categoryId">The category ID.</param>
    /// <returns>A collection of books in the specified category.</returns>
    Task<IEnumerable<Book>> GetBooksByCategoryAsync(int categoryId);

    /// <summary>
    /// Gets all available books (not currently borrowed).
    /// </summary>
    /// <returns>A collection of available books.</returns>
    Task<IEnumerable<Book>> GetAvailableBooksAsync();

    /// <summary>
    /// Searches books by title or author.
    /// </summary>
    /// <param name="searchTerm">The search term.</param>
    /// <returns>A collection of books matching the search term.</returns>
    Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm);

    /// <summary>
    /// Gets a book with its category and reservations.
    /// </summary>
    /// <param name="id">The book ID.</param>
    /// <returns>The book with related data, or null if not found.</returns>
    Task<Book?> GetBookWithDetailsAsync(int id);
}