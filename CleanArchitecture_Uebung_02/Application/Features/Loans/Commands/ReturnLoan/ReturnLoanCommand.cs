using Application.Common.Models;
using MediatR;

namespace Application.Features.Loans.Commands.ReturnLoan;

/// <summary>
/// Command zum Zurückgeben einer Ausleihe.
/// Setzt das ReturnDate und erhöht AvailableCopies wieder.
/// </summary>
public readonly record struct ReturnLoanCommand(
    int LoanId,
    DateTime ReturnDate
) : IRequest<Result<bool>>;

