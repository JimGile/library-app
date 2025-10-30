using LibraryApp.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LibraryApp.Core.Data;

/// <summary>
/// Design-time factory for creating ApplicationDbContext instances.
/// Used by EF Core tools for migrations and other design-time operations.
/// </summary>
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    /// <summary>
    /// Creates a new instance of ApplicationDbContext for design-time operations.
    /// </summary>
    /// <param name="args">Command line arguments (not used).</param>
    /// <returns>A configured ApplicationDbContext instance.</returns>
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        // Use SQLite for development/migrations
        optionsBuilder.UseSqlite(DatabaseConfiguration.DevelopmentConnectionString);

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}