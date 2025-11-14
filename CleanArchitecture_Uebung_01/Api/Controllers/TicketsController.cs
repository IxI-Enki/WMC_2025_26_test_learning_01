using Api.Extensions;
using Application.Features.Dtos;
using Application.Features.Tickets.Commands.CreateTicket;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Endpunkte rund um Tickets.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TicketsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Erstellt ein neues Ticket für ein Event.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(GetTicketDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateTicketCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return result.ToActionResult(this);
    }
}
