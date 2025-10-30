using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApp.Core.Models;

/// <summary>
/// Represents a book reservation by a library member.
/// </summary>
public class Reservation
{
    /// <summary>
    /// Unique identifier for the reservation.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Foreign key to the book being reserved.
    /// </summary>
    [Required]
    public int BookId { get; set; }

    /// <summary>
    /// Foreign key to the member making the reservation.
    /// </summary>
    [Required]
    public int MemberId { get; set; }

    /// <summary>
    /// Date when the reservation started (book was borrowed).
    /// </summary>
    [Required]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Date when the book is due to be returned (StartDate + 14 days).
    /// </summary>
    [Required]
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Date when the book was actually returned (null if not returned).
    /// </summary>
    public DateTime? ReturnDate { get; set; }

    /// <summary>
    /// Current status of the reservation ("Active", "Returned", "Overdue").
    /// </summary>
    [Required]
    [StringLength(20)]
    public string Status { get; set; } = "Active";

    /// <summary>
    /// Accumulated late fee ($5 per day overdue).
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal LateFee { get; set; } = 0;

    /// <summary>
    /// Navigation property to the reserved book.
    /// </summary>
    [ForeignKey("BookId")]
    public virtual Book Book { get; set; } = null!;

    /// <summary>
    /// Navigation property to the member who made the reservation.
    /// </summary>
    [ForeignKey("MemberId")]
    public virtual Member Member { get; set; } = null!;
}