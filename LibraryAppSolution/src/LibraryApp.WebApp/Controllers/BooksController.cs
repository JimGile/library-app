using LibraryApp.Core.DTOs;
using LibraryApp.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.WebApp.Controllers;

/// <summary>
/// Controller for book-related operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookRepository _bookRepository;

    /// <summary>
    /// Initializes a new instance of the BooksController.
    /// </summary>
    /// <param name="bookRepository">The book repository.</param>
    public BooksController(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
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
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResult<BookDto>), 200)]
    public async Task<IActionResult> GetBooks(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? sortOrder = "asc")
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 20;

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

        var response = new PaginatedResult<BookDto>
        {
            Items = bookDtos,
            TotalCount = result.TotalCount,
            Page = result.Page,
            PageSize = result.PageSize,
            TotalPages = result.TotalPages
        };

        return Ok(response);
    }

    /// <summary>
    /// Gets detailed information for a specific book.
    /// </summary>
    /// <param name="id">Book ID.</param>
    /// <returns>Book details.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BookDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetBook(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null)
            return NotFound();

        var bookDto = new BookDto
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

        return Ok(bookDto);
    }
}