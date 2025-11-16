using Api.Extensions;
using Application.Common.Models;
using Application.Dtos;
using Application.Features.Books.Commands.CreateBook;
using Application.Features.Books.Commands.DeleteBook;
using Application.Features.Books.Commands.UpdateBook;
using Application.Features.Books.Queries.GetAllBooks;
using Application.Features.Books.Queries.GetBookById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Endpunkte rund um Bücher.
/// </summary>
[ApiController]
[Route( "api/[controller]" )]
public class BooksController( IMediator mediator ) : ControllerBase
{
    /// <summary>
    /// GET /api/books - GetAll
    /// </summary>
    [HttpGet]
    [ProducesResponseType( typeof( IEnumerable<GetBookDto> ), StatusCodes.Status200OK )]
    public async Task<IActionResult> GetAll( CancellationToken ct )
    {
        var result = await mediator.Send(new GetAllBooksQuery(), ct);

        return result.ToActionResult( this );
    }

    /// <summary>
    /// GET /api/books/{id} - GetById
    /// </summary>
    [HttpGet( "{id:int}" )]
    [ProducesResponseType( typeof( GetBookDto ), StatusCodes.Status200OK )]
    [ProducesResponseType( StatusCodes.Status404NotFound )]
    public async Task<IActionResult> GetById( int id, CancellationToken ct )
    {
        var result = await mediator.Send(new GetBookByIdQuery(id), ct);

        return result.ToActionResult( this );
    }

    /// <summary>
    /// TODO: Implementiere POST /api/books - CreateAsync
    /// </summary>
    [HttpPost]
    [ProducesResponseType( typeof( GetBookDto ), StatusCodes.Status201Created )]
    [ProducesResponseType( StatusCodes.Status400BadRequest )]
    [ProducesResponseType( StatusCodes.Status409Conflict )]
    public async Task<IActionResult> Create( [FromBody] CreateBookCommand command, CancellationToken ct )
    {
        var result = await mediator.Send(command, ct);
        return result.ToActionResult(this, createdAtAction: nameof(GetById), 
            routeValues: new { id = result?.Value?.Id });
    }

    /// <summary>
    /// PUT /api/books/{id} - Update
    /// </summary>
    [HttpPut( "{id:int}" )]
    [ProducesResponseType( typeof( GetBookDto ), StatusCodes.Status200OK )]
    [ProducesResponseType( StatusCodes.Status400BadRequest )]
    [ProducesResponseType( StatusCodes.Status404NotFound )]
    [ProducesResponseType( StatusCodes.Status409Conflict )]
    public async Task<IActionResult> Update( int id, [FromBody] UpdateBookCommand command, CancellationToken ct )
    {
        if (id != command.Id)
        {
            Result<GetBookDto> badResult = Result<GetBookDto>.ValidationError(
                "The route ID does not match the book ID in the request body.");
            return badResult.ToActionResult(this);
        }
        
        var result = await mediator.Send(command, ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// DELETE /api/books/{id} - Delete
    /// </summary>
    [HttpDelete( "{id:int}" )]
    [ProducesResponseType( StatusCodes.Status204NoContent )]
    [ProducesResponseType( StatusCodes.Status404NotFound )]
    public async Task<IActionResult> Delete( int id, CancellationToken ct )
    {
        var result = await mediator.Send( new DeleteBookCommand( id ) , ct );
        return result.ToActionResult( this );
    }
}
