using LibraryApp.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Core.Data;

/// <summary>
/// Application database context for Entity Framework Core.
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the ApplicationDbContext.
    /// </summary>
    /// <param name="options">The options to be used by a DbContext.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the Books DbSet.
    /// </summary>
    public DbSet<Book> Books { get; set; } = null!;

    /// <summary>
    /// Gets or sets the Categories DbSet.
    /// </summary>
    public DbSet<Category> Categories { get; set; } = null!;

    /// <summary>
    /// Gets or sets the Members DbSet.
    /// </summary>
    public DbSet<Member> Members { get; set; } = null!;

    /// <summary>
    /// Gets or sets the Reservations DbSet.
    /// </summary>
    public DbSet<Reservation> Reservations { get; set; } = null!;

    /// <summary>
    /// Configures the model that was discovered by convention from the entity types
    /// exposed in DbSet properties on your derived context.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Book entity
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasIndex(e => e.Title);
            entity.HasIndex(e => e.Author);

            entity.HasOne(d => d.Category)
                .WithMany(p => p.Books)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure Category entity
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // Configure Member entity
        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Configure Reservation entity
        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasIndex(e => new { e.BookId, e.Status });
            entity.HasIndex(e => new { e.MemberId, e.Status });

            entity.HasOne(d => d.Book)
                .WithMany(p => p.Reservations)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.Member)
                .WithMany(p => p.Reservations)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}