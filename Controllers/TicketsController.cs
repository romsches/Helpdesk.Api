using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Helpdesk.Api.Data;
using Helpdesk.Api.Models;

namespace Helpdesk.Api.Controllers;

/// <summary>
/// RESTful controller for managing IT support tickets.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class TicketsController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    /// <summary>
    /// Retrieves a list of all support tickets, optionally filtered by status.
    /// </summary>
    /// <param name="status">Optional status filter (case-insensitive).</param>
    /// <returns>A collection of tickets matching the criteria.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Ticket>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets([FromQuery] string? status)
    {
        var query = _context.Tickets.AsQueryable();

        // Dynamically apply status filter if provided
        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(t => t.Status.ToLower() == status.ToLower());
        }

        var tickets = await query.AsNoTracking().ToListAsync();
        return Ok(tickets);
    }

    /// <summary>
    /// Retrieves a specific support ticket by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the ticket.</param>
    /// <returns>The requested ticket if found; otherwise, NotFound.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Ticket), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Ticket>> GetTicket(int id)
    {
        var ticket = await _context.Tickets.FindAsync(id);

        if (ticket == null)
        {
            return NotFound();
        }

        return Ok(ticket);
    }

    /// <summary>
    /// Creates a new support ticket with automatic fallback defaults.
    /// </summary>
    /// <param name="ticket">The ticket payload.</param>
    /// <returns>The created ticket with its assigned ID.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Ticket), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Ticket>> PostTicket([FromBody] Ticket ticket)
    {
        // Fallback injection to prevent PostgreSQL NOT NULL constraint violations
        if (string.IsNullOrWhiteSpace(ticket.CreatedBy))
        {
            ticket.CreatedBy = "Anonymous";
        }

        if (string.IsNullOrWhiteSpace(ticket.AssignedTo))
        {
            ticket.AssignedTo = "Unassigned";
        }

        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, ticket);
    }

    /// <summary>
    /// Updates the status of an existing ticket and optionally assigns an owner.
    /// </summary>
    /// <param name="id">The unique identifier of the ticket.</param>
    /// <param name="dto">The status update payload.</param>
    /// <returns>NoContent if successful; otherwise, NotFound.</returns>
    [HttpPatch("{id:int}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTicketStatus(int id, [FromBody] UpdateTicketStatusDto dto)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
        {
            return NotFound();
        }

        ticket.Status = dto.Status;

        if (!string.IsNullOrWhiteSpace(dto.AssignedTo))
        {
            ticket.AssignedTo = dto.AssignedTo;
        }

        if (dto.Status.Equals("Closed", StringComparison.OrdinalIgnoreCase) ||
            dto.Status.Equals("Resolved", StringComparison.OrdinalIgnoreCase))
        {
            ticket.ResolvedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Updates the assignee of an existing ticket.
    /// </summary>
    /// <param name="id">The unique identifier of the ticket.</param>
    /// <param name="dto">The assignee update payload.</param>
    /// <returns>NoContent if successful; otherwise, NotFound.</returns>
    [HttpPatch("{id:int}/assign")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTicketAssignee(int id, [FromBody] UpdateTicketAssigneeDto dto)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
        {
            return NotFound();
        }

        ticket.AssignedTo = string.IsNullOrWhiteSpace(dto.AssignedTo) ? "Unassigned" : dto.AssignedTo;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Fully replaces/updates an existing support ticket.
    /// </summary>
    /// <param name="id">The unique identifier of the ticket.</param>
    /// <param name="ticket">The updated ticket object.</param>
    /// <returns>NoContent if successful; otherwise, BadRequest or NotFound.</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutTicket(int id, [FromBody] Ticket ticket)
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
            if (!await TicketExistsAsync(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    /// <summary>
    /// Permanently deletes a support ticket from the system.
    /// </summary>
    /// <param name="id">The unique identifier of the ticket.</param>
    /// <returns>NoContent if successful; otherwise, NotFound.</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Checks whether a ticket exists asynchronously.
    /// </summary>
    private async Task<bool> TicketExistsAsync(int id)
    {
        return await _context.Tickets.AnyAsync(e => e.Id == id);
    }
}

/// <summary>
/// Data Transfer Object for updating a ticket's status and assignee.
/// </summary>
/// <param name="Status">The new ticket status.</param>
/// <param name="AssignedTo">Optional new assignee name.</param>
public record UpdateTicketStatusDto(string Status, string? AssignedTo);

/// <summary>
/// Data Transfer Object for updating a ticket's assignee.
/// </summary>
/// <param name="AssignedTo">The new assignee name.</param>
public record UpdateTicketAssigneeDto(string? AssignedTo);