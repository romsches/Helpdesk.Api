using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Helpdesk.Api.Data;
using Helpdesk.Api.Models;

namespace Helpdesk.Api.Controllers;

/// <summary>
/// Controller zur Verwaltung von Support-Tickets (RESTful Endpoints).
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class TicketsController : ControllerBase
{
    private readonly AppDbContext _context;

    public TicketsController(AppDbContext context)
    {
        _context = context;
    }

    // ---------------------------------------------------------------------------
    // GET: api/tickets
    // GET: api/tickets?status=Open
    // ---------------------------------------------------------------------------
    /// <summary>
    /// Ruft eine Liste aller Tickets ab, optional gefiltert nach Status.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets([FromQuery] string? status)
    {
        var query = _context.Tickets.AsQueryable();

        // Status-Filter dynamisch anwenden (case-insensitive)
        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(t => t.Status.ToLower() == status.ToLower());
        }

        return await query.ToListAsync();
    }

    // ---------------------------------------------------------------------------
    // GET: api/tickets/5
    // ---------------------------------------------------------------------------
    /// <summary>
    /// Ruft ein spezifisches Ticket anhand seiner eindeutigen ID ab.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Ticket>> GetTicket(int id)
    {
        var ticket = await _context.Tickets.FindAsync(id);

        if (ticket == null)
        {
            return NotFound();
        }

        return ticket;
    }

    // ---------------------------------------------------------------------------
    // POST: api/tickets
    // ---------------------------------------------------------------------------
    /// <summary>
    /// Erstellt ein neues Support-Ticket mit automatischen Fallback-Werten.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Ticket>> PostTicket(Ticket ticket)
    {
        // Fallback-Injektion zur Vermeidung von PostgreSQL NOT NULL-Constraint-Verletzungen
        if (string.IsNullOrEmpty(ticket.CreatedBy))
        {
            ticket.CreatedBy = "Anonymous";
        }

        if (string.IsNullOrEmpty(ticket.AssignedTo))
        {
            ticket.AssignedTo = "Unassigned";
        }

        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, ticket);
    }

    // ---------------------------------------------------------------------------
    // PUT: api/tickets/5
    // ---------------------------------------------------------------------------
    /// <summary>
    /// Aktualisiert ein bestehendes Ticket vollständig.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTicket(int id, Ticket ticket)
    {
        if (id != ticket.Id)
        {
            return BadRequest();
        }

        _context.Entry(ticket).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Tickets.Any(e => e.Id == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // ---------------------------------------------------------------------------
    // DELETE: api/tickets/5
    // ---------------------------------------------------------------------------
    /// <summary>
    /// Löscht ein Ticket unwiderruflich aus dem System.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTicket(int id)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
        {
            return NotFound();
        }

        _context.Tickets.Remove(ticket);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}