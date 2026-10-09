using System.ComponentModel.DataAnnotations;

namespace Helpdesk.Api.Models;

public class Ticket
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Заголовок обязателен для заполнения")]
    [StringLength(100, ErrorMessage = "Заголовок не должен превышать 100 символов")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Описание обязательно для заполнения")]
    public string Description { get; set; } = string.Empty;

    public string Status { get; set; } = "Open"; // Open, InProgress, Closed

    [Required]
    public string Priority { get; set; } = "Medium"; // Low, Medium, High

    public string? CreatedBy { get; set; }
    public string? AssignedTo { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ResolvedAt { get; set; }
}