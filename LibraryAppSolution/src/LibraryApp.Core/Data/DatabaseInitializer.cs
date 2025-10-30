using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LibraryApp.Core.Data;

/// <summary>
/// Provides database initialization functionality.
/// </summary>
public static class DatabaseInitializer
{
    /// <summary>
    /// Initializes the database by applying migrations and seeding data.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <param name="logger">Optional logger for logging operations.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task InitializeAsync(ApplicationDbContext context, ILogger? logger = null)
    {
        try
        {
            logger?.LogInformation("Initializing database...");

            // Apply any pending migrations
            await context.Database.MigrateAsync();
            logger?.LogInformation("Migrations applied successfully.");

            // Seed the database
            await DatabaseSeeder.SeedAsync(context);
            logger?.LogInformation("Database seeded successfully.");

            logger?.LogInformation("Database initialization completed.");
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }

    /// <summary>
    /// Ensures the database is created (for development/testing scenarios).
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <param name="logger">Optional logger for logging operations.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task EnsureCreatedAsync(ApplicationDbContext context, ILogger? logger = null)
    {
        try
        {
            logger?.LogInformation("Ensuring database is created...");

            await context.Database.EnsureCreatedAsync();
            logger?.LogInformation("Database creation ensured.");

            await DatabaseSeeder.SeedAsync(context);
            logger?.LogInformation("Database seeded successfully.");
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "An error occurred while ensuring database creation.");
            throw;
        }
    }
}