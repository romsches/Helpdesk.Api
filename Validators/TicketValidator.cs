using FluentValidation;
using Helpdesk.Api.Models;

namespace Helpdesk.Api.Validators;

/// <summary>
/// Validierungsregeln für die Ticket-Entität unter Verwendung von FluentValidation.
/// Garantiert die Integrität der Eingabedaten vor der Verarbeitung durch die Geschäftslogik.
/// </summary>
public class TicketValidator : AbstractValidator<Ticket>
{
    public TicketValidator()
    {
        // Validierung für den Ticket-Titel
        RuleFor(t => t.Title)
            .NotEmpty().WithMessage("Der Titel ist ein Pflichtfeld und darf nicht leer sein.")
            .MaximumLength(100).WithMessage("Der Titel darf maximal 100 Zeichen lang sein.");

        // Validierung für die Ticket-Beschreibung
        RuleFor(t => t.Description)
            .NotEmpty().WithMessage("Die Beschreibung ist ein Pflichtfeld und darf nicht leer sein.");

        // Validierung für die Priorität (zulässige Werte: Low, Medium, High)
        RuleFor(t => t.Priority)
            .NotEmpty().WithMessage("Die Priorität ist ein Pflichtfeld.")
            .Must(p => new[] { "Low", "Medium", "High" }.Contains(p))
            .WithMessage("Die Priorität muss einen der folgenden Werte haben: Low, Medium, High.");

        // Validierung für den Status (zulässige Werte: Open, InProgress, Closed)
        RuleFor(t => t.Status)
            .Must(s => new[] { "Open", "InProgress", "Closed" }.Contains(s))
            .WithMessage("Der Status muss einen der folgenden Werte haben: Open, InProgress, Closed.");
    }
}