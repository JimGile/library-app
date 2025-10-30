using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Core.Models;

/// <summary>
/// Represents a category for organizing books.
/// </summary>
public class Category
{
    /// <summary>
    /// Unique identifier for the category.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Category name (required, max 50 characters, unique).
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Category description (optional, max 200 characters).
    /// </summary>
    [StringLength(200)]
    public string? Description { get; set; }

    /// <summary>
    /// Navigation property to the category's books.
    /// </summary>
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}