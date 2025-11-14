namespace Domain.Exceptions;

/// <summary>
/// Exception für Domain-Validierungsfehler.
/// </summary>
public class DomainValidationException : Exception
{
    public string Property { get; }

    public DomainValidationException(string property, string message) : base(message)
    {
        Property = property;
    }
}

