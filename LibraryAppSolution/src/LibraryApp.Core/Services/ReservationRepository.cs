using LibraryApp.Core.Data;
using LibraryApp.Core.Interfaces;
using LibraryApp.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Core.Services;

/// <summary>
/// Repository implementation for Reservation entity operations.
/// </summary>
public class ReservationRepository : IReservationRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the ReservationRepository.
    /// </summary>
    /// <param name="context">The database context.</param>
    public ReservationRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Gets all reservations.
    /// </summary>
    /// <returns>A collection of all reservations.</returns>
    public async Task<IEnumerable<Reservation>> GetAllAsync()
    {
        return await _context.Reservations
            .Include(r => r.Book)
            .Include(r => r.Member)
            .ToListAsync();
    }

    /// <summary>
    /// Gets a reservation by ID.
    /// </summary>
    /// <param name="id">The reservation ID.</param>
    /// <returns>The reservation, or null if not found.</returns>
    public async Task<Reservation?> GetByIdAsync(int id)
    {
        return await _context.Reservations
            .Include(r => r.Book)
            .Include(r => r.Member)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    /// <summary>
    /// Adds a new reservation.
    /// </summary>
    /// <param name="reservation">The reservation to add.</param>
    /// <returns>The added reservation.</returns>
    public async Task<Reservation> AddAsync(Reservation reservation)
    {
        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();
        return reservation;
    }

    /// <summary>
    /// Updates an existing reservation.
    /// </summary>
    /// <param name="reservation">The reservation to update.</param>
    /// <returns>The updated reservation.</returns>
    public async Task<Reservation> UpdateAsync(Reservation reservation)
    {
        _context.Reservations.Update(reservation);
        await _context.SaveChangesAsync();
        return reservation;
    }

    /// <summary>
    /// Deletes a reservation.
    /// </summary>
    /// <param name="id">The reservation ID to delete.</param>
    /// <returns>True if deleted, false if not found.</returns>
    public async Task<bool> DeleteAsync(int id)
    {
        var reservation = await GetByIdAsync(id);
        if (reservation == null)
            return false;

        _context.Reservations.Remove(reservation);
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Checks if a reservation exists by ID.
    /// </summary>
    /// <param name="id">The reservation ID.</param>
    /// <returns>True if exists, false otherwise.</returns>
    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Reservations.AnyAsync(r => r.Id == id);
    }

    /// <summary>
    /// Gets all reservations for a specific book.
    /// </summary>
    /// <param name="bookId">The book ID.</param>
    /// <returns>A collection of reservations for the book.</returns>
    public async Task<IEnumerable<Reservation>> GetReservationsByBookAsync(int bookId)
    {
        return await _context.Reservations
            .Where(r => r.BookId == bookId)
            .Include(r => r.Book)
            .Include(r => r.Member)
            .OrderByDescending(r => r.StartDate)
            .ToListAsync();
    }

    /// <summary>
    /// Gets all reservations for a specific member.
    /// </summary>
    /// <param name="memberId">The member ID.</param>
    /// <returns>A collection of reservations for the member.</returns>
    public async Task<IEnumerable<Reservation>> GetReservationsByMemberAsync(int memberId)
    {
        return await _context.Reservations
            .Where(r => r.MemberId == memberId)
            .Include(r => r.Book)
            .Include(r => r.Member)
            .OrderByDescending(r => r.DueDate)
            .ToListAsync();
    }

    /// <summary>
    /// Gets all active reservations (not returned).
    /// </summary>
    /// <returns>A collection of active reservations.</returns>
    public async Task<IEnumerable<Reservation>> GetActiveReservationsAsync()
    {
        return await _context.Reservations
            .Where(r => r.Status == "Active")
            .Include(r => r.Book)
            .Include(r => r.Member)
            .OrderBy(r => r.DueDate)
            .ToListAsync();
    }

    /// <summary>
    /// Gets overdue reservations.
    /// </summary>
    /// <returns>A collection of overdue reservations.</returns>
    public async Task<IEnumerable<Reservation>> GetOverdueReservationsAsync()
    {
        var now = DateTime.UtcNow;
        return await _context.Reservations
            .Where(r => r.Status == "Active" && r.DueDate < now)
            .Include(r => r.Book)
            .Include(r => r.Member)
            .OrderBy(r => r.DueDate)
            .ToListAsync();
    }

    /// <summary>
    /// Gets a reservation with book and member details.
    /// </summary>
    /// <param name="id">The reservation ID.</param>
    /// <returns>The reservation with details, or null if not found.</returns>
    public async Task<Reservation?> GetReservationWithDetailsAsync(int id)
    {
        return await _context.Reservations
            .Include(r => r.Book)
            .Include(r => r.Member)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    /// <summary>
    /// Checks if a member has reached the maximum number of active reservations (3).
    /// </summary>
    /// <param name="memberId">The member ID.</param>
    /// <returns>True if member has 3 or more active reservations.</returns>
    public async Task<bool> HasMemberReachedReservationLimitAsync(int memberId)
    {
        var activeReservationsCount = await _context.Reservations
            .CountAsync(r => r.MemberId == memberId && r.Status == "Active");
        return activeReservationsCount >= 3;
    }

    /// <summary>
    /// Checks if a book is currently reserved by another member.
    /// </summary>
    /// <param name="bookId">The book ID.</param>
    /// <returns>True if book is currently reserved.</returns>
    public async Task<bool> IsBookCurrentlyReservedAsync(int bookId)
    {
        return await _context.Reservations
            .AnyAsync(r => r.BookId == bookId && r.Status == "Active");
    }
}