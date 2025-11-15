using Api.Extensions;
using Application.Dtos;
using Application.Features.Authors.Queries;
using Application.Features.Books.Commands.CreateBook;
using Application.Features.Books.Commands.DeleteBook;
using Application.Features.Books.Queries.GetAllBooks;
using Application.Features.Books.Queries.GetBookById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Endpunkte rund um Autoren.
/// </summary>
[ApiController]
[Route( "api/[controller]" )]
public class AutorsController( IMediator mediator ) : ControllerBase
{
    /// <summary>
    /// GET /api/books - GetAll
    /// </summary>
    [HttpGet]
    [ProducesResponseType( typeof( IEnumerable<GetAuthorDto> ), StatusCodes.Status200OK )]
    public async Task<IActionResult> GetAll( CancellationToken ct )
    {
        //throw new NotImplementedException("BooksController.GetAll muss noch implementiert werden!");
        //var result = await mediator.Send(  ct);

        var result = await mediator.Send( new GetAllAuthorsQuery(), ct );

        return result.ToActionResult( this );
    }

    /// <summary>
    /// TODO: Implementiere GET /api/books/{id} - GetById
    /// </summary>
    [HttpGet( "{id:int}" )]
    [ProducesResponseType( typeof( GetBookDto ), StatusCodes.Status200OK )]
    [ProducesResponseType( StatusCodes.Status404NotFound )]
    public async Task<IActionResult> GetById( int id, CancellationToken ct )
    {
        throw new NotImplementedException( "BooksController.GetById muss noch implementiert werden!" );
    }

}

