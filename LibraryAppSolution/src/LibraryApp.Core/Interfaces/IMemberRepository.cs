using LibraryApp.Core.Models;

namespace LibraryApp.Core.Interfaces;

/// <summary>
/// Repository interface for Member entity operations.
/// </summary>
public interface IMemberRepository : IRepository<Member>
{
    /// <summary>
    /// Gets a member by email.
    /// </summary>
    /// <param name="email">The member email.</param>
    /// <returns>The member, or null if not found.</returns>
    Task<Member?> GetByEmailAsync(string email);

    /// <summary>
    /// Gets all active members.
    /// </summary>
    /// <returns>A collection of active members.</returns>
    Task<IEnumerable<Member>> GetActiveMembersAsync();

    /// <summary>
    /// Gets members with outstanding balances.
    /// </summary>
    /// <returns>A collection of members with balances greater than zero.</returns>
    Task<IEnumerable<Member>> GetMembersWithOutstandingBalancesAsync();

    /// <summary>
    /// Gets a member with their reservations.
    /// </summary>
    /// <param name="id">The member ID.</param>
    /// <returns>The member with reservations, or null if not found.</returns>
    Task<Member?> GetMemberWithReservationsAsync(int id);
}