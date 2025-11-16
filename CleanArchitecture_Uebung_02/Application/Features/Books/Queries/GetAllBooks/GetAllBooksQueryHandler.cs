using Application.Common.Models;
using Application.Dtos;
using Application.Interfaces;
using Mapster;
using MediatR;

namespace Application.Features.Books.Queries.GetAllBooks;

/// <summary>
/// Handler zum Abrufen aller Bücher.
/// </summary>
public sealed class GetAllBooksQueryHandler(IUnitOfWork uow) 
    : IRequestHandler<GetAllBooksQuery, Result<IReadOnlyCollection<GetBookDto>>>
{
    public async Task<Result<IReadOnlyCollection<GetBookDto>>>
        Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
    {
        var entities = await uow.Books.GetAllAsync(ct: cancellationToken);
        var dtos = entities.Adapt<IReadOnlyCollection<GetBookDto>>();
        return Result<IReadOnlyCollection<GetBookDto>>.Success(dtos);
    }
}
