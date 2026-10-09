using System.ComponentModel.DataAnnotations;

namespace Helpdesk.Api.Models;

/// <summary>
/// Repräsentiert ein Support-Ticket im System (Core Domain Model).
/// </summary>
public class Ticket
{
    /// <summary>
    /// Eindeutige Identifikationsnummer des Tickets (Primärschlüssel).
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Titel oder kurzer Betreff des Problems.
    /// </summary>
    [Required(ErrorMessage = "Der Titel ist ein Pflichtfeld.")]
    [StringLength(100, ErrorMessage = "Der Titel darf maximal 100 Zeichen lang sein.")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Detaillierte Beschreibung des Vorfalls oder der Anfrage.
    /// </summary>
    [Required(ErrorMessage = "Die Beschreibung ist ein Pflichtfeld.")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Aktueller Status des Tickets (Standard: 'Open').
    /// Mögliche Werte: Open, InProgress, Closed.
    /// </summary>
    public string Status { get; set; } = "Open";

    /// <summary>
    /// Prioritätsstufe des Tickets (Standard: 'Medium').
    /// Mögliche Werte: Low, Medium, High.
    /// </summary>
    [Required]
    public string Priority { get; set; } = "Medium";

    /// <summary>
    /// Benutzer, der das Ticket erstellt hat (mit Fallback auf 'Anonymous').
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Zuständiger Support-Mitarbeiter (mit Fallback auf 'Unassigned').
    /// </summary>
    public string? AssignedTo { get; set; }

    /// <summary>
    /// Zeitstempel der Erstellung in UTC.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Optionaler Zeitstempel für die Ticket-Lösung.
    /// </summary>
    public DateTime? ResolvedAt { get; set; }
}