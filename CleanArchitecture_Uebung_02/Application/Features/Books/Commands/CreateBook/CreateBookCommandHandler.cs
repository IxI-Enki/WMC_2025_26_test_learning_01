using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Dtos;
using Application.Interfaces;
using Domain.Contracts;
using Domain.Entities;
using Mapster;
using MediatR;

namespace Application.Features.Books.Commands.CreateBook;

public sealed class CreateBookCommandHandler(IUnitOfWork uow, IBookUniquenessChecker uniquenessChecker) 
    : IRequestHandler<CreateBookCommand, Result<GetBookDto>>
{
    public async Task<Result<GetBookDto>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var author = await uow.Authors.GetByIdAsync(request.AuthorId, cancellationToken);
        if (author == null)
            throw new NotFoundException($"Author with ID {request.AuthorId} not found.");

        var entity = await Book.CreateAsync(request.ISBN, request.Title, author, request.PublicationYear, 
            request.AvailableCopies, uniquenessChecker, cancellationToken);
        
        await uow.Books.AddAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);
        
        var dto = entity.Adapt<GetBookDto>();
        return Result<GetBookDto>.Created(dto with { AuthorName = author.FullName });
    }
}

