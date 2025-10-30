using LibraryApp.Core.Data;
using LibraryApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace LibraryApp.Core.Data;

/// <summary>
/// Provides database seeding functionality for initial data.
/// </summary>
public static class DatabaseSeeder
{
    /// <summary>
    /// Seeds the database with initial data if it's empty.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await SeedCategoriesAsync(context);
        await SeedBooksAsync(context);
        await SeedAdminUserAsync(context);

        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Seeds initial book categories.
    /// </summary>
    /// <param name="context">The application database context.</param>
    private static async Task SeedCategoriesAsync(ApplicationDbContext context)
    {
        if (await context.Categories.AnyAsync())
            return;

        var categories = new[]
        {
            new Category { Name = "Fiction", Description = "Fictional stories and novels" },
            new Category { Name = "Non-Fiction", Description = "Educational and informative books" },
            new Category { Name = "Science Fiction", Description = "Speculative fiction based on imagined future scientific or technological advances" },
            new Category { Name = "Mystery", Description = "Books involving crime, detective work, and suspense" },
            new Category { Name = "Biography", Description = "Accounts of people's lives written by others" },
            new Category { Name = "History", Description = "Books about historical events and periods" },
            new Category { Name = "Technology", Description = "Books about computers, programming, and technology" },
            new Category { Name = "Self-Help", Description = "Books designed to help readers solve personal problems" }
        };

        await context.Categories.AddRangeAsync(categories);
    }

    /// <summary>
    /// Seeds initial books.
    /// </summary>
    /// <param name="context">The application database context.</param>
    private static async Task SeedBooksAsync(ApplicationDbContext context)
    {
        if (await context.Books.AnyAsync())
            return;

        var fictionCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Fiction");
        var sciFiCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Science Fiction");
        var mysteryCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Mystery");
        var techCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Technology");

        if (fictionCategory == null || sciFiCategory == null || mysteryCategory == null || techCategory == null)
            return;

        var books = new[]
        {
            new Book
            {
                Title = "To Kill a Mockingbird",
                Author = "Harper Lee",
                Description = "A classic novel about racial injustice and childhood innocence in the American South.",
                IsAvailable = true,
                CategoryId = fictionCategory.Id
            },
            new Book
            {
                Title = "1984",
                Author = "George Orwell",
                Description = "A dystopian novel about totalitarianism and surveillance.",
                IsAvailable = true,
                CategoryId = sciFiCategory.Id
            },
            new Book
            {
                Title = "The Great Gatsby",
                Author = "F. Scott Fitzgerald",
                Description = "A story of the Jazz Age and the American Dream.",
                IsAvailable = true,
                CategoryId = fictionCategory.Id
            },
            new Book
            {
                Title = "Dune",
                Author = "Frank Herbert",
                Description = "A science fiction epic set on the desert planet Arrakis.",
                IsAvailable = true,
                CategoryId = sciFiCategory.Id
            },
            new Book
            {
                Title = "The Hound of the Baskervilles",
                Author = "Arthur Conan Doyle",
                Description = "A Sherlock Holmes mystery involving a legendary hound.",
                IsAvailable = true,
                CategoryId = mysteryCategory.Id
            },
            new Book
            {
                Title = "Clean Code",
                Author = "Robert C. Martin",
                Description = "A handbook of agile software craftsmanship.",
                IsAvailable = true,
                CategoryId = techCategory.Id
            }
        };

        await context.Books.AddRangeAsync(books);
    }

    /// <summary>
    /// Seeds an admin user.
    /// </summary>
    /// <param name="context">The application database context.</param>
    private static async Task SeedAdminUserAsync(ApplicationDbContext context)
    {
        if (await context.Members.AnyAsync())
            return;

        var adminUser = new Member
        {
            Name = "Library Admin",
            Email = "admin@library.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            MembershipType = "Admin",
            MembershipStartDate = DateTime.UtcNow,
            MembershipStatus = "Active",
            MembershipBalance = 0
        };

        await context.Members.AddAsync(adminUser);
    }
}