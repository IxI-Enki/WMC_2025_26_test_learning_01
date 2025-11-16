using Application.Common.Models;
using Application.Dtos;
using Application.Features.Authors.Queries.GetAuthorById;
using Application.Features.Books.Queries.GetAllBooks;
using Application.Interfaces;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Authors.Queries.GetAllAuthors;

public sealed class GetAllAuthorsQueryHandler( IUnitOfWork uow ) 
    : IRequestHandler<GetAllAuthorsQuery, Result<IReadOnlyCollection<GetAuthorDto>>>
{
    public async Task<Result<IReadOnlyCollection<GetAuthorDto>>> 
        Handle( GetAllAuthorsQuery request, CancellationToken cancellationToken )
    {
        var authors = await uow.Authors.GetAllAsync(
            orderBy: q => q.OrderBy(s => s.FirstName).ThenBy(s => s.LastName),
            ct: cancellationToken );

        var dtos = authors.Adapt<IReadOnlyCollection<GetAuthorDto>>( );

        return Result<IReadOnlyCollection<GetAuthorDto>>.Success( dtos );
    }
}