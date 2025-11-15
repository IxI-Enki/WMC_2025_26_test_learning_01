namespace Domain.Common;

/// <summary>
/// Repräsentiert das Ergebnis einer Domain-Validierung.
/// </summary>
public sealed record DomainValidationResult( bool IsValid, string Property, string? ErrorMessage )
{

    public static DomainValidationResult Success( string property ) => new(
        true,
        property,
        null
    );

    public static DomainValidationResult Failure( string property, string errorMessage ) => new(
        false,
        property,
        errorMessage
    );
}

