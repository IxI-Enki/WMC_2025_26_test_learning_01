using Application.Common.Models;
using Application.Dtos;
using Application.Interfaces;
using Domain.Contracts;
using Domain.Entities;
using Mapster;
using MediatR;

namespace Application.Features.Books.Commands.CreateBook;

/// <summary>
/// Command-Handler zum Erstellen eines neuen Buches.
/// Domänenvalidierungen erfolgen in der Entität (Factory-Methode) und werden ggf von Behavior
/// in Result<T> gemappt.
/// </summary>
public sealed class CreateBookCommandHandler( IUnitOfWork uow, IBookUniquenessChecker uniquenessChecker )
    : IRequestHandler<CreateBookCommand, Result<GetBookDto>>
{
    public async Task<Result<GetBookDto>> Handle( CreateBookCommand request,
        CancellationToken cancellationToken )
    {
        // 1. Author aus DB laden
        var author = await uow.Authors.GetByIdAsync(request.AuthorId, cancellationToken);
        if (author == null)
            return Result<GetBookDto>.NotFound($"Author with ID {request.AuthorId} not found.");
        
        // 2. Book über Domänenlogik erstellen
        var entity = await Book.CreateAsync(
            request.ISBN, 
            request.Title, 
            author, 
            request.PublicationYear, 
            request.AvailableCopies,
            uniquenessChecker, 
            cancellationToken);
        
        // 3. Persistieren
        await uow.Books.AddAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);
        
        // 4. DTO zurückgeben
        return Result<GetBookDto>.Created(entity.Adapt<GetBookDto>());
    }
}