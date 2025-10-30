namespace LibraryApp.Core.DTOs;

/// <summary>
/// DTO for category information in API responses.
/// </summary>
public class CategoryDto
{
    /// <summary>
    /// Unique identifier for the category.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Category name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Category description.
    /// </summary>
    public string? Description { get; set; }
}