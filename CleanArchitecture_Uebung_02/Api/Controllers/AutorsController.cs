using Api.Extensions;
using Application.Dtos;
using Application.Features.Authors.Queries.GetAllAuthors;
using Application.Features.Authors.Queries.GetAuthorById;
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
        var result = await mediator.Send( new GetAllAuthorsQuery(), ct );

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
        var result = await mediator.Send( new GetAuthorByIdQuery(id), ct );

        return result.ToActionResult( this );
    }

}

