using Application.Common.Models;
using Application.Dtos;
using Application.Interfaces;
using Mapster;
using MediatR;

namespace Application.Features.Books.Queries.GetBookById;

/// <summary>
/// TODO: Implementiere den GetBookByIdQueryHandler.
/// </summary>
public sealed class GetBookByIdQueryHandler(IUnitOfWork uow) 
    : IRequestHandler<GetBookByIdQuery, Result<GetBookDto>>
{
    public async Task<Result<GetBookDto>> Handle(GetBookByIdQuery request, 
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException("GetBookByIdQueryHandler muss noch implementiert werden!");
    }
}

