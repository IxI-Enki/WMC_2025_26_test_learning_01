using Application.Common.Models;
using Application.Dtos;
using Application.Interfaces;
using Domain.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Books.Commands.CreateBook;
public class CreateBookCommandHandler( IUnitOfWork uow, IBookUniquenessChecker uniquenessChecker )
    : IRequestHandler<CreateBookCommand, Result<GetBookDto>>
{
    public async Task<Result<GetBookDto>> Handle( CreateBookCommand request,
        CancellationToken cancellationToken )
    {
        throw new NotImplementedException( );
    }
}