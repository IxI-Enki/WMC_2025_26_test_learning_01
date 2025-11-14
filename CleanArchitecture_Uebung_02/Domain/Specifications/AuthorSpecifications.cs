using Domain.Common;
using Domain.Exceptions;

namespace Domain.Specifications;

public static class AuthorSpecifications
{
    public const int NameMinLength = 2;

    /// <summary>
    /// TODO: Implementiere CheckFirstName, CheckLastName, CheckDateOfBirth und ValidateAuthorInternal.
    /// </summary>
    public static DomainValidationResult CheckFirstName(string firstName)
    {
        throw new NotImplementedException("CheckFirstName muss noch implementiert werden!");
    }

    public static DomainValidationResult CheckLastName(string lastName)
    {
        throw new NotImplementedException("CheckLastName muss noch implementiert werden!");
    }

    public static DomainValidationResult CheckDateOfBirth(DateTime dateOfBirth)
    {
        throw new NotImplementedException("CheckDateOfBirth muss noch implementiert werden!");
    }

    public static void ValidateAuthorInternal(string firstName, string lastName, DateTime dateOfBirth)
    {
        throw new NotImplementedException("ValidateAuthorInternal muss noch implementiert werden!");
    }
}

