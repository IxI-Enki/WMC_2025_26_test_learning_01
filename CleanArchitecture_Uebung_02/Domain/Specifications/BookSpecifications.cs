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

    public static DomainValidationResult CheckISBN( string isbn )
    {
        //throw new NotImplementedException( "CheckISBN muss noch implementiert werden!" );
        return
            (string.IsNullOrWhiteSpace( isbn ))
            ? DomainValidationResult.Failure( "isbn", "ISBN darf nicht leer sein." )
            : (isbn.Length != ISBNLength)
            ? DomainValidationResult.Failure( "isbn", $"ISBN muss genau {ISBNLength} Zeichen haben (ohne Bindestriche)." )  // ✅ RICHTIG
            : (!isbn.All( char.IsDigit ))
            ? DomainValidationResult.Failure( "isbn", "ISBN darf nur Ziffern enthalten." )
            : DomainValidationResult.Success( "isbn" );
    }

    public static DomainValidationResult CheckTitle( string title )
    {
        // throw new NotImplementedException( "CheckTitle muss noch implementiert werden!" );
        return
            (string.IsNullOrWhiteSpace( title ))

            ? DomainValidationResult.Failure( "title", "Titel darf nicht leer sein." )

            : (title.Length < TitleMinLength)

            ? DomainValidationResult.Failure( "title", $"Title muss mindestens {TitleMinLength} Zeichen haben (ohne Bindestriche)." )

            : DomainValidationResult.Success( "title" );
    }

    public static DomainValidationResult CheckAuthorId( int authorId )
    {
        //throw new NotImplementedException("CheckAuthorId muss noch implementiert werden!");
        return authorId <= 0
            ? DomainValidationResult.Failure( "authorId", "AuthorId muss größer als 0 sein." )
            : DomainValidationResult.Success( "authorId" );
    }

    public static DomainValidationResult CheckPublicationYear( int publicationYear )
    {
        // throw new NotImplementedException( "CheckPublicationYear muss noch implementiert werden!" );
        return publicationYear < MinPublicationYear || publicationYear > DateTime.Now.Year + 1
            ? DomainValidationResult.Failure( "publicationYear", $"PublicationYear muss zwischen {MinPublicationYear} und {DateTime.Now.Year} liegen." )
            : DomainValidationResult.Success( "publicationYear" );

    }

    public static DomainValidationResult CheckAvailableCopies( int availableCopies )
    {
        // throw new NotImplementedException( "CheckAvailableCopies muss noch implementiert werden!" );
        return availableCopies < MinAvailableCopies
            ? DomainValidationResult.Failure( "availableCopies", $"AvailableCopies muss mindestens {MinAvailableCopies} sein." )
            : DomainValidationResult.Success( "availableCopies" );
    }

    /// <summary>
    /// TODO: Implementiere ValidateBookInternal.
    /// </summary>
    public static void ValidateBookInternal(
        string isbn,
        string title,
        int authorId,
        int publicationYear,
        int availableCopies )
    {
        throw new NotImplementedException( "ValidateBookInternal muss noch implementiert werden!" );
        return;
    }

    /// <summary>
    /// TODO: Implementiere ValidateBookExternal.
    /// </summary>
    public static async Task ValidateBookExternal( int id, string isbn,
        IBookUniquenessChecker uniquenessChecker, CancellationToken ct = default )
    {
        throw new NotImplementedException( "ValidateBookExternal muss noch implementiert werden!" );
        return;

    }
}
