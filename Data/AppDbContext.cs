using Microsoft.EntityFrameworkCore;
using Helpdesk.Api.Models;

namespace Helpdesk.Api.Data;

/// <summary>
/// Represents the Entity Framework database context for the Helpdesk application,
/// managing database sessions and mapping domain entities to the underlying PostgreSQL storage.
/// </summary>
/// <param name="options">The database context options.</param>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Gets the database set for support tickets.
    /// </summary>
    public DbSet<Ticket> Tickets => Set<Ticket>();

    /// <summary>
    /// Configures the database schema, entity relationships, and constraints using the Fluent API.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Explicit entity configuration for enterprise data integrity
        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(t => t.Id);

            entity.Property(t => t.Title)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(t => t.Description)
                  .IsRequired();

            entity.Property(t => t.Status)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(t => t.Priority)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(t => t.CreatedBy)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(t => t.AssignedTo)
                  .HasMaxLength(100);
        });
    }
}