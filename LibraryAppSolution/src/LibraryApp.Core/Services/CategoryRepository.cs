using LibraryApp.Core.Data;
using LibraryApp.Core.Interfaces;
using LibraryApp.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Core.Services;

/// <summary>
/// Repository for category data operations.
/// </summary>
public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the CategoryRepository.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Gets all categories.
    /// </summary>
    /// <returns>A collection of all categories.</returns>
    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    /// <summary>
    /// Gets a category by ID.
    /// </summary>
    /// <param name="id">The category ID.</param>
    /// <returns>The category, or null if not found.</returns>
    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories.FindAsync(id);
    }

    /// <summary>
    /// Adds a new category.
    /// </summary>
    /// <param name="category">The category to add.</param>
    /// <returns>The added category.</returns>
    public async Task<Category> AddAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    /// <summary>
    /// Updates an existing category.
    /// </summary>
    /// <param name="category">The category to update.</param>
    /// <returns>The updated category.</returns>
    public async Task<Category> UpdateAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
        return category;
    }

    /// <summary>
    /// Deletes a category.
    /// </summary>
    /// <param name="id">The category ID to delete.</param>
    /// <returns>True if deleted, false if not found.</returns>
    public async Task<bool> DeleteAsync(int id)
    {
        var category = await GetByIdAsync(id);
        if (category == null)
            return false;

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Checks if a category exists by ID.
    /// </summary>
    /// <param name="id">The category ID.</param>
    /// <returns>True if exists, false otherwise.</returns>
    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Categories.AnyAsync(c => c.Id == id);
    }

    /// <summary>
    /// Gets a category by name.
    /// </summary>
    /// <param name="name">The category name.</param>
    /// <returns>The category, or null if not found.</returns>
    public async Task<Category?> GetByNameAsync(string name)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
    }

    /// <summary>
    /// Gets all categories with their book counts.
    /// </summary>
    /// <returns>A collection of categories with book counts.</returns>
    public async Task<IEnumerable<Category>> GetCategoriesWithBookCountAsync()
    {
        return await _context.Categories
            .Include(c => c.Books)
            .ToListAsync();
    }
}