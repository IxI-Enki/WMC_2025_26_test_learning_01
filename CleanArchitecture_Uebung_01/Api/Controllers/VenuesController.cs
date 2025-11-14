using Api.Extensions;
using Application.Features.Dtos;
using Application.Features.Venues.Commands.CreateVenue;
using Application.Features.Venues.Commands.DeleteVenue;
using Application.Features.Venues.Commands.UpdateVenue;
using Application.Features.Venues.Queries.GetAllVenues;
using Application.Features.Venues.Queries.GetVenueById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Endpunkte rund um Veranstaltungsorte (Venues).
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class VenuesController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Liefert alle Venues sortiert nach Name.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetVenueDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllVenuesQuery(), ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// TODO: Implementiere den GetById Endpoint.
    /// 
    /// HINWEISE:
    /// - Route: [HttpGet("{id:int}")]
    /// - ProducesResponseType: typeof(GetVenueDto), 200 OK
    /// - ProducesResponseType: 404 Not Found
    /// - Sende GetVenueByIdQuery mit mediator.Send
    /// - Gib result.ToActionResult(this) zurück
    /// </summary>
    // TODO: Implementiere diese Methode

    /// <summary>
    /// Legt eine neue Venue an.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(GetVenueDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateVenueCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return result.ToActionResult(this, createdAtAction: nameof(GetById), 
            routeValues: new { id = result?.Value?.Id });
    }

    /// <summary>
    /// Aktualisiert eine Venue.
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(GetVenueDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateVenueCommand command, CancellationToken ct)
    {
        if (id != command.Id)
        {
            return BadRequest("Route ID does not match request body ID.");
        }
        var result = await mediator.Send(command, ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Löscht eine Venue.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteVenueCommand(id), ct);
        return result.ToActionResult(this);
    }
}
