using Api.Extensions;
using Application.Dtos;
using Application.Features.Loans.Commands.CreateLoan;
using Application.Features.Loans.Commands.ReturnLoan;
using Application.Features.Loans.Queries.GetLoansByBook;
using Application.Features.Loans.Queries.GetOverdueLoans;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Endpunkte für Ausleihen (Loan Management).
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class LoansController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// POST /api/loans - CreateLoan (Buch ausleihen)
    /// </summary>
    /// <remarks>
    /// Erstellt eine neue Ausleihe. Reduziert automatisch AvailableCopies des Buchs.
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(typeof(GetLoanDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateLoan([FromBody] CreateLoanCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// PUT /api/loans/{id}/return - ReturnLoan (Buch zurückgeben)
    /// </summary>
    /// <remarks>
    /// Markiert eine Ausleihe als zurückgegeben. Erhöht automatisch AvailableCopies des Buchs.
    /// </remarks>
    [HttpPut("{id:int}/return")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReturnLoan(int id, [FromBody] DateTime returnDate, CancellationToken ct)
    {
        var command = new ReturnLoanCommand(id, returnDate);
        var result = await mediator.Send(command, ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// GET /api/loans/book/{bookId} - GetLoansByBook
    /// </summary>
    /// <remarks>
    /// Liefert alle Ausleihen für ein bestimmtes Buch (Historie).
    /// </remarks>
    [HttpGet("book/{bookId:int}")]
    [ProducesResponseType(typeof(IEnumerable<GetLoanDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLoansByBook(int bookId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetLoansByBookQuery(bookId), ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// GET /api/loans/overdue - GetOverdueLoans
    /// </summary>
    /// <remarks>
    /// Liefert alle überfälligen Ausleihen (ReturnDate == null && DueDate &lt; DateTime.Now).
    /// </remarks>
    [HttpGet("overdue")]
    [ProducesResponseType(typeof(IEnumerable<GetLoanDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOverdueLoans(CancellationToken ct)
    {
        var result = await mediator.Send(new GetOverdueLoansQuery(), ct);
        return result.ToActionResult(this);
    }
}

