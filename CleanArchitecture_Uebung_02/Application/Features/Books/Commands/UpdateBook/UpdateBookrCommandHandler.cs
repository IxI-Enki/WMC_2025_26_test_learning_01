using Application.Common.Models;
using Application.Dtos;
using Application.Interfaces;
using Domain.Contracts;
using Mapster;
using MediatR;

namespace Application.Features.Books.Commands.UpdateBook
{
    public sealed class UpdateBookCommandHandler( IUnitOfWork uow, IBookUniquenessChecker uniquenessChecker )
        : IRequestHandler<UpdateBookCommand, Result<GetBookDto>>
    {
        // Kommunikation mittels Result-Objekt  ==> Exceptions abfangen und in Result umwandeln
        public async Task<Result<GetBookDto>> Handle( UpdateBookCommand request, CancellationToken cancellationToken )
        {
            // Sensor laden
            var entity = await uow.Books.GetByIdAsync(request.Id, cancellationToken);

            if(entity is null) return Result<GetBookDto>.NotFound( $"Book with ID {request.Id} not found." );
            // Update via domain method (added on Sensor)
            // Eindeutigkeit (Location, Name) wird über ISensorUniquenessChecker geprüft

            await entity.UpdateAsync( request.ISBN, request.Title, request.AuthorId, request.PublicationYear, request.AvailableCopies, uniquenessChecker, cancellationToken );

            uow.Books.Update( entity );
            await uow.SaveChangesAsync( cancellationToken );
            return Result<GetBookDto>.Success( entity.Adapt<GetBookDto>( ) );

        }
    }
}