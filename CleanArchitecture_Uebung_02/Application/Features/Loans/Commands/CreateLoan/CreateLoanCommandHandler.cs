using Application.Common.Models;
using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Mapster;
using MediatR;

namespace Application.Features.Loans.Commands.CreateLoan;

/// <summary>
/// Command-Handler zum Erstellen einer neuen Ausleihe.
/// Lädt das Book aus der DB, erstellt die Loan-Entität via Factory-Methode,
/// reduziert AvailableCopies und speichert alles.
/// </summary>
public sealed class CreateLoanCommandHandler(IUnitOfWork uow) 
    : IRequestHandler<CreateLoanCommand, Result<GetLoanDto>>
{
    public async Task<Result<GetLoanDto>> Handle(CreateLoanCommand request, 
        CancellationToken cancellationToken)
    {
        // 1. Book aus DB laden
        var book = await uow.Books.GetByIdAsync(request.BookId, cancellationToken);
        if (book == null)
            return Result<GetLoanDto>.NotFound($"Book with ID {request.BookId} not found.");
        
        // 2. Prüfen ob Exemplare verfügbar
        if (book.AvailableCopies <= 0)
            return Result<GetLoanDto>.ValidationError("No copies available for loan.");
        
        // 3. Loan über Domänenlogik erstellen
        var loan = Loan.Create(book, request.BorrowerName, request.LoanDate);
        
        // 4. AvailableCopies reduzieren
        book.DecreaseCopies();
        
        // 5. Persistieren
        await uow.Loans.AddAsync(loan, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);
        
        // 6. DTO zurückgeben
        var dto = loan.Adapt<GetLoanDto>();
        return Result<GetLoanDto>.Created(dto);
    }
}

