using LibraryApp.Core.Data;
using LibraryApp.Core.Interfaces;
using LibraryApp.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Core.Services;

/// <summary>
/// Repository for member data operations.
/// </summary>
public class MemberRepository : IMemberRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the MemberRepository.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public MemberRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Gets a member by ID.
    /// </summary>
    /// <param name="id">The member ID.</param>
    /// <returns>The member, or null if not found.</returns>
    public async Task<Member?> GetByIdAsync(int id)
    {
        return await _context.Members.FindAsync(id);
    }

    /// <summary>
    /// Gets a member by email.
    /// </summary>
    /// <param name="email">The member's email address.</param>
    /// <returns>The member, or null if not found.</returns>
    public async Task<Member?> GetByEmailAsync(string email)
    {
        return await _context.Members
            .FirstOrDefaultAsync(m => m.Email.ToLower() == email.ToLower());
    }

    /// <summary>
    /// Adds a new member.
    /// </summary>
    /// <param name="member">The member to add.</param>
    /// <returns>The added member.</returns>
    public async Task<Member> AddAsync(Member member)
    {
        _context.Members.Add(member);
        await _context.SaveChangesAsync();
        return member;
    }

    /// <summary>
    /// Updates an existing member.
    /// </summary>
    /// <param name="member">The member to update.</param>
    /// <returns>The updated member.</returns>
    public async Task<Member> UpdateAsync(Member member)
    {
        _context.Members.Update(member);
        await _context.SaveChangesAsync();
        return member;
    }

    /// <summary>
    /// Checks if a member exists by ID.
    /// </summary>
    /// <param name="id">The member ID.</param>
    /// <returns>True if exists, false otherwise.</returns>
    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Members.AnyAsync(m => m.Id == id);
    }

    /// <summary>
    /// Gets all active members.
    /// </summary>
    /// <returns>A collection of active members.</returns>
    public async Task<IEnumerable<Member>> GetActiveMembersAsync()
    {
        return await _context.Members
            .Where(m => m.MembershipStatus == "Active")
            .ToListAsync();
    }

    /// <summary>
    /// Gets members with outstanding balances.
    /// </summary>
    /// <returns>A collection of members with balances greater than zero.</returns>
    public async Task<IEnumerable<Member>> GetMembersWithOutstandingBalancesAsync()
    {
        return await _context.Members
            .Where(m => m.MembershipBalance > 0)
            .ToListAsync();
    }

    /// <summary>
    /// Gets a member with their reservations.
    /// </summary>
    /// <param name="id">The member ID.</param>
    /// <returns>The member with reservations, or null if not found.</returns>
    public async Task<Member?> GetMemberWithReservationsAsync(int id)
    {
        return await _context.Members
            .Include(m => m.Reservations)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    /// <summary>
    /// Deletes a member.
    /// </summary>
    /// <param name="id">The member ID to delete.</param>
    /// <returns>True if deleted, false if not found.</returns>
    public async Task<bool> DeleteAsync(int id)
    {
        var member = await GetByIdAsync(id);
        if (member == null)
            return false;

        _context.Members.Remove(member);
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Gets all members.
    /// </summary>
    /// <returns>A collection of all members.</returns>
    public async Task<IEnumerable<Member>> GetAllAsync()
    {
        return await _context.Members.ToListAsync();
    }
}