using Application.Features.[ENTITY_PLURAL].Commands.Create[ENTITY_NAME];
using Application.Features.[ENTITY_PLURAL].Commands.Delete[ENTITY_NAME];
using Application.Features.[ENTITY_PLURAL].Commands.Update[ENTITY_NAME];
using Application.Features.[ENTITY_PLURAL].Queries.GetAll[ENTITY_PLURAL];
using Application.Features.[ENTITY_PLURAL].Queries.Get[ENTITY_NAME]ById;
using Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Controller für [ENTITY_NAME] Operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class [ENTITY_PLURAL]Controller(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Erstellt einen neuen [ENTITY_NAME].
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Get[ENTITY_NAME]Dto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] Create[ENTITY_NAME]Command command,
        CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return result.ToActionResult(
            this,
            createdAtAction: nameof(GetById),
            routeValues: new { id = result?.Value?.Id });
    }

    /// <summary>
    /// Ruft alle [ENTITY_PLURAL] ab.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<Get[ENTITY_NAME]Dto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAll[ENTITY_PLURAL]Query(), ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Ruft einen [ENTITY_NAME] nach ID ab.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Get[ENTITY_NAME]Dto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new Get[ENTITY_NAME]ByIdQuery(id), ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Aktualisiert einen [ENTITY_NAME].
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Get[ENTITY_NAME]Dto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] Update[ENTITY_NAME]Command command,
        CancellationToken ct)
    {
        if (id != command.Id)
            return BadRequest("Route ID und Command ID stimmen nicht überein.");

        var result = await mediator.Send(command, ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Löscht einen [ENTITY_NAME].
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new Delete[ENTITY_NAME]Command(id), ct);
        return result.ToActionResult(this);
    }
}

