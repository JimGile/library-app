namespace LibraryApp.Core.DTOs;

/// <summary>
/// DTO for book information in API responses.
/// </summary>
public class BookDto
{
    /// <summary>
    /// Unique identifier for the book.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Book title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Book author.
    /// </summary>
    public string Author { get; set; } = string.Empty;

    /// <summary>
    /// Book's category information.
    /// </summary>
    public CategoryDto Category { get; set; } = null!;

    /// <summary>
    /// Book description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Current availability status of the book.
    /// </summary>
    public bool IsAvailable { get; set; }
}