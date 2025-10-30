using LibraryApp.Core.Models;
using LibraryApp.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.WebApp.Controllers;

/// <summary>
/// Controller for managing book reservations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "MemberOnly")]
public class ReservationController : ControllerBase
{
    private readonly ReservationService _reservationService;

    /// <summary>
    /// Initializes a new instance of the ReservationController.
    /// </summary>
    /// <param name="reservationService">The reservation service.</param>
    public ReservationController(ReservationService reservationService)
    {
        _reservationService = reservationService ?? throw new ArgumentNullException(nameof(reservationService));
    }

    /// <summary>
    /// Reserves a book for the authenticated member.
    /// </summary>
    /// <param name="bookId">The book ID to reserve.</param>
    /// <returns>The created reservation.</returns>
    [HttpPost("reserve/{bookId}")]
    public async Task<IActionResult> ReserveBook(int bookId)
    {
        try
        {
            // Get member ID from JWT token
            var memberIdClaim = User.FindFirst("memberId")?.Value;
            if (string.IsNullOrEmpty(memberIdClaim) || !int.TryParse(memberIdClaim, out var memberId))
                return Unauthorized("Invalid member ID in token.");

            var reservation = await _reservationService.ReserveBookAsync(bookId, memberId);
            return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, reservation);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while reserving the book: {ex.Message}");
        }
    }

    /// <summary>
    /// Returns a reserved book.
    /// </summary>
    /// <param name="id">The reservation ID to return.</param>
    /// <returns>The updated reservation.</returns>
    [HttpPost("{id}/return")]
    public async Task<IActionResult> ReturnBook(int id)
    {
        try
        {
            var reservation = await _reservationService.ReturnBookAsync(id);
            return Ok(reservation);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while returning the book: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets a reservation by ID.
    /// </summary>
    /// <param name="id">The reservation ID.</param>
    /// <returns>The reservation details.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetReservation(int id)
    {
        try
        {
            var reservation = await _reservationService.GetReservationWithDetailsAsync(id);
            if (reservation == null)
                return NotFound("Reservation not found.");

            return Ok(reservation);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while retrieving the reservation: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets all reservations for the authenticated member.
    /// </summary>
    /// <returns>The member's reservations.</returns>
    [HttpGet("my-reservations")]
    public async Task<IActionResult> GetMyReservations()
    {
        try
        {
            // Get member ID from JWT token
            var memberIdClaim = User.FindFirst("memberId")?.Value;
            if (string.IsNullOrEmpty(memberIdClaim) || !int.TryParse(memberIdClaim, out var memberId))
                return Unauthorized("Invalid member ID in token.");

            var reservations = await _reservationService.GetMemberReservationsAsync(memberId);
            return Ok(reservations);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while retrieving reservations: {ex.Message}");
        }
    }

    /// <summary>
    /// Checks if the authenticated member can reserve more books.
    /// </summary>
    /// <returns>True if the member can reserve more books.</returns>
    [HttpGet("can-reserve-more")]
    public async Task<IActionResult> CanReserveMoreBooks()
    {
        try
        {
            // Get member ID from JWT token
            var memberIdClaim = User.FindFirst("memberId")?.Value;
            if (string.IsNullOrEmpty(memberIdClaim) || !int.TryParse(memberIdClaim, out var memberId))
                return Unauthorized("Invalid member ID in token.");

            var canReserve = await _reservationService.CanMemberReserveMoreBooksAsync(memberId);
            return Ok(new { canReserve });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while checking reservation limit: {ex.Message}");
        }
    }

    /// <summary>
    /// Checks if a book is available for reservation.
    /// </summary>
    /// <param name="bookId">The book ID.</param>
    /// <returns>True if the book is available.</returns>
    [HttpGet("book/{bookId}/available")]
    public async Task<IActionResult> IsBookAvailable(int bookId)
    {
        try
        {
            var isAvailable = await _reservationService.IsBookAvailableForReservationAsync(bookId);
            return Ok(new { isAvailable });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while checking book availability: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets all active reservations (Admin only).
    /// </summary>
    /// <returns>All active reservations.</returns>
    [HttpGet("active")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetActiveReservations()
    {
        try
        {
            var reservations = await _reservationService.GetActiveReservationsAsync();
            return Ok(reservations);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while retrieving active reservations: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets all overdue reservations (Admin only).
    /// </summary>
    /// <returns>All overdue reservations.</returns>
    [HttpGet("overdue")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetOverdueReservations()
    {
        try
        {
            var reservations = await _reservationService.GetOverdueReservationsAsync();
            return Ok(reservations);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while retrieving overdue reservations: {ex.Message}");
        }
    }
}