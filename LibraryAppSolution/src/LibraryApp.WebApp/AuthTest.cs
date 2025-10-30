using LibraryApp.Core.Data;
using LibraryApp.Core.Interfaces;
using LibraryApp.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LibraryApp.WebApp;

class AuthTestProgram
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Testing Authentication System...");

        // Setup configuration
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        // Setup services
        var services = new ServiceCollection();
        services.AddLogging(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Debug));
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        // Add authentication services
        services.AddScoped<ITokenService>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var secretKey = config["Jwt:Key"]!;
            var issuer = config["Jwt:Issuer"]!;
            var audience = config["Jwt:Audience"]!;
            var expiryHours = int.Parse(config["Jwt:ExpiryInHours"]!);
            return new TokenService(secretKey, issuer, audience, expiryHours);
        });
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IMemberRepository, MemberRepository>();

        var serviceProvider = services.BuildServiceProvider();

        // Initialize database
        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            await DatabaseInitializer.InitializeAsync(context, logger);
        }

        // Test authentication
        using (var scope = serviceProvider.CreateScope())
        {
            var authService = scope.ServiceProvider.GetRequiredService<IAuthenticationService>();
            var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();

            Console.WriteLine("\n1. Testing user registration...");
            var registeredMember = await authService.RegisterAsync("Test User", "test@example.com", "TestPass123");
            if (registeredMember == null)
            {
                Console.WriteLine("Registration failed - member is null");
            }
            else
            {
                Console.WriteLine($"Registration successful: {registeredMember.Name} ({registeredMember.Email})");
            }

            Console.WriteLine("\n2. Testing user login...");
            var authenticatedMember = await authService.AuthenticateAsync("test@example.com", "TestPass123");
            if (authenticatedMember == null)
            {
                Console.WriteLine("Authentication failed - member is null");
            }
            else
            {
                Console.WriteLine($"Authentication successful: {authenticatedMember.Name}");

                Console.WriteLine("\n3. Testing token generation...");
                var token = tokenService.GenerateToken(authenticatedMember);
                Console.WriteLine($"Generated token: {token.Substring(0, 50)}...");

                Console.WriteLine("\n4. Testing token validation...");
                var claims = tokenService.GetClaimsFromToken(token);
                if (claims != null)
                {
                    Console.WriteLine($"Token claims: {claims.Length}");
                    foreach (var claim in claims)
                    {
                        Console.WriteLine($"  {claim.Type}: {claim.Value}");
                    }
                }
                else
                {
                    Console.WriteLine("Failed to get claims from token");
                }

                Console.WriteLine("\n5. Testing role extraction...");
                var role = tokenService.GetRoleFromToken(token);
                Console.WriteLine($"User role: {role}");

                var memberId = tokenService.GetMemberIdFromToken(token);
                Console.WriteLine($"Member ID: {memberId}");
            }

            Console.WriteLine("\n6. Testing duplicate registration...");
            var duplicateMember = await authService.RegisterAsync("Another User", "test@example.com", "AnotherPass123");
            if (duplicateMember == null)
            {
                Console.WriteLine("Duplicate registration correctly failed (member is null)");
            }
            else
            {
                Console.WriteLine("ERROR: Duplicate registration should have failed!");
            }
        }

        Console.WriteLine("\nAuthentication system test completed!");
    }
}