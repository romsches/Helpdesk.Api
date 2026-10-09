using FluentValidation;
using Helpdesk.Api.Models;

namespace Helpdesk.Api.Validators;

public class TicketValidator : AbstractValidator<Ticket>
{
    public TicketValidator()
    {
        RuleFor(t => t.Title)
            .NotEmpty().WithMessage("Заголовок обязателен для заполнения.")
            .MaximumLength(100).WithMessage("Заголовок не должен превышать 100 символов.");

        RuleFor(t => t.Description)
            .NotEmpty().WithMessage("Описание обязательно для заполнения.");

        RuleFor(t => t.Priority)
            .NotEmpty().WithMessage("Приоритет обязателен.")
            .Must(p => new[] { "Low", "Medium", "High" }.Contains(p))
            .WithMessage("Приоритет должен быть одним из следующих: Low, Medium, High.");

        RuleFor(t => t.Status)
            .Must(s => new[] { "Open", "InProgress", "Closed" }.Contains(s))
            .WithMessage("Статус должен быть: Open, InProgress или Closed.");
    }
}