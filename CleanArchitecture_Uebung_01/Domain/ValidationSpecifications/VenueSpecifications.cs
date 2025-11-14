using Domain.Common;

namespace Domain.ValidationSpecifications;

public static class VenueSpecifications
{
    public const int NameMinLength = 3;
    public const int AddressMinLength = 5;
    public const int CapacityMin = 1;

    public static DomainValidationResult CheckName(string name) =>
        string.IsNullOrWhiteSpace(name)
            ? DomainValidationResult.Failure("Name", "Name darf nicht leer sein.")
            : name.Trim().Length < NameMinLength
                ? DomainValidationResult.Failure("Name", $"Name muss mindestens {NameMinLength} Zeichen haben.")
                : DomainValidationResult.Success("Name");

    public static DomainValidationResult CheckAddress(string address) =>
        string.IsNullOrWhiteSpace(address)
            ? DomainValidationResult.Failure("Address", "Address darf nicht leer sein.")
            : address.Trim().Length < AddressMinLength
                ? DomainValidationResult.Failure("Address", $"Address muss mindestens {AddressMinLength} Zeichen haben.")
                : DomainValidationResult.Success("Address");

    public static DomainValidationResult CheckCapacity(int capacity) =>
        capacity < CapacityMin
            ? DomainValidationResult.Failure("Capacity", $"Capacity muss mindestens {CapacityMin} sein.")
            : DomainValidationResult.Success("Capacity");
}
