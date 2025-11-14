namespace Domain.Common;

/// <summary>
/// Repräsentiert das Ergebnis einer Domain-Validierung.
/// </summary>
public readonly record struct DomainValidationResult
{
    public string Property { get; init; }
    public bool IsValid { get; init; }
    public string? ErrorMessage { get; init; }

    public static DomainValidationResult Success(string property) => new()
    {
        Property = property,
        IsValid = true,
        ErrorMessage = null
    };

    public static DomainValidationResult Failure(string property, string errorMessage) => new()
    {
        Property = property,
        IsValid = false,
        ErrorMessage = errorMessage
    };
}
