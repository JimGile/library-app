using LibraryApp.Core.Data;
using LibraryApp.Core.Interfaces;
using LibraryApp.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace LibraryApp.WebApp;

public class AuthSystemTest
{
    [Fact]
    public async Task AuthenticationSystem_WorksCorrectly()
    {
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
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<AuthSystemTest>>();
            await DatabaseInitializer.InitializeAsync(context, logger);
        }

        // Test authentication
        using (var scope = serviceProvider.CreateScope())
        {
            var authService = scope.ServiceProvider.GetRequiredService<IAuthenticationService>();
            var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();

            // Test user registration
            var registeredMember = await authService.RegisterAsync("Test User", "test@example.com", "TestPass123");
            Assert.NotNull(registeredMember);
            Assert.NotNull(registeredMember!.Name);
            Assert.Equal("Test User", registeredMember.Name);
            Assert.Equal("test@example.com", registeredMember.Email);

            // Test user login
            var authenticatedMember = await authService.AuthenticateAsync("test@example.com", "TestPass123");
            Assert.NotNull(authenticatedMember);
            Assert.NotNull(authenticatedMember!.Name);
            Assert.Equal("Test User", authenticatedMember.Name);

            // Test token generation
            var token = tokenService.GenerateToken(authenticatedMember);
            Assert.NotNull(token);
            Assert.NotEmpty(token);

            // Test token validation
            var claims = tokenService.GetClaimsFromToken(token);
            Assert.NotNull(claims);
            Assert.NotNull(claims);
            Assert.True(claims.Length > 0);

            // Test role extraction
            var role = tokenService.GetRoleFromToken(token);
            Assert.Equal("Member", role);

            var memberId = tokenService.GetMemberIdFromToken(token);
            Assert.Equal(authenticatedMember.Id, memberId);

            // Test duplicate registration fails
            var duplicateMember = await authService.RegisterAsync("Another User", "test@example.com", "AnotherPass123");
            Assert.Null(duplicateMember);
        }
    }
}