using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Dtos;
using Application.Interfaces;
using Domain.Contracts;
using Domain.Entities;
using Mapster;
using MediatR;

namespace Application.Features.Books.Commands.CreateBook;

/// <summary>
/// TODO: Implementiere den CreateBookCommandHandler.
/// </summary>
public sealed class CreateBookCommandHandler(IUnitOfWork uow, IBookUniquenessChecker uniquenessChecker) 
    : IRequestHandler<CreateBookCommand, Result<GetBookDto>>
{
    public async Task<Result<GetBookDto>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException("CreateBookCommandHandler muss noch implementiert werden!");
    }
}

