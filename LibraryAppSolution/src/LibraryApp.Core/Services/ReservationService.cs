using LibraryApp.Core.Interfaces;
using LibraryApp.Core.Models;

namespace LibraryApp.Core.Services;

/// <summary>
/// Service for managing book reservations with business logic.
/// </summary>
public class ReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IMemberRepository _memberRepository;

    /// <summary>
    /// Initializes a new instance of the ReservationService.
    /// </summary>
    /// <param name="reservationRepository">The reservation repository.</param>
    /// <param name="bookRepository">The book repository.</param>
    /// <param name="memberRepository">The member repository.</param>
    public ReservationService(
        IReservationRepository reservationRepository,
        IBookRepository bookRepository,
        IMemberRepository memberRepository)
    {
        _reservationRepository = reservationRepository ?? throw new ArgumentNullException(nameof(reservationRepository));
        _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
        _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
    }

    /// <summary>
    /// Reserves a book for a member.
    /// </summary>
    /// <param name="bookId">The book ID to reserve.</param>
    /// <param name="memberId">The member ID making the reservation.</param>
    /// <returns>The created reservation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when business rules are violated.</exception>
    public async Task<Reservation> ReserveBookAsync(int bookId, int memberId)
    {
        // Validate that the book exists
        var book = await _bookRepository.GetByIdAsync(bookId);
        if (book == null)
            throw new InvalidOperationException("Book not found.");

        // Validate that the member exists
        var member = await _memberRepository.GetByIdAsync(memberId);
        if (member == null)
            throw new InvalidOperationException("Member not found.");

        // Check if member has reached the reservation limit (3 books)
        if (await _reservationRepository.HasMemberReachedReservationLimitAsync(memberId))
            throw new InvalidOperationException("Member has reached the maximum number of active reservations (3).");

        // Check if the book is already reserved
        if (await _reservationRepository.IsBookCurrentlyReservedAsync(bookId))
            throw new InvalidOperationException("Book is currently reserved by another member.");

        // Create the reservation
        var reservation = new Reservation
        {
            BookId = bookId,
            MemberId = memberId,
            StartDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(14), // 14-day loan period
            Status = "Active",
            LateFee = 0
        };

        return await _reservationRepository.AddAsync(reservation);
    }

    /// <summary>
    /// Returns a reserved book.
    /// </summary>
    /// <param name="reservationId">The reservation ID to return.</param>
    /// <returns>The updated reservation with any late fees calculated.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the reservation is not found or already returned.</exception>
    public async Task<Reservation> ReturnBookAsync(int reservationId)
    {
        var reservation = await _reservationRepository.GetByIdAsync(reservationId);
        if (reservation == null)
            throw new InvalidOperationException("Reservation not found.");

        if (reservation.Status != "Active")
            throw new InvalidOperationException("Book has already been returned.");

        var returnDate = DateTime.UtcNow;
        reservation.ReturnDate = returnDate;

        // Calculate late fees if returned after due date ($5 per day)
        if (returnDate > reservation.DueDate)
        {
            var daysLate = (returnDate - reservation.DueDate).Days;
            reservation.LateFee = daysLate * 5;
        }

        reservation.Status = "Returned";

        return await _reservationRepository.UpdateAsync(reservation);
    }

    /// <summary>
    /// Gets all reservations for a member.
    /// </summary>
    /// <param name="memberId">The member ID.</param>
    /// <returns>A collection of the member's reservations.</returns>
    public async Task<IEnumerable<Reservation>> GetMemberReservationsAsync(int memberId)
    {
        return await _reservationRepository.GetReservationsByMemberAsync(memberId);
    }

    /// <summary>
    /// Gets all active reservations.
    /// </summary>
    /// <returns>A collection of active reservations.</returns>
    public async Task<IEnumerable<Reservation>> GetActiveReservationsAsync()
    {
        return await _reservationRepository.GetActiveReservationsAsync();
    }

    /// <summary>
    /// Gets all overdue reservations.
    /// </summary>
    /// <returns>A collection of overdue reservations.</returns>
    public async Task<IEnumerable<Reservation>> GetOverdueReservationsAsync()
    {
        return await _reservationRepository.GetOverdueReservationsAsync();
    }

    /// <summary>
    /// Gets a reservation with full details.
    /// </summary>
    /// <param name="reservationId">The reservation ID.</param>
    /// <returns>The reservation with details, or null if not found.</returns>
    public async Task<Reservation?> GetReservationWithDetailsAsync(int reservationId)
    {
        return await _reservationRepository.GetReservationWithDetailsAsync(reservationId);
    }

    /// <summary>
    /// Checks if a member can reserve more books.
    /// </summary>
    /// <param name="memberId">The member ID.</param>
    /// <returns>True if the member can reserve more books.</returns>
    public async Task<bool> CanMemberReserveMoreBooksAsync(int memberId)
    {
        return !await _reservationRepository.HasMemberReachedReservationLimitAsync(memberId);
    }

    /// <summary>
    /// Checks if a book is available for reservation.
    /// </summary>
    /// <param name="bookId">The book ID.</param>
    /// <returns>True if the book is available.</returns>
    public async Task<bool> IsBookAvailableForReservationAsync(int bookId)
    {
        return !await _reservationRepository.IsBookCurrentlyReservedAsync(bookId);
    }
}