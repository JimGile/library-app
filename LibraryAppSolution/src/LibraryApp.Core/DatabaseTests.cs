using LibraryApp.Core.Data;
using LibraryApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LibraryApp.Core.Tests;

/// <summary>
/// Tests for database functionality.
/// </summary>
public class DatabaseTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly string _dbPath;
    private bool _disposed;

    public DatabaseTests()
    {
        _dbPath = Path.Combine(Path.GetTempPath(), $"library_test_{Guid.NewGuid()}.db");
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite($"Data Source={_dbPath}")
            .Options;

        _context = new ApplicationDbContext(options);
        _context.Database.EnsureCreated();
    }

    [Fact]
    public async Task DatabaseSeeder_SeedsInitialData()
    {
        // Act
        await DatabaseSeeder.SeedAsync(_context);

        // Assert
        var categories = await _context.Categories.ToListAsync();
        Assert.NotEmpty(categories);
        Assert.Contains(categories, c => c.Name == "Fiction");
        Assert.Contains(categories, c => c.Name == "Science Fiction");

        var books = await _context.Books.ToListAsync();
        Assert.NotEmpty(books);
        Assert.Contains(books, b => b.Title == "To Kill a Mockingbird");

        var members = await _context.Members.ToListAsync();
        Assert.NotEmpty(members);
        Assert.Contains(members, m => m.Email == "admin@library.com");
    }

    [Fact]
    public async Task ApplicationDbContext_HasAllDbSets()
    {
        // Assert
        Assert.NotNull(_context.Books);
        Assert.NotNull(_context.Categories);
        Assert.NotNull(_context.Members);
        Assert.NotNull(_context.Reservations);
    }

    [Fact]
    public async Task EntityRelationships_WorkCorrectly()
    {
        // Arrange
        await DatabaseSeeder.SeedAsync(_context);

        // Act
        var book = await _context.Books
            .Include(b => b.Category)
            .FirstOrDefaultAsync(b => b.Title == "To Kill a Mockingbird");

        // Assert
        Assert.NotNull(book);
        Assert.NotNull(book.Category);
        Assert.Equal("Fiction", book.Category.Name);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }

            if (File.Exists(_dbPath))
            {
                File.Delete(_dbPath);
            }

            _disposed = true;
        }
    }
}