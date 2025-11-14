namespace Domain.Contracts;

/// <summary>
/// Contract für die Eindeutigkeitsprüfung von Books (ISBN).
/// </summary>
public interface IBookUniquenessChecker
{
    /// <summary>
    /// Prüft, ob ein Buch mit der gegebenen ISBN bereits existiert.
    /// </summary>
    /// <param name="id">ID des aktuellen Buches (0 bei Neuanlage).</param>
    /// <param name="isbn">ISBN des Buches.</param>
    /// <param name="ct">Cancellation Token.</param>
    /// <returns>True, wenn das Buch eindeutig ist.</returns>
    Task<bool> IsUniqueAsync(int id, string isbn, CancellationToken ct = default);
}

