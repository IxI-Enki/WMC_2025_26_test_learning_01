using Application.Common.Models;
using Application.Dtos;
using Application.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Books.Queries.GetAllBooks;

public sealed class GetAllBooksQueryHandler(IUnitOfWork uow) 
    : IRequestHandler<GetAllBooksQuery, Result<IEnumerable<GetBookDto>>>
{
    public async Task<Result<IEnumerable<GetBookDto>>> Handle(GetAllBooksQuery request, 
        CancellationToken cancellationToken)
    {
        var entities = await uow.Books.GetAllAsync(
            cancellationToken, 
            orderBy: q => q.OrderBy(b => b.Title));
        
        var dtos = entities.Select(b => b.Adapt<GetBookDto>() with { AuthorName = b.Author.FullName });
        
        return Result<IEnumerable<GetBookDto>>.Success(dtos);
    }
}

