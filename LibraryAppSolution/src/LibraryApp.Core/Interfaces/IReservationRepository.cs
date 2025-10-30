using LibraryApp.Core.Models;

namespace LibraryApp.Core.Interfaces;

/// <summary>
/// Repository interface for Reservation entity operations.
/// </summary>
public interface IReservationRepository : IRepository<Reservation>
{
    /// <summary>
    /// Gets all reservations for a specific book.
    /// </summary>
    /// <param name="bookId">The book ID.</param>
    /// <returns>A collection of reservations for the book.</returns>
    Task<IEnumerable<Reservation>> GetReservationsByBookAsync(int bookId);

    /// <summary>
    /// Gets all reservations for a specific member.
    /// </summary>
    /// <param name="memberId">The member ID.</param>
    /// <returns>A collection of reservations for the member.</returns>
    Task<IEnumerable<Reservation>> GetReservationsByMemberAsync(int memberId);

    /// <summary>
    /// Gets all active reservations (not returned).
    /// </summary>
    /// <returns>A collection of active reservations.</returns>
    Task<IEnumerable<Reservation>> GetActiveReservationsAsync();

    /// <summary>
    /// Gets overdue reservations.
    /// </summary>
    /// <returns>A collection of overdue reservations.</returns>
    Task<IEnumerable<Reservation>> GetOverdueReservationsAsync();

    /// <summary>
    /// Gets a reservation with book and member details.
    /// </summary>
    /// <param name="id">The reservation ID.</param>
    /// <returns>The reservation with details, or null if not found.</returns>
    Task<Reservation?> GetReservationWithDetailsAsync(int id);
}