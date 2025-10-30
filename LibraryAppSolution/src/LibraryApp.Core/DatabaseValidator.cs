using LibraryApp.Core.Data;
using LibraryApp.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Core;

/// <summary>
/// Simple database validation program.
/// </summary>
public static class DatabaseValidator
{
    public static async Task ValidateAsync()
    {
        Console.WriteLine("Validating database setup...");

        // Create in-memory database for testing
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;

        using var context = new ApplicationDbContext(options);
        await context.Database.EnsureCreatedAsync();

        Console.WriteLine("Database created successfully.");

        // Test seeding
        await DatabaseSeeder.SeedAsync(context);
        Console.WriteLine("Database seeded successfully.");

        // Validate data
        var categories = await context.Categories.ToListAsync();
        Console.WriteLine($"Categories seeded: {categories.Count}");
        foreach (var category in categories)
        {
            Console.WriteLine($"  - {category.Name}: {category.Description}");
        }

        var books = await context.Books.Include(b => b.Category).ToListAsync();
        Console.WriteLine($"Books seeded: {books.Count}");
        foreach (var book in books.Take(3)) // Show first 3
        {
            Console.WriteLine($"  - {book.Title} by {book.Author} ({book.Category?.Name})");
        }

        var members = await context.Members.ToListAsync();
        Console.WriteLine($"Members seeded: {members.Count}");
        foreach (var member in members)
        {
            Console.WriteLine($"  - {member.Name} ({member.Email})");
        }

        Console.WriteLine("Database validation completed successfully!");
    }
}