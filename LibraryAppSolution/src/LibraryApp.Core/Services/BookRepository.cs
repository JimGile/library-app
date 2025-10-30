using LibraryApp.Core.Data;
using LibraryApp.Core.DTOs;
using LibraryApp.Core.Interfaces;
using LibraryApp.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Core.Services;

/// <summary>
/// Repository for book data operations.
/// </summary>
public class BookRepository : IBookRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the BookRepository.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public BookRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
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
    public async Task<PaginatedResult<Book>> GetBooksAsync(
        int page = 1,
        int pageSize = 20,
        string? search = null,
        string? sortBy = null,
        string? sortOrder = "asc")
    {
        var query = _context.Books
            .Include(b => b.Category)
            .AsQueryable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(b =>
                b.Title.Contains(search) ||
                b.Author.Contains(search) ||
                b.Category.Name.Contains(search));
        }

        // Apply sorting
        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            query = sortBy.ToLower() switch
            {
                "title" => sortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(b => b.Title)
                    : query.OrderBy(b => b.Title),
                "author" => sortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(b => b.Author)
                    : query.OrderBy(b => b.Author),
                "category" => sortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(b => b.Category.Name)
                    : query.OrderBy(b => b.Category.Name),
                _ => query.OrderBy(b => b.Id)
            };
        }
        else
        {
            query = query.OrderBy(b => b.Id);
        }

        // Get total count
        var totalCount = await query.CountAsync();

        // Apply pagination
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PaginatedResult<Book>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages
        };
    }

    /// <summary>
    /// Gets all books.
    /// </summary>
    /// <returns>A collection of all books.</returns>
    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _context.Books
            .Include(b => b.Category)
            .ToListAsync();
    }

    /// <summary>
    /// Gets a book by ID.
    /// </summary>
    /// <param name="id">The book ID.</param>
    /// <returns>The book, or null if not found.</returns>
    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _context.Books
            .Include(b => b.Category)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    /// <summary>
    /// Adds a new book.
    /// </summary>
    /// <param name="book">The book to add.</param>
    /// <returns>The added book.</returns>
    public async Task<Book> AddAsync(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    /// <summary>
    /// Updates an existing book.
    /// </summary>
    /// <param name="book">The book to update.</param>
    /// <returns>The updated book.</returns>
    public async Task<Book> UpdateAsync(Book book)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
        return book;
    }

    /// <summary>
    /// Deletes a book.
    /// </summary>
    /// <param name="id">The book ID to delete.</param>
    /// <returns>True if deleted, false if not found.</returns>
    public async Task<bool> DeleteAsync(int id)
    {
        var book = await GetByIdAsync(id);
        if (book == null)
            return false;

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Checks if a book exists by ID.
    /// </summary>
    /// <param name="id">The book ID.</param>
    /// <returns>True if exists, false otherwise.</returns>
    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Books.AnyAsync(b => b.Id == id);
    }

    /// <summary>
    /// Gets all books by category.
    /// </summary>
    /// <param name="categoryId">The category ID.</param>
    /// <returns>A collection of books in the specified category.</returns>
    public async Task<IEnumerable<Book>> GetBooksByCategoryAsync(int categoryId)
    {
        return await _context.Books
            .Include(b => b.Category)
            .Where(b => b.CategoryId == categoryId)
            .ToListAsync();
    }

    /// <summary>
    /// Gets all available books (not currently borrowed).
    /// </summary>
    /// <returns>A collection of available books.</returns>
    public async Task<IEnumerable<Book>> GetAvailableBooksAsync()
    {
        return await _context.Books
            .Include(b => b.Category)
            .Where(b => b.IsAvailable)
            .ToListAsync();
    }

    /// <summary>
    /// Searches books by title or author.
    /// </summary>
    /// <param name="searchTerm">The search term.</param>
    /// <returns>A collection of books matching the search term.</returns>
    public async Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm)
    {
        return await _context.Books
            .Include(b => b.Category)
            .Where(b => b.Title.Contains(searchTerm) || b.Author.Contains(searchTerm))
            .ToListAsync();
    }

    /// <summary>
    /// Gets a book with its category and reservations.
    /// </summary>
    /// <param name="id">The book ID.</param>
    /// <returns>The book with related data, or null if not found.</returns>
    public async Task<Book?> GetBookWithDetailsAsync(int id)
    {
        return await _context.Books
            .Include(b => b.Category)
            .Include(b => b.Reservations)
            .FirstOrDefaultAsync(b => b.Id == id);
    }
}