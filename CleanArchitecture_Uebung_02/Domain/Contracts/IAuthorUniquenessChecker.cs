namespace Domain.Contracts;

/// <summary>
/// Contract für die Eindeutigkeitsprüfung von Books (ISBN).
/// </summary>
public interface IAuthorUniquenessChecker
{
    /// <summary>
    /// Prüft, ob ein Author mit FullName und DateOfBirth eindeutig ist.
    /// </summary>
    /// <param name="id">ID des aktuellen Authors (0 bei Neuanlage).</param>
    /// <param name="fullName">FullName des Authors.</param>
    /// <param name="ct">Cancellation Token.</param>
    /// <returns>True, wenn das Buch eindeutig ist.</returns>
    Task<bool> IsUniqueAsync(int id, string fullName, CancellationToken ct = default);
}

