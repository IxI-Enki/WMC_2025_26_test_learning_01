using Application.Common.Models;
using Application.Dtos;
using Application.Features.Books.Queries.GetAllBooks;
using Application.Interfaces;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Authors.Queries.GetAuthorById;

public sealed class GetAuthorByIdQueryHandler( IUnitOfWork uow ) 
    : IRequestHandler<GetAuthorByIdQuery, Result<GetAuthorDto>>
{
    public async Task<Result<GetAuthorDto>>
        Handle( GetAuthorByIdQuery request, CancellationToken cancellationToken )
    {
        var entity = await uow.Authors.GetByIdAsync(request.Id, ct: cancellationToken );

        if (entity == null)
            return Result<GetAuthorDto>.NotFound($"Author with ID {request.Id} not found.");

        return Result<GetAuthorDto>.Success( entity.Adapt<GetAuthorDto>( ) );
    }
}