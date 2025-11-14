using Application.Common.Models;
using Application.Dtos;
using Application.Interfaces;
using Mapster;
using MediatR;

namespace Application.Features.Books.Queries.GetBookById;

public sealed class GetBookByIdQueryHandler(IUnitOfWork uow) 
    : IRequestHandler<GetBookByIdQuery, Result<GetBookDto>>
{
    public async Task<Result<GetBookDto>> Handle(GetBookByIdQuery request, 
        CancellationToken cancellationToken)
    {
        var entity = await uow.Books.GetByIdAsync(request.Id, cancellationToken);
        
        if (entity == null)
            return Result<GetBookDto>.NotFound($"Book with ID {request.Id} not found.");
        
        var dto = entity.Adapt<GetBookDto>() with { AuthorName = entity.Author.FullName };
        return Result<GetBookDto>.Success(dto);
    }
}

