using Application.Common.Models;
using Application.Dtos;
using MediatR;

namespace Application.Features.Loans.Queries.GetLoansByBook;

/// <summary>
/// Query zum Abrufen aller Ausleihen für ein bestimmtes Buch.
/// </summary>
public readonly record struct GetLoansByBookQuery(int BookId) 
    : IRequest<Result<IReadOnlyCollection<GetLoanDto>>>;

