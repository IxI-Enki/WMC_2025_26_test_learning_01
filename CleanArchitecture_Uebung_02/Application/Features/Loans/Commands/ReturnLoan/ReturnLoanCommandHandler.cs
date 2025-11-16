using Application.Common.Models;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Loans.Commands.ReturnLoan;

/// <summary>
/// Command-Handler zum Zurückgeben einer Ausleihe.
/// Lädt Loan mit Book, setzt ReturnDate und erhöht AvailableCopies.
/// </summary>
public sealed class ReturnLoanCommandHandler(IUnitOfWork uow) 
    : IRequestHandler<ReturnLoanCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ReturnLoanCommand request, 
        CancellationToken cancellationToken)
    {
        // 1. Loan mit Book laden (für AvailableCopies Update)
        var loan = await uow.Loans.GetByIdAsync(request.LoanId, cancellationToken);
        if (loan == null)
            return Result<bool>.NotFound($"Loan with ID {request.LoanId} not found.");
        
        // 2. Prüfen ob bereits zurückgegeben
        if (loan.ReturnDate != null)
            return Result<bool>.ValidationError("Loan was already returned.");
        
        // 3. ReturnDate setzen
        loan.MarkAsReturned(request.ReturnDate);
        
        // 4. AvailableCopies erhöhen
        loan.Book.IncreaseCopies();
        
        // 5. Speichern
        await uow.SaveChangesAsync(cancellationToken);
        
        return Result<bool>.NoContent();
    }
}

