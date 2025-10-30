using System.ComponentModel.DataAnnotations;

namespace LibraryApp.WebApp.Controllers.Models;

/// <summary>
/// Request model for member registration.
/// </summary>
public class RegisterRequest
{
    /// <summary>
    /// The member's full name.
    /// </summary>
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public required string Name { get; set; }

    /// <summary>
    /// The member's email address.
    /// </summary>
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    /// <summary>
    /// The member's password.
    /// </summary>
    [Required]
    [StringLength(100, MinimumLength = 8)]
    public required string Password { get; set; }
}

/// <summary>
/// Request model for member login.
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// The member's email address.
    /// </summary>
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    /// <summary>
    /// The member's password.
    /// </summary>
    [Required]
    public required string Password { get; set; }
}

/// <summary>
/// Request model for changing password.
/// </summary>
public class ChangePasswordRequest
{
    /// <summary>
    /// The current password.
    /// </summary>
    [Required]
    public required string CurrentPassword { get; set; }

    /// <summary>
    /// The new password.
    /// </summary>
    [Required]
    [StringLength(100, MinimumLength = 8)]
    public required string NewPassword { get; set; }
}

/// <summary>
/// Response model for authentication operations.
/// </summary>
public class AuthResponse
{
    /// <summary>
    /// The JWT token.
    /// </summary>
    public required string Token { get; set; }

    /// <summary>
    /// The authenticated member information.
    /// </summary>
    public required MemberDto Member { get; set; }
}

/// <summary>
/// Data transfer object for member information.
/// </summary>
public class MemberDto
{
    /// <summary>
    /// The member ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The member's full name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The member's email address.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// The member's role.
    /// </summary>
    public required string Role { get; set; }
}