using LibraryApp.Core.Interfaces;
using LibraryApp.Core.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryApp.Core.Services;

/// <summary>
/// Service for JWT token generation and validation.
/// </summary>
public class TokenService : ITokenService
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expirationHours;

    /// <summary>
    /// Initializes a new instance of the TokenService.
    /// </summary>
    /// <param name="secretKey">The secret key for signing tokens.</param>
    /// <param name="issuer">The token issuer.</param>
    /// <param name="audience">The token audience.</param>
    /// <param name="expirationHours">Token expiration time in hours.</param>
    public TokenService(string secretKey, string issuer = "LibraryApp", string audience = "LibraryAppUsers", int expirationHours = 24)
    {
        _secretKey = secretKey ?? throw new ArgumentNullException(nameof(secretKey));
        _issuer = issuer;
        _audience = audience;
        _expirationHours = expirationHours;
    }

    /// <summary>
    /// Generates a JWT token for a member.
    /// </summary>
    /// <param name="member">The member to generate token for.</param>
    /// <returns>The JWT token string.</returns>
    public string GenerateToken(Member member)
    {
        if (member == null)
            throw new ArgumentNullException(nameof(member));

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, member.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, member.Email),
            new Claim(JwtRegisteredClaimNames.Name, member.Name),
            new Claim("role", GetRoleFromMembershipType(member.MembershipType)),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(_expirationHours),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Validates a JWT token and extracts the member ID.
    /// </summary>
    /// <param name="token">The JWT token to validate.</param>
    /// <returns>The member ID if token is valid, null otherwise.</returns>
    public int? ValidateToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return null;

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };

            tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;

            var memberIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            if (memberIdClaim != null && int.TryParse(memberIdClaim.Value, out var memberId))
            {
                return memberId;
            }
        }
        catch
        {
            // Token validation failed
        }

        return null;
    }

    /// <summary>
    /// Gets the member ID from a JWT token without full validation.
    /// </summary>
    /// <param name="token">The JWT token.</param>
    /// <returns>The member ID, or null if token is invalid.</returns>
    public int? GetMemberIdFromToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return null;

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var memberIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            if (memberIdClaim != null && int.TryParse(memberIdClaim.Value, out var memberId))
            {
                return memberId;
            }
        }
        catch
        {
            // Token parsing failed
        }

        return null;
    }

    /// <summary>
    /// Gets the role from a JWT token.
    /// </summary>
    /// <param name="token">The JWT token.</param>
    /// <returns>The role string, or null if token is invalid.</returns>
    public string? GetRoleFromToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return null;

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role");
            return roleClaim?.Value;
        }
        catch
        {
            // Token parsing failed
        }

        return null;
    }

    /// <summary>
    /// Gets all claims from a JWT token.
    /// </summary>
    /// <param name="token">The JWT token.</param>
    /// <returns>The claims, or null if token is invalid.</returns>
    public Claim[]? GetClaimsFromToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return null;

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            return jwtToken.Claims.ToArray();
        }
        catch
        {
            // Token parsing failed
        }

        return null;
    }

    /// <summary>
    /// Maps membership type to role.
    /// </summary>
    /// <param name="membershipType">The membership type.</param>
    /// <returns>The role string.</returns>
    private static string GetRoleFromMembershipType(string membershipType)
    {
        return membershipType?.ToLower() switch
        {
            "admin" => "Admin",
            _ => "Member"
        };
    }
}