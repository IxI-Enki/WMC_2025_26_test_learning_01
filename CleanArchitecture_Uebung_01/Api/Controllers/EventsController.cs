using Api.Extensions;
using Application.Features.Dtos;
using Application.Features.Events.Commands.CreateEvent;
using Application.Features.Events.Queries.GetAllEvents;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Endpunkte rund um Veranstaltungen (Events).
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EventsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Liefert alle Events sortiert nach DateTime.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetEventDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllEventsQuery(), ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// TODO: Implementiere den Create Endpoint für Events.
    /// 
    /// HINWEISE:
    /// - Route: [HttpPost]
    /// - ProducesResponseType: typeof(GetEventDto), 201 Created
    /// - ProducesResponseType: 400 Bad Request
    /// - ProducesResponseType: 404 Not Found (falls Venue nicht existiert)
    /// - Sende CreateEventCommand mit mediator.Send
    /// - Gib result.ToActionResult zurück
    /// </summary>
    // TODO: Implementiere diese Methode
}
