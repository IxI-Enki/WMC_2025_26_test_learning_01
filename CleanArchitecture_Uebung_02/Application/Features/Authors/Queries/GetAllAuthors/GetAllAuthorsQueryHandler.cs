using Application.Common.Models;
using Application.Dtos;
using Application.Interfaces;
using Mapster;
using MediatR;

namespace Application.Features.Authors.Queries.GetAllAuthors;

/// <summary>
/// Handler zum Abrufen aller Autoren.
/// </summary>
public sealed class GetAllAuthorsQueryHandler(IUnitOfWork uow) 
    : IRequestHandler<GetAllAuthorsQuery, Result<IReadOnlyCollection<GetAuthorDto>>>
{
    public async Task<Result<IReadOnlyCollection<GetAuthorDto>>> 
        Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
    {
        var authors = await uow.Authors.GetAllAsync(
            orderBy: q => q.OrderBy(s => s.FirstName).ThenBy(s => s.LastName), ct: cancellationToken);

        var dtos = authors.Adapt<IReadOnlyCollection<GetAuthorDto>>();

        return Result<IReadOnlyCollection<GetAuthorDto>>.Success(dtos);
    }
}