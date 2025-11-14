using Domain.Common;

namespace Domain.ValidationSpecifications;

public static class EventSpecifications
{
    public const int NameMinLength = 3;
    public const int MaxAttendeesMin = 1;

    /// <summary>
    /// TODO: Implementiere die Validierung für VenueId.
    /// VenueId muss größer als 0 sein.
    /// Bei Fehler: "VenueId muss größer als 0 sein."
    /// </summary>
    public static DomainValidationResult CheckVenueId(int venueId)
    {
        // TODO: Implementiere diese Methode
        throw new NotImplementedException("CheckVenueId muss noch implementiert werden!");
    }

    public static DomainValidationResult CheckName(string name) =>
        string.IsNullOrWhiteSpace(name)
            ? DomainValidationResult.Failure("Name", "Name darf nicht leer sein.")
            : name.Trim().Length < NameMinLength
                ? DomainValidationResult.Failure("Name", $"Name muss mindestens {NameMinLength} Zeichen haben.")
                : DomainValidationResult.Success("Name");

    /// <summary>
    /// TODO: Implementiere die Validierung für DateTime.
    /// DateTime darf nicht in der Vergangenheit liegen.
    /// Bei Fehler: "DateTime darf nicht in der Vergangenheit liegen."
    /// Verwende DateTime.Now für den Vergleich.
    /// </summary>
    public static DomainValidationResult CheckDateTime(DateTime dateTime)
    {
        // TODO: Implementiere diese Methode
        throw new NotImplementedException("CheckDateTime muss noch implementiert werden!");
    }

    public static DomainValidationResult CheckMaxAttendees(int maxAttendees) =>
        maxAttendees < MaxAttendeesMin
            ? DomainValidationResult.Failure("MaxAttendees", $"MaxAttendees muss mindestens {MaxAttendeesMin} sein.")
            : DomainValidationResult.Success("MaxAttendees");

    /// <summary>
    /// TODO: Implementiere die Validierung, dass MaxAttendees die Venue-Kapazität nicht überschreitet.
    /// Bei Fehler: "MaxAttendees darf die Venue-Kapazität nicht überschreiten."
    /// </summary>
    public static DomainValidationResult CheckMaxAttendeesNotExceedVenueCapacity(int maxAttendees, int venueCapacity)
    {
        // TODO: Implementiere diese Methode
        throw new NotImplementedException("CheckMaxAttendeesNotExceedVenueCapacity muss noch implementiert werden!");
    }
}
