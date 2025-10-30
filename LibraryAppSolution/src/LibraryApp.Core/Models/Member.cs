using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Core.Models;

/// <summary>
/// Represents a library member who can borrow books.
/// </summary>
public class Member
{
    /// <summary>
    /// Unique identifier for the member.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Member full name (required, max 100 characters).
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Member email address (required, max 100 characters, unique).
    /// </summary>
    [Required]
    [StringLength(100)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Hashed password for authentication.
    /// </summary>
    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Type of membership (e.g., "Standard", "Premium").
    /// </summary>
    [Required]
    public string MembershipType { get; set; } = "Standard";

    /// <summary>
    /// When membership started.
    /// </summary>
    [Required]
    public DateTime MembershipStartDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// When membership expires (optional).
    /// </summary>
    public DateTime? MembershipEndDate { get; set; }

    /// <summary>
    /// Membership status ("Active" or "Inactive").
    /// </summary>
    [Required]
    public string MembershipStatus { get; set; } = "Active";

    /// <summary>
    /// Current balance (positive for fees owed).
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal MembershipBalance { get; set; } = 0;

    /// <summary>
    /// Navigation property to the member's reservations.
    /// </summary>
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}