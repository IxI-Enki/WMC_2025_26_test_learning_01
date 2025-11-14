using Domain.Common;

namespace Domain.ValidationSpecifications;

public static class TicketSpecifications
{
    public const int BuyerNameMinLength = 2;
    public const decimal PriceMin = 0.01m;

    public static DomainValidationResult CheckEventId(int eventId) =>
        eventId <= 0
            ? DomainValidationResult.Failure("EventId", "EventId muss größer als 0 sein.")
            : DomainValidationResult.Success("EventId");

    public static DomainValidationResult CheckBuyerName(string buyerName) =>
        string.IsNullOrWhiteSpace(buyerName)
            ? DomainValidationResult.Failure("BuyerName", "BuyerName darf nicht leer sein.")
            : buyerName.Trim().Length < BuyerNameMinLength
                ? DomainValidationResult.Failure("BuyerName", $"BuyerName muss mindestens {BuyerNameMinLength} Zeichen haben.")
                : DomainValidationResult.Success("BuyerName");

    public static DomainValidationResult CheckPurchaseDate(DateTime purchaseDate) =>
        purchaseDate > DateTime.Now
            ? DomainValidationResult.Failure("PurchaseDate", "PurchaseDate darf nicht in der Zukunft liegen.")
            : DomainValidationResult.Success("PurchaseDate");

    public static DomainValidationResult CheckPrice(decimal price) =>
        price < PriceMin
            ? DomainValidationResult.Failure("Price", $"Price muss mindestens {PriceMin} sein.")
            : DomainValidationResult.Success("Price");
}
