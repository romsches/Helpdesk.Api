using FluentValidation;
using Helpdesk.Api.Models;

namespace Helpdesk.Api.Validators;

/// <summary>
/// Validierungsregeln für die Ticket-Entität unter Verwendung von FluentValidation.
/// Garantiert die Integrität der Eingabedaten vor der Verarbeitung durch die Geschäftslogik.
/// </summary>
public class TicketValidator : AbstractValidator<Ticket>
{
    private static readonly string[] AllowedPriorities = { "Low", "Medium", "High" };
    private static readonly string[] AllowedStatuses = { "Open", "InProgress", "Closed" };

    public TicketValidator()
    {
        // Validierung für den Ticket-Titel
        RuleFor(t => t.Title)
            .NotEmpty().WithMessage("Der Titel ist ein Pflichtfeld und darf nicht leer sein.")
            .MaximumLength(100).WithMessage("Der Titel darf maximal 100 Zeichen lang sein.");

        // Validierung für die Ticket-Beschreibung
        RuleFor(t => t.Description)
            .NotEmpty().WithMessage("Die Beschreibung ist ein Pflichtfeld und darf nicht leer sein.");

        // Validierung für die Priorität (ignoriert Groß-/Kleinschreibung)
        RuleFor(t => t.Priority)
            .NotEmpty().WithMessage("Die Priorität ist ein Pflichtfeld.")
            .Must(p => AllowedPriorities.Contains(p, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Die Priorität muss einen der folgenden Werte haben: Low, Medium, High.");

        // Validierung für den Status (ignoriert Groß-/Kleinschreibung)
        RuleFor(t => t.Status)
            .NotEmpty().WithMessage("Der Status ist ein Pflichtfeld.")
            .Must(s => AllowedStatuses.Contains(s, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Der Status muss einen der folgenden Werte haben: Open, InProgress, Closed.");
    }
}