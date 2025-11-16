using Application.Common.Models;
using Application.Dtos;
using MediatR;

namespace Application.Features.Loans.Commands.CreateLoan;

/// <summary>
/// Command zum Erstellen einer neuen Ausleihe.
/// DueDate wird automatisch auf LoanDate + 14 Tage gesetzt.
/// </summary>
public readonly record struct CreateLoanCommand(
    int BookId,
    string BorrowerName,
    DateTime LoanDate
) : IRequest<Result<GetLoanDto>>;

