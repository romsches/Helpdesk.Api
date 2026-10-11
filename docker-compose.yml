using FluentValidation;
using Helpdesk.Api.Models;

namespace Helpdesk.Api.Validators;

/// <summary>
/// Defines validation rules for the <see cref="Ticket"/> entity using FluentValidation,
/// ensuring data integrity before processing by the business logic layer.
/// </summary>
public class TicketValidator : AbstractValidator<Ticket>
{
    private static readonly string[] AllowedPriorities = ["Low", "Medium", "High"];
    private static readonly string[] AllowedStatuses = ["Open", "InProgress", "Closed", "Resolved"];

    /// <summary>
    /// Initializes a new instance of the <see cref="TicketValidator"/> class.
    /// </summary>
    public TicketValidator()
    {
        // Validation for Ticket Title
        RuleFor(t => t.Title)
            .NotEmpty().WithMessage("The title is a required field and cannot be empty.")
            .MaximumLength(200).WithMessage("The title cannot exceed 200 characters.");

        // Validation for Ticket Description
        RuleFor(t => t.Description)
            .NotEmpty().WithMessage("The description is a required field and cannot be empty.");

        // Validation for Priority (case-insensitive)
        RuleFor(t => t.Priority)
            .NotEmpty().WithMessage("The priority is a required field.")
            .Must(p => AllowedPriorities.Contains(p, StringComparer.OrdinalIgnoreCase))
            .WithMessage("The priority must be one of the following values: Low, Medium, High.");

        // Validation for Status (case-insensitive)
        RuleFor(t => t.Status)
            .NotEmpty().WithMessage("The status is a required field.")
            .Must(s => AllowedStatuses.Contains(s, StringComparer.OrdinalIgnoreCase))
            .WithMessage("The status must be one of the following values: Open, InProgress, Closed, Resolved.");
    }
}