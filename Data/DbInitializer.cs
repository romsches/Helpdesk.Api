using Helpdesk.Api.Models;

namespace Helpdesk.Api.Data;

/// <summary>
/// Provides seeding logic to initialize the database with initial demo tickets
/// if no records currently exist.
/// </summary>
public static class DbInitializer
{
    /// <summary>
    /// Initializes the database with default seed data asynchronously.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task InitializeAsync(AppDbContext context)
    {
        // Ensure the database is created and check if any tickets already exist
        if (await context.Tickets.AnyAsync())
        {
            return; // Database has already been seeded
        }

        var initialTickets = new Ticket[]
        {
            new()
            {
                Title = "Printer on 2nd floor is not working",
                Description = "The HP LaserJet network printer is unresponsive to print jobs and displays an error code.",
                Priority = "High",
                Status = "Open",
                CreatedBy = "Anna Schmidt",
                AssignedTo = "Unassigned",
                CreatedAt = DateTime.UtcNow.AddHours(-3)
            },
            new()
            {
                Title = "VPN connection keeps dropping",
                Description = "After approx. 10 minutes, the VPN connection to the corporate network drops automatically.",
                Priority = "Medium",
                Status = "InProgress",
                CreatedBy = "Max Mustermann",
                AssignedTo = "Roman",
                CreatedAt = DateTime.UtcNow.AddHours(-6)
            },
            new()
            {
                Title = "Password reset for ERP system",
                Description = "I forgot my password and can no longer log into my account.",
                Priority = "Low",
                Status = "Closed",
                CreatedBy = "Julia Weber",
                AssignedTo = "Roman",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                ResolvedAt = DateTime.UtcNow.AddHours(-2)
            }
        };

        await context.Tickets.AddRangeAsync(initialTickets);
        await context.SaveChangesAsync();
    }
}