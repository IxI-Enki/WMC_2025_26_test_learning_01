namespace Application.Features.Dtos;

public record GetTicketDto(int Id, int EventId, string BuyerName, DateTime PurchaseDate, decimal Price);
