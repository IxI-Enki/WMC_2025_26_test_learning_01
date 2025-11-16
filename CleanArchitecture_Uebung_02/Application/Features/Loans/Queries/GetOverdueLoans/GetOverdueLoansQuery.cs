using Application.Common.Models;
using Application.Dtos;
using MediatR;

namespace Application.Features.Loans.Queries.GetOverdueLoans;

/// <summary>
/// Query zum Abrufen aller überfälligen Ausleihen.
/// </summary>
public readonly record struct GetOverdueLoansQuery() 
    : IRequest<Result<IReadOnlyCollection<GetLoanDto>>>;

