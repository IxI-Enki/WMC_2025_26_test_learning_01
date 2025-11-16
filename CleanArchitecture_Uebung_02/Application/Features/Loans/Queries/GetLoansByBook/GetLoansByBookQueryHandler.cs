using Application.Common.Models;
using Application.Dtos;
using Application.Interfaces;
using Mapster;
using MediatR;

namespace Application.Features.Loans.Queries.GetLoansByBook;

/// <summary>
/// Handler für GetLoansByBookQuery.
/// Gibt alle Ausleihen für ein bestimmtes Buch zurück.
/// </summary>
public sealed class GetLoansByBookQueryHandler(IUnitOfWork uow) 
    : IRequestHandler<GetLoansByBookQuery, Result<IReadOnlyCollection<GetLoanDto>>>
{
    public async Task<Result<IReadOnlyCollection<GetLoanDto>>> Handle(
        GetLoansByBookQuery request, 
        CancellationToken cancellationToken)
    {
        var loans = await uow.Loans.GetLoansByBookIdAsync(request.BookId, cancellationToken);
        var dtos = loans.Adapt<IReadOnlyCollection<GetLoanDto>>();
        return Result<IReadOnlyCollection<GetLoanDto>>.Success(dtos);
    }
}

