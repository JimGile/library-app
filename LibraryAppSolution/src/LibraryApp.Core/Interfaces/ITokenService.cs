using LibraryApp.Core.Models;

namespace LibraryApp.Core.Interfaces;

/// <summary>
/// Interface for JWT token operations.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generates a JWT token for a member.
    /// </summary>
    /// <param name="member">The member to generate token for.</param>
    /// <returns>The JWT token string.</returns>
    string GenerateToken(Member member);

    /// <summary>
    /// Validates a JWT token and extracts the member ID.
    /// </summary>
    /// <param name="token">The JWT token to validate.</param>
    /// <returns>The member ID if token is valid, null otherwise.</returns>
    int? ValidateToken(string token);

    /// <summary>
    /// Gets the member ID from a JWT token without full validation.
    /// </summary>
    /// <param name="token">The JWT token.</param>
    /// <returns>The member ID, or null if token is invalid.</returns>
    int? GetMemberIdFromToken(string token);

    /// <summary>
    /// Gets the role from a JWT token.
    /// </summary>
    /// <param name="token">The JWT token.</param>
    /// <returns>The role string, or null if token is invalid.</returns>
    string? GetRoleFromToken(string token);

    /// <summary>
    /// Gets all claims from a JWT token.
    /// </summary>
    /// <param name="token">The JWT token.</param>
    /// <returns>The claims, or null if token is invalid.</returns>
    System.Security.Claims.Claim[]? GetClaimsFromToken(string token);
}