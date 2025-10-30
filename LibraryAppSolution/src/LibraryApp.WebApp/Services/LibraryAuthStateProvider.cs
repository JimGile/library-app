using LibraryApp.Core.Interfaces;
using LibraryApp.WebApp.Controllers.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
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
    private readonly IJSRuntime _jsRuntime;

    /// <summary>
    /// Initializes a new instance of the LibraryAuthStateProvider.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="tokenService">The token service.</param>
    /// <param name="jsRuntime">The JavaScript runtime.</param>
    public LibraryAuthStateProvider(HttpClient httpClient, ITokenService tokenService, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
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
        try
        {
            return await _jsRuntime.InvokeAsync<string?>("sessionStorageHelper.getItem", "authToken");
        }
        catch
        {
            // JSRuntime not available or session storage not accessible
            return null;
        }
    }

    /// <summary>
    /// Saves the token securely.
    /// </summary>
    /// <param name="token">The JWT token to save.</param>
    private async Task SaveTokenAsync(string token)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("sessionStorageHelper.setItem", "authToken", token);
        }
        catch
        {
            // JSRuntime not available or session storage not accessible
            // This is expected during server-side initialization
        }
    }

    /// <summary>
    /// Removes the stored token.
    /// </summary>
    private async Task RemoveTokenAsync()
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("sessionStorageHelper.removeItem", "authToken");
        }
        catch
        {
            // JSRuntime not available or session storage not accessible
            // This is expected during server-side initialization
        }
    }
}