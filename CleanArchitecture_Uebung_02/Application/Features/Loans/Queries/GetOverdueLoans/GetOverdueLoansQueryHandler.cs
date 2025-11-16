using Application.Common.Models;
using Application.Dtos;
using Application.Interfaces;
using Mapster;
using MediatR;

namespace Application.Features.Loans.Queries.GetOverdueLoans;

/// <summary>
/// Handler für GetOverdueLoansQuery.
/// Gibt alle überfälligen Ausleihen zurück (ReturnDate == null && DueDate < DateTime.Now).
/// </summary>
public sealed class GetOverdueLoansQueryHandler(IUnitOfWork uow) 
    : IRequestHandler<GetOverdueLoansQuery, Result<IReadOnlyCollection<GetLoanDto>>>
{
    public async Task<Result<IReadOnlyCollection<GetLoanDto>>> Handle(
        GetOverdueLoansQuery request, 
        CancellationToken cancellationToken)
    {
        var loans = await uow.Loans.GetOverdueLoansAsync(cancellationToken);
        var dtos = loans.Adapt<IReadOnlyCollection<GetLoanDto>>();
        return Result<IReadOnlyCollection<GetLoanDto>>.Success(dtos);
    }
}

