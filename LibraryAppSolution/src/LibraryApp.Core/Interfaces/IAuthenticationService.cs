using LibraryApp.Core.Models;

namespace LibraryApp.Core.Interfaces;

/// <summary>
/// Interface for authentication operations.
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Registers a new member.
    /// </summary>
    /// <param name="name">The member's full name.</param>
    /// <param name="email">The member's email address.</param>
    /// <param name="password">The member's password.</param>
    /// <returns>The registered member, or null if registration failed.</returns>
    Task<Member?> RegisterAsync(string name, string email, string password);

    /// <summary>
    /// Authenticates a member with email and password.
    /// </summary>
    /// <param name="email">The member's email address.</param>
    /// <param name="password">The member's password.</param>
    /// <returns>The authenticated member, or null if authentication failed.</returns>
    Task<Member?> AuthenticateAsync(string email, string password);

    /// <summary>
    /// Changes a member's password.
    /// </summary>
    /// <param name="memberId">The member ID.</param>
    /// <param name="currentPassword">The current password.</param>
    /// <param name="newPassword">The new password.</param>
    /// <returns>True if password was changed successfully, false otherwise.</returns>
    Task<bool> ChangePasswordAsync(int memberId, string currentPassword, string newPassword);

    /// <summary>
    /// Validates if a password meets security requirements.
    /// </summary>
    /// <param name="password">The password to validate.</param>
    /// <returns>True if password is valid, false otherwise.</returns>
    bool ValidatePassword(string password);

    /// <summary>
    /// Gets the role for a member.
    /// </summary>
    /// <param name="member">The member.</param>
    /// <returns>The role string ("Member" or "Admin").</returns>
    string GetMemberRole(Member member);
}