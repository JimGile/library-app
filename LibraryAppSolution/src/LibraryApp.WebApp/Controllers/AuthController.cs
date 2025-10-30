using LibraryApp.Core.Interfaces;
using LibraryApp.WebApp.Controllers.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.WebApp.Controllers;

/// <summary>
/// Controller for authentication operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ITokenService _tokenService;

    /// <summary>
    /// Initializes a new instance of the AuthController.
    /// </summary>
    /// <param name="authenticationService">The authentication service.</param>
    /// <param name="tokenService">The token service.</param>
    public AuthController(IAuthenticationService authenticationService, ITokenService tokenService)
    {
        _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
    }

    /// <summary>
    /// Registers a new member.
    /// </summary>
    /// <param name="request">The registration request.</param>
    /// <returns>The authentication response with token.</returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var member = await _authenticationService.RegisterAsync(request.Name, request.Email, request.Password);
        if (member == null)
            return BadRequest("Registration failed. Email may already be in use or password doesn't meet requirements.");

        var token = _tokenService.GenerateToken(member);
        var response = new AuthResponse
        {
            Token = token,
            Member = new MemberDto
            {
                Id = member.Id,
                Name = member.Name,
                Email = member.Email,
                Role = _authenticationService.GetMemberRole(member)
            }
        };

        return Ok(response);
    }

    /// <summary>
    /// Authenticates a member.
    /// </summary>
    /// <param name="request">The login request.</param>
    /// <returns>The authentication response with token.</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), 200)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var member = await _authenticationService.AuthenticateAsync(request.Email, request.Password);
        if (member == null)
            return Unauthorized("Invalid email or password.");

        var token = _tokenService.GenerateToken(member);
        var response = new AuthResponse
        {
            Token = token,
            Member = new MemberDto
            {
                Id = member.Id,
                Name = member.Name,
                Email = member.Email,
                Role = _authenticationService.GetMemberRole(member)
            }
        };

        return Ok(response);
    }

    /// <summary>
    /// Changes a member's password.
    /// </summary>
    /// <param name="request">The change password request.</param>
    /// <returns>Success response.</returns>
    [HttpPost("change-password")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Get member ID from JWT token
        var memberIdClaim = User.FindFirst("memberId")?.Value;
        if (string.IsNullOrEmpty(memberIdClaim) || !int.TryParse(memberIdClaim, out var memberId))
            return Unauthorized();

        var success = await _authenticationService.ChangePasswordAsync(memberId, request.CurrentPassword, request.NewPassword);
        if (!success)
            return BadRequest("Password change failed. Current password may be incorrect or new password doesn't meet requirements.");

        return Ok(new { message = "Password changed successfully." });
    }
}