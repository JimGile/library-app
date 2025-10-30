using LibraryApp.Core.Data;
using LibraryApp.Core.Interfaces;
using LibraryApp.Core.Services;
using LibraryApp.WebApp.Components;
using LibraryApp.WebApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add controllers
builder.Services.AddControllers();

// Add database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add repositories
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// Add HTTP client
builder.Services.AddScoped<HttpClient>();

// Add authentication services
builder.Services.AddScoped<ITokenService>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var secretKey = configuration["Jwt:Key"]!;
    var issuer = configuration["Jwt:Issuer"]!;
    var audience = configuration["Jwt:Audience"]!;
    var expiryHours = int.Parse(configuration["Jwt:ExpiryInHours"]!);
    return new TokenService(secretKey, issuer, audience, expiryHours);
});
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

// Add book service for UI
builder.Services.AddScoped<LibraryApp.Core.Interfaces.IBookService, LibraryApp.UI.Services.BookService>();

// Add Blazor authentication
builder.Services.AddScoped<LibraryApp.WebApp.Services.LibraryAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<LibraryApp.WebApp.Services.LibraryAuthStateProvider>());

// Add JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// Add authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MemberOnly", policy => policy.RequireRole("Member", "Admin"));
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

var app = builder.Build();

// Initialize the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    var logger = services.GetRequiredService<ILogger<Program>>();
    await DatabaseInitializer.InitializeAsync(context, logger);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add API controllers
app.MapControllers();

await app.RunAsync();
