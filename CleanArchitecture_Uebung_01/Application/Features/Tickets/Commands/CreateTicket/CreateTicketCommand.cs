using Application.Common.Results;
using Application.Features.Dtos;
using MediatR;

namespace Application.Features.Tickets.Commands.CreateTicket;

public readonly record struct CreateTicketCommand(int EventId, string BuyerName, decimal Price) 
    : IRequest<Result<GetTicketDto>>;
