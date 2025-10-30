using LibraryApp.Core.Interfaces;
using LibraryApp.WebApp.Controllers.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;

namespace LibraryApp.WebApp.Services;

/// <summary>
/// Authentication state provider for Blazor Server.
/// </summary>
public class LibraryAuthStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly ITokenService _tokenService;

    /// <summary>
    /// Initializes a new instance of the LibraryAuthStateProvider.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="tokenService">The token service.</param>
    public LibraryAuthStateProvider(HttpClient httpClient, ITokenService tokenService)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
    }

    /// <summary>
    /// Gets the current authentication state.
    /// </summary>
    /// <returns>The authentication state.</returns>
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await GetTokenAsync();
        if (string.IsNullOrEmpty(token))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        try
        {
            var claims = _tokenService.GetClaimsFromToken(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }
        catch
        {
            // Token is invalid, return anonymous user
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    /// <summary>
    /// Logs in a user with the provided credentials.
    /// </summary>
    /// <param name="email">The user's email.</param>
    /// <param name="password">The user's password.</param>
    /// <returns>True if login was successful, false otherwise.</returns>
    public async Task<bool> LoginAsync(string email, string password)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", new
            {
                Email = email,
                Password = password
            });

            if (!response.IsSuccessStatusCode)
                return false;

            var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
            if (authResponse == null)
                return false;

            await SaveTokenAsync(authResponse.Token);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="name">The user's name.</param>
    /// <param name="email">The user's email.</param>
    /// <param name="password">The user's password.</param>
    /// <returns>True if registration was successful, false otherwise.</returns>
    public async Task<bool> RegisterAsync(string name, string email, string password)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", new
            {
                Name = name,
                Email = email,
                Password = password
            });

            if (!response.IsSuccessStatusCode)
                return false;

            var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
            if (authResponse == null)
                return false;

            await SaveTokenAsync(authResponse.Token);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Logs out the current user.
    /// </summary>
    public async Task LogoutAsync()
    {
        await RemoveTokenAsync();
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    /// <summary>
    /// Gets the current user's token.
    /// </summary>
    /// <returns>The JWT token, or null if not authenticated.</returns>
    private async Task<string?> GetTokenAsync()
    {
        // In a real application, this would get the token from local storage or a secure cookie
        // For now, we'll use session storage via JavaScript interop
        return null; // TODO: Implement token storage
    }

    /// <summary>
    /// Saves the token securely.
    /// </summary>
    /// <param name="token">The JWT token to save.</param>
    private async Task SaveTokenAsync(string token)
    {
        // TODO: Implement secure token storage
    }

    /// <summary>
    /// Removes the stored token.
    /// </summary>
    private async Task RemoveTokenAsync()
    {
        // TODO: Implement token removal
    }
}