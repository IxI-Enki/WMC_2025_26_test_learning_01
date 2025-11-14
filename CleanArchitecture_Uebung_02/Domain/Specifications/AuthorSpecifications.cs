using Domain.Common;
using Domain.Exceptions;

namespace Domain.Specifications;

public static class AuthorSpecifications
{
    public const int NameMinLength = 2;

    public static DomainValidationResult CheckFirstName(string firstName) =>
        string.IsNullOrWhiteSpace(firstName)
            ? DomainValidationResult.Failure("FirstName", "FirstName darf nicht leer sein.")
            : firstName.Trim().Length < NameMinLength
                ? DomainValidationResult.Failure("FirstName", $"FirstName muss mindestens {NameMinLength} Zeichen haben.")
                : DomainValidationResult.Success("FirstName");

    public static DomainValidationResult CheckLastName(string lastName) =>
        string.IsNullOrWhiteSpace(lastName)
            ? DomainValidationResult.Failure("LastName", "LastName darf nicht leer sein.")
            : lastName.Trim().Length < NameMinLength
                ? DomainValidationResult.Failure("LastName", $"LastName muss mindestens {NameMinLength} Zeichen haben.")
                : DomainValidationResult.Success("LastName");

    public static DomainValidationResult CheckDateOfBirth(DateTime dateOfBirth)
    {
        if (dateOfBirth > DateTime.Now)
            return DomainValidationResult.Failure("DateOfBirth", "DateOfBirth darf nicht in der Zukunft liegen.");
        
        if (dateOfBirth < new DateTime(1800, 1, 1))
            return DomainValidationResult.Failure("DateOfBirth", "DateOfBirth muss nach dem Jahr 1800 liegen.");
        
        return DomainValidationResult.Success("DateOfBirth");
    }

    /// <summary>
    /// Validiert die Author-Eigenschaften.
    /// </summary>
    public static void ValidateAuthorInternal(string firstName, string lastName, DateTime dateOfBirth)
    {
        var validationResults = new List<DomainValidationResult>
        {
            CheckFirstName(firstName),
            CheckLastName(lastName),
            CheckDateOfBirth(dateOfBirth)
        };
        
        foreach (var result in validationResults)
        {
            if (!result.IsValid)
            {
                throw new DomainValidationException(result.Property, result.ErrorMessage!);
            }
        }
    }
}

