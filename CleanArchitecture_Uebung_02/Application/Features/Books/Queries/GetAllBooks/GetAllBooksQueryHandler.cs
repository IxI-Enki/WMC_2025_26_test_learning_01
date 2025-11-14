using Application.Common.Models;
using Application.Dtos;
using Application.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Books.Queries.GetAllBooks;

/// <summary>
/// TODO: Implementiere den GetAllBooksQueryHandler.
/// </summary>
public sealed class GetAllBooksQueryHandler(IUnitOfWork uow) 
    : IRequestHandler<GetAllBooksQuery, Result<IEnumerable<GetBookDto>>>
{
    public async Task<Result<IEnumerable<GetBookDto>>> Handle(GetAllBooksQuery request, 
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException("GetAllBooksQueryHandler muss noch implementiert werden!");
    }
}

