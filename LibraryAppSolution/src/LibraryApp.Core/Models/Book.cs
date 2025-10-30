using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApp.Core.Models;

/// <summary>
/// Represents a book available in the library catalog.
/// </summary>
public class Book
{
    /// <summary>
    /// Unique identifier for the book.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Book title (required, max 200 characters).
    /// </summary>
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Book author (required, max 100 characters).
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Author { get; set; } = string.Empty;

    /// <summary>
    /// Book description (optional, max 1000 characters).
    /// </summary>
    [StringLength(1000)]
    public string? Description { get; set; }

    /// <summary>
    /// Current availability status of the book.
    /// </summary>
    public bool IsAvailable { get; set; } = true;

    /// <summary>
    /// Foreign key reference to the book's category.
    /// </summary>
    [ForeignKey("Category")]
    public int CategoryId { get; set; }

    /// <summary>
    /// Navigation property to the book's category.
    /// </summary>
    public virtual Category Category { get; set; } = null!;

    /// <summary>
    /// Navigation property to the book's reservations.
    /// </summary>
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}