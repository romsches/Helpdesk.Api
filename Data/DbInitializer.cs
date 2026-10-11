using Helpdesk.Api.Models;

namespace Helpdesk.Api.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        // Проверяем, есть ли уже тикеты в базе
        if (context.Tickets.Any())
        {
            return; // База уже заполнена
        }

        var tickets = new Ticket[]
        {
            new Ticket
            {
                Title = "Drucker im 2. Stock funktioniert nicht",
                Description = "Der Netzwerkdrucker HP LaserJet reagiert nicht auf Druckaufträge und zeigt einen Fehlercode an.",
                Priority = "High",
                Status = "Open",
                CreatedBy = "Anna Schmidt",
                AssignedTo = "Unassigned",
                CreatedAt = DateTime.UtcNow.AddHours(-3)
            },
            new Ticket
            {
                Title = "VPN Verbindung bricht ab",
                Description = "Nach ca. 10 Minuten trennt sich die VPN-Verbindung zum Firmennetzwerk automatisch.",
                Priority = "Medium",
                Status = "InProgress",
                CreatedBy = "Max Mustermann",
                AssignedTo = "Roman",
                CreatedAt = DateTime.UtcNow.AddHours(-6)
            },
            new Ticket
            {
                Title = "Passwort zurücksetzen für ERP-System",
                Description = "Ich habe mein Passwort vergessen und kann mich nicht mehr anmelden.",
                Priority = "Low",
                Status = "Closed",
                CreatedBy = "Julia Weber",
                AssignedTo = "Roman",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                ResolvedAt = DateTime.UtcNow.AddHours(-2)
            }
        };

        context.Tickets.AddRange(tickets);
        context.SaveChanges();
    }
}