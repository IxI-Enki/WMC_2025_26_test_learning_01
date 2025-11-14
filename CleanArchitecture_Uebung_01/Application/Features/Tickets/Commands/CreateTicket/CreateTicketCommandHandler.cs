using Application.Common.Exceptions;
using Application.Common.Results;
using Application.Contracts;
using Application.Features.Dtos;
using Domain.Entities;
using Mapster;
using MediatR;

namespace Application.Features.Tickets.Commands.CreateTicket;

public sealed class CreateTicketCommandHandler(IUnitOfWork uow) 
    : IRequestHandler<CreateTicketCommand, Result<GetTicketDto>>
{
    public async Task<Result<GetTicketDto>> Handle(CreateTicketCommand request, 
        CancellationToken cancellationToken)
    {
        var eventEntity = await uow.Events.GetByIdAsync(request.EventId, cancellationToken);
        if (eventEntity == null)
            throw new NotFoundException($"Event with ID {request.EventId} not found.");

        var entity = Ticket.Create(eventEntity, request.BuyerName, DateTime.Now, request.Price);
        await uow.Tickets.AddAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);
        
        return Result<GetTicketDto>.Created(entity.Adapt<GetTicketDto>());
    }
}
