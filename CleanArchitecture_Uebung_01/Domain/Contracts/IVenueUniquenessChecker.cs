namespace Domain.Contracts;

/// <summary>
/// Contract für die Eindeutigkeitsprüfung von Venues.
/// Wird vom Domain Layer definiert, aber vom Application/Infrastructure Layer implementiert.
/// </summary>
public interface IVenueUniquenessChecker
{
    /// <summary>
    /// Prüft, ob eine Venue mit dem gegebenen Namen bereits existiert.
    /// </summary>
    /// <param name="id">ID der aktuellen Venue (0 bei Neuanlage).</param>
    /// <param name="name">Name der Venue.</param>
    /// <param name="ct">Cancellation Token.</param>
    /// <returns>True, wenn die Venue eindeutig ist.</returns>
    Task<bool> IsUniqueAsync(int id, string name, CancellationToken ct = default);
}
