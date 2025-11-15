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

namespace Application.Features.Authors.Queries;
public record GetAllAuthorsQueryHandler( IUnitOfWork uow ) :
        IRequestHandler<GetAllAuthorsQuery, Result<IReadOnlyCollection<GetAuthorDto>>>
{
    public async Task<Result<IReadOnlyCollection<GetAuthorDto>>>
        Handle( GetAllAuthorsQuery request, CancellationToken cancellationToken )
    {
        var entities = await uow.Authors.GetAllAsync(ct: cancellationToken );

        var dtos = entities.Adapt<IReadOnlyCollection<GetAuthorDto>>( );

        return Result<IReadOnlyCollection<GetAuthorDto>>.Success( dtos );

    }
}