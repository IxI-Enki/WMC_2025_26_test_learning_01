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

    public static DomainValidationResult CheckISBN(string isbn)
    {
        if (string.IsNullOrWhiteSpace(isbn))
            return DomainValidationResult.Failure("ISBN", "ISBN darf nicht leer sein.");
        
        var cleaned = isbn.Replace("-", "").Replace(" ", "");
        if (cleaned.Length != ISBNLength)
            return DomainValidationResult.Failure("ISBN", $"ISBN muss genau {ISBNLength} Zeichen haben (ohne Bindestriche).");
        
        if (!cleaned.All(char.IsDigit))
            return DomainValidationResult.Failure("ISBN", "ISBN darf nur Ziffern enthalten.");
        
        return DomainValidationResult.Success("ISBN");
    }

    public static DomainValidationResult CheckTitle(string title) =>
        string.IsNullOrWhiteSpace(title)
            ? DomainValidationResult.Failure("Title", "Title darf nicht leer sein.")
            : title.Trim().Length < TitleMinLength
                ? DomainValidationResult.Failure("Title", $"Title muss mindestens {TitleMinLength} Zeichen haben.")
                : DomainValidationResult.Success("Title");

    public static DomainValidationResult CheckAuthorId(int authorId) =>
        authorId <= 0
            ? DomainValidationResult.Failure("AuthorId", "AuthorId muss größer als 0 sein.")
            : DomainValidationResult.Success("AuthorId");

    public static DomainValidationResult CheckPublicationYear(int publicationYear)
    {
        if (publicationYear < MinPublicationYear)
            return DomainValidationResult.Failure("PublicationYear", 
                $"PublicationYear muss mindestens {MinPublicationYear} sein.");
        
        if (publicationYear > DateTime.Now.Year + 1)
            return DomainValidationResult.Failure("PublicationYear", 
                "PublicationYear darf nicht mehr als 1 Jahr in der Zukunft liegen.");
        
        return DomainValidationResult.Success("PublicationYear");
    }

    public static DomainValidationResult CheckAvailableCopies(int availableCopies) =>
        availableCopies < MinAvailableCopies
            ? DomainValidationResult.Failure("AvailableCopies", 
                $"AvailableCopies muss mindestens {MinAvailableCopies} sein.")
            : DomainValidationResult.Success("AvailableCopies");

    /// <summary>
    /// Validiert die internen Book-Eigenschaften.
    /// </summary>
    public static void ValidateBookInternal(string isbn, string title, int authorId, 
        int publicationYear, int availableCopies)
    {
        var validationResults = new List<DomainValidationResult>
        {
            CheckISBN(isbn),
            CheckTitle(title),
            CheckAuthorId(authorId),
            CheckPublicationYear(publicationYear),
            CheckAvailableCopies(availableCopies)
        };
        
        foreach (var result in validationResults)
        {
            if (!result.IsValid)
            {
                throw new DomainValidationException(result.Property, result.ErrorMessage!);
            }
        }
    }

    /// <summary>
    /// Validiert die externe Eindeutigkeit (ISBN muss eindeutig sein).
    /// </summary>
    public static async Task ValidateBookExternal(int id, string isbn, 
        IBookUniquenessChecker uniquenessChecker, CancellationToken ct = default)
    {
        if (!await uniquenessChecker.IsUniqueAsync(id, isbn, ct))
            throw new DomainValidationException("ISBN", "Ein Buch mit dieser ISBN existiert bereits.");
    }
}

