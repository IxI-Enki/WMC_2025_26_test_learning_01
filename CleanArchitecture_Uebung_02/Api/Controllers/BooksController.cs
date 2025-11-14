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
    /// TODO: Implementiere GET /api/books - GetAll
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetBookDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        throw new NotImplementedException("BooksController.GetAll muss noch implementiert werden!");
    }

    /// <summary>
    /// TODO: Implementiere GET /api/books/{id} - GetById
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GetBookDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        throw new NotImplementedException("BooksController.GetById muss noch implementiert werden!");
    }

    /// <summary>
    /// TODO: Implementiere POST /api/books - Create
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(GetBookDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateBookCommand command, CancellationToken ct)
    {
        throw new NotImplementedException("BooksController.Create muss noch implementiert werden!");
    }

    /// <summary>
    /// TODO: Implementiere DELETE /api/books/{id} - Delete
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        throw new NotImplementedException("BooksController.Delete muss noch implementiert werden!");
    }
}

