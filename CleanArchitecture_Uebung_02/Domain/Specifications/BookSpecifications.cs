using Domain.Common;
using Domain.Contracts;
using Domain.Exceptions;

namespace Domain.Specifications;

public static class BookSpecifications
{
    public const int ISBNLength = 13;
    public const int TitleMinLength = 1;
    public const int MinPublicationYear = 1450;
    public const int MinAvailableCopies = 0;

    /// <summary>
    /// TODO: Implementiere die CheckISBN Validierung.
    /// 
    /// Regeln:
    /// - Darf nicht leer sein
    /// - Muss genau ISBNLength (13) Zeichen haben (ohne Bindestriche/Leerzeichen)
    /// - Darf nur Ziffern enthalten
    /// </summary>
    public static DomainValidationResult CheckISBN(string isbn)
    {
        throw new NotImplementedException("CheckISBN muss noch implementiert werden!");
    }

    /// <summary>
    /// TODO: Implementiere die CheckTitle Validierung.
    /// </summary>
    public static DomainValidationResult CheckTitle(string title)
    {
        throw new NotImplementedException("CheckTitle muss noch implementiert werden!");
    }

    /// <summary>
    /// TODO: Implementiere die CheckAuthorId Validierung.
    /// </summary>
    public static DomainValidationResult CheckAuthorId(int authorId)
    {
        throw new NotImplementedException("CheckAuthorId muss noch implementiert werden!");
    }

    /// <summary>
    /// TODO: Implementiere die CheckPublicationYear Validierung.
    /// </summary>
    public static DomainValidationResult CheckPublicationYear(int publicationYear)
    {
        throw new NotImplementedException("CheckPublicationYear muss noch implementiert werden!");
    }

    /// <summary>
    /// TODO: Implementiere die CheckAvailableCopies Validierung.
    /// </summary>
    public static DomainValidationResult CheckAvailableCopies(int availableCopies)
    {
        throw new NotImplementedException("CheckAvailableCopies muss noch implementiert werden!");
    }

    /// <summary>
    /// TODO: Implementiere ValidateBookInternal.
    /// </summary>
    public static void ValidateBookInternal(string isbn, string title, int authorId, 
        int publicationYear, int availableCopies)
    {
        throw new NotImplementedException("ValidateBookInternal muss noch implementiert werden!");
    }

    /// <summary>
    /// TODO: Implementiere ValidateBookExternal.
    /// </summary>
    public static async Task ValidateBookExternal(int id, string isbn, 
        IBookUniquenessChecker uniquenessChecker, CancellationToken ct = default)
    {
        throw new NotImplementedException("ValidateBookExternal muss noch implementiert werden!");
    }
}
