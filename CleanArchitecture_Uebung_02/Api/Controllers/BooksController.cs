using Api.Extensions;
using Application.Dtos;
using Application.Features.Books.Commands.CreateBook;
using Application.Features.Books.Commands.DeleteBook;
using Application.Features.Books.Queries.GetAllBooks;
using Application.Features.Books.Queries.GetBookById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Endpunkte rund um Bücher.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BooksController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Liefert alle Bücher sortiert nach Titel.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetBookDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllBooksQuery(), ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Liefert ein Buch per ID.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GetBookDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetBookByIdQuery(id), ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Legt ein neues Buch an.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(GetBookDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateBookCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return result.ToActionResult(this, createdAtAction: nameof(GetById), 
            routeValues: new { id = result?.Value?.Id });
    }

    /// <summary>
    /// Löscht ein Buch.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteBookCommand(id), ct);
        return result.ToActionResult(this);
    }
}

