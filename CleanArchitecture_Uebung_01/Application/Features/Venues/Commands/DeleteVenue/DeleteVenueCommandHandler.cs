using Application.Common.Results;
using Application.Contracts;
using MediatR;

namespace Application.Features.Venues.Commands.DeleteVenue;

/// <summary>
/// Command-Handler zum Löschen einer Venue.
/// </summary>
public sealed class DeleteVenueCommandHandler(IUnitOfWork uow) 
    : IRequestHandler<DeleteVenueCommand, Result>
{
    public async Task<Result> Handle(DeleteVenueCommand request, CancellationToken cancellationToken)
    {
        var entity = await uow.Venues.GetByIdAsync(request.Id, cancellationToken);
        
        if (entity == null)
            return Result.NotFound($"Venue with ID {request.Id} not found.");
        
        uow.Venues.Delete(entity);
        await uow.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
