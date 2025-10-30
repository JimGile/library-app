using LibraryApp.Core.DTOs;

namespace LibraryApp.Core.Interfaces;

/// <summary>
/// Service interface for book-related API operations.
/// </summary>
public interface IBookService
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
    Task<PaginatedResult<BookDto>> GetBooksAsync(
        int page = 1,
        int pageSize = 20,
        string? search = null,
        string? sortBy = null,
        string? sortOrder = "asc");

    /// <summary>
    /// Gets detailed information for a specific book.
    /// </summary>
    /// <param name="id">Book ID.</param>
    /// <returns>Book details.</returns>
    Task<BookDto?> GetBookAsync(int id);

    /// <summary>
    /// Gets all categories.
    /// </summary>
    /// <returns>List of categories.</returns>
    Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
}