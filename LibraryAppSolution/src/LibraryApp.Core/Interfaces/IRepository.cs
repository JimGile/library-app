namespace LibraryApp.Core.Interfaces;

/// <summary>
/// Generic repository interface for basic CRUD operations.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Gets all entities.
    /// </summary>
    /// <returns>A collection of all entities.</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Gets an entity by its ID.
    /// </summary>
    /// <param name="id">The entity ID.</param>
    /// <returns>The entity, or null if not found.</returns>
    Task<T?> GetByIdAsync(int id);

    /// <summary>
    /// Adds a new entity.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>The added entity.</returns>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>The updated entity.</returns>
    Task<T> UpdateAsync(T entity);

    /// <summary>
    /// Deletes an entity by its ID.
    /// </summary>
    /// <param name="id">The entity ID to delete.</param>
    /// <returns>True if deleted, false if not found.</returns>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Checks if an entity exists by its ID.
    /// </summary>
    /// <param name="id">The entity ID.</param>
    /// <returns>True if exists, false otherwise.</returns>
    Task<bool> ExistsAsync(int id);
}