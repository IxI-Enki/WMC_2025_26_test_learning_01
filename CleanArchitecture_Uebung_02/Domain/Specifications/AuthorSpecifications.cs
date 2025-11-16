using Domain.Common;
using Domain.Exceptions;

namespace Domain.Specifications;

public static class AuthorSpecifications
{
    public const int NameMinLength = 2;

    public static DomainValidationResult CheckFirstName( string firstName )
    {
        //throw new NotImplementedException("CheckFirstName muss noch implementiert werden!");
        return
            firstName.Length >= NameMinLength
                ? DomainValidationResult.Success( nameof( firstName ) )
                : DomainValidationResult.Failure(
                    nameof( firstName ),
                    $"Der Vorname muss mindestens {NameMinLength} Zeichen lang sein."
                );
    }

    public static DomainValidationResult CheckLastName( string lastName )
    {
        //throw new NotImplementedException("CheckLastName muss noch implementiert werden!");
        return
            lastName.Length >= NameMinLength
                ? DomainValidationResult.Success( nameof( lastName ) )
                : DomainValidationResult.Failure(
                    nameof( lastName ),
                    $"Der Nachname muss mindestens {NameMinLength} Zeichen lang sein."
                );
    }

    public static DomainValidationResult CheckDateOfBirth( DateTime dateOfBirth )
    {
        //throw new NotImplementedException( "CheckDateOfBirth muss noch implementiert werden!" );
        return
            dateOfBirth < DateTime.Now
                ? DomainValidationResult.Success( nameof( dateOfBirth ) )
                : DomainValidationResult.Failure(
                    nameof( dateOfBirth ),
                    "Das Geburtsdatum muss in der Vergangenheit liegen."
                );
    }

    public static void ValidateAuthorInternal( string firstName, string lastName, DateTime dateOfBirth )
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
