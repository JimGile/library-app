using LibraryApp.Core.Data;
using LibraryApp.Core.Interfaces;
using LibraryApp.Core.Models;

namespace LibraryApp.Core.Services;

/// <summary>
/// Service for member authentication operations.
/// </summary>
public class AuthenticationService : IAuthenticationService
{
    private readonly IMemberRepository _memberRepository;

    /// <summary>
    /// Initializes a new instance of the AuthenticationService.
    /// </summary>
    /// <param name="memberRepository">The member repository.</param>
    public AuthenticationService(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
    }

    /// <summary>
    /// Registers a new member.
    /// </summary>
    /// <param name="name">The member's full name.</param>
    /// <param name="email">The member's email address.</param>
    /// <param name="password">The member's password.</param>
    /// <returns>The registered member, or null if registration failed.</returns>
    public async Task<Member?> RegisterAsync(string name, string email, string password)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return null;

        if (!ValidatePassword(password))
            return null;

        // Check if email already exists
        var existingMember = await _memberRepository.GetByEmailAsync(email);
        if (existingMember != null)
            return null;

        var member = new Member
        {
            Name = name.Trim(),
            Email = email.Trim().ToLower(),
            PasswordHash = HashPassword(password),
            MembershipType = "Standard",
            MembershipStartDate = DateTime.UtcNow,
            MembershipStatus = "Active",
            MembershipBalance = 0
        };

        return await _memberRepository.AddAsync(member);
    }

    /// <summary>
    /// Authenticates a member with email and password.
    /// </summary>
    /// <param name="email">The member's email address.</param>
    /// <param name="password">The member's password.</param>
    /// <returns>The authenticated member, or null if authentication failed.</returns>
    public async Task<Member?> AuthenticateAsync(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return null;

        var member = await _memberRepository.GetByEmailAsync(email.Trim().ToLower());
        if (member == null)
            return null;

        if (!VerifyPassword(password, member.PasswordHash))
            return null;

        if (member.MembershipStatus != "Active")
            return null;

        return member;
    }

    /// <summary>
    /// Changes a member's password.
    /// </summary>
    /// <param name="memberId">The member ID.</param>
    /// <param name="currentPassword">The current password.</param>
    /// <param name="newPassword">The new password.</param>
    /// <returns>True if password was changed successfully, false otherwise.</returns>
    public async Task<bool> ChangePasswordAsync(int memberId, string currentPassword, string newPassword)
    {
        if (string.IsNullOrWhiteSpace(currentPassword) || string.IsNullOrWhiteSpace(newPassword))
            return false;

        if (!ValidatePassword(newPassword))
            return false;

        var member = await _memberRepository.GetByIdAsync(memberId);
        if (member == null)
            return false;

        if (!VerifyPassword(currentPassword, member.PasswordHash))
            return false;

        member.PasswordHash = HashPassword(newPassword);
        await _memberRepository.UpdateAsync(member);

        return true;
    }

    /// <summary>
    /// Validates if a password meets security requirements.
    /// </summary>
    /// <param name="password">The password to validate.</param>
    /// <returns>True if password is valid, false otherwise.</returns>
    public bool ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return false;

        // Password must be at least 8 characters long
        if (password.Length < 8)
            return false;

        // Password must contain at least one uppercase letter
        if (!password.Any(char.IsUpper))
            return false;

        // Password must contain at least one lowercase letter
        if (!password.Any(char.IsLower))
            return false;

        // Password must contain at least one digit
        if (!password.Any(char.IsDigit))
            return false;

        return true;
    }

    /// <summary>
    /// Gets the role for a member.
    /// </summary>
    /// <param name="member">The member.</param>
    /// <returns>The role string ("Member" or "Admin").</returns>
    public string GetMemberRole(Member member)
    {
        if (member == null)
            throw new ArgumentNullException(nameof(member));

        return member.MembershipType?.ToLower() switch
        {
            "admin" => "Admin",
            _ => "Member"
        };
    }

    /// <summary>
    /// Hashes a password using BCrypt.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <returns>The hashed password.</returns>
    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
    }

    /// <summary>
    /// Verifies a password against a hash.
    /// </summary>
    /// <param name="password">The password to verify.</param>
    /// <param name="hash">The hash to verify against.</param>
    /// <returns>True if password matches hash, false otherwise.</returns>
    private static bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}