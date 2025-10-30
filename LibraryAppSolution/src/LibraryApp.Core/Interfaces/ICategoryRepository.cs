using LibraryApp.Core.Models;

namespace LibraryApp.Core.Interfaces;

/// <summary>
/// Repository interface for Category entity operations.
/// </summary>
public interface ICategoryRepository : IRepository<Category>
{
    /// <summary>
    /// Gets a category by name.
    /// </summary>
    /// <param name="name">The category name.</param>
    /// <returns>The category, or null if not found.</returns>
    Task<Category?> GetByNameAsync(string name);

    /// <summary>
    /// Gets all categories with their book counts.
    /// </summary>
    /// <returns>A collection of categories with book counts.</returns>
    Task<IEnumerable<Category>> GetCategoriesWithBookCountAsync();
}