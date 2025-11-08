using LibraryApp.Core.DTOs;
using LibraryApp.Core.Interfaces;

namespace LibraryApp.UI.Services;

/// <summary>
/// Service for book-related operations in the UI.
/// </summary>
public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly ICategoryRepository _categoryRepository;

    /// <summary>
    /// Initializes a new instance of the BookService.
    /// </summary>
    /// <param name="bookRepository">The book repository.</param>
    /// <param name="categoryRepository">The category repository.</param>
    public BookService(IBookRepository bookRepository, ICategoryRepository categoryRepository)
    {
        _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    /// <summary>
    /// Gets a paginated list of books with optional filtering and sorting.
    /// </summary>
    /// <param name="page">Page number (1-based).</param>
    /// <param name="pageSize">Number of items per page.</param>
    /// <param name="search">Search term for title, author, or category.</param>
    /// <param name="sortBy">Field to sort by.</param>
    /// <param name="sortOrder">Sort order (asc or desc).</param>
    /// <returns>Paginated result of books.</returns>
    public async Task<PaginatedResult<BookDto>> GetBooksAsync(
        int page = 1,
        int pageSize = 20,
        string? search = null,
        string? sortBy = null,
        string? sortOrder = "asc")
    {
        var result = await _bookRepository.GetBooksAsync(page, pageSize, search, sortBy, sortOrder);

        var bookDtos = result.Items.Select(b => new BookDto
        {
            Id = b.Id,
            Title = b.Title,
            Author = b.Author,
            Description = b.Description ?? string.Empty,
            IsAvailable = b.IsAvailable,
            Category = new CategoryDto
            {
                Id = b.Category.Id,
                Name = b.Category.Name,
                Description = b.Category.Description
            }
        });

        return new PaginatedResult<BookDto>
        {
            Items = bookDtos,
            TotalCount = result.TotalCount,
            Page = result.Page,
            PageSize = result.PageSize,
            TotalPages = result.TotalPages
        };
    }

    /// <summary>
    /// Gets detailed information for a specific book.
    /// </summary>
    /// <param name="id">Book ID.</param>
    /// <returns>Book details.</returns>
    public async Task<BookDto?> GetBookAsync(int id)
    {
        var book = await _bookRepository.GetBookWithDetailsAsync(id);
        if (book == null)
            return null;

        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Description = book.Description ?? string.Empty,
            IsAvailable = book.IsAvailable,
            Category = new CategoryDto
            {
                Id = book.Category.Id,
                Name = book.Category.Name,
                Description = book.Category.Description
            }
        };
    }

    /// <summary>
    /// Gets all categories.
    /// </summary>
    /// <returns>List of categories.</returns>
    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();

        return categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description
        });
    }

    /// <summary>
    /// Gets all books for a specific category.
    /// </summary>
    /// <param name="categoryId">The category id.</param>
    /// <returns>Collection of book DTOs in the category.</returns>
    public async Task<IEnumerable<BookDto>> GetBooksByCategoryAsync(int categoryId)
    {
        var books = await _bookRepository.GetBooksByCategoryAsync(categoryId);

        return books.Select(b => new BookDto
        {
            Id = b.Id,
            Title = b.Title,
            Author = b.Author,
            Description = b.Description ?? string.Empty,
            IsAvailable = b.IsAvailable,
            Category = new CategoryDto
            {
                Id = b.Category.Id,
                Name = b.Category.Name,
                Description = b.Category.Description
            }
        });
    }
}