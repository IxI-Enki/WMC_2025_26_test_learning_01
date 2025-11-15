using Domain.Common;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Specifications;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

/// <summary>
/// Repräsentiert ein Buch in der Bibliothek.
/// </summary>
public class Book : BaseEntity
{
    /// <summary>
    /// ISBN-Nummer des Buches (International Standard Book Number).
    /// </summary>
    public string ISBN { get; private set; } = string.Empty;

    /// <summary>
    /// Titel des Buches.
    /// </summary>
    public string Title { get; private set; } = string.Empty;

    /// <summary>
    /// Fremdschlüssel auf den Autor.
    /// </summary>
    public int AuthorId { get; private set; }

    /// <summary>
    /// Navigation zum Autor.
    /// </summary>
    public Author Author { get; private set; } = null!;

    /// <summary>
    /// Veröffentlichungsjahr.
    /// </summary>
    public int PublicationYear { get; private set; }

    /// <summary>
    /// Anzahl verfügbarer Exemplare.
    /// </summary>
    public int AvailableCopies { get; private set; }

    /// <summary>
    /// Sammlung aller Ausleihen für dieses Buch.
    /// </summary>
    public ICollection<Loan> Loans { get; private set; } = default!;

    private Book( ) { } // Für EF Core

    /// <summary>
    /// CreateAsync für Book.
    /// </summary>
    public static async Task<Book> CreateAsync(
        string isbn,
        string title,
        Author author,
        int publicationYear,
        int availableCopies,
        IBookUniquenessChecker uniquenessChecker,
        int id = 0,
        CancellationToken ct = default
        )
    {
        //throw new NotImplementedException( "Book.CreateAsync muss noch implementiert werden!" );
        var trimmedIsbn = (isbn ?? string.Empty).Trim();
        var trimmedTitle = (title ?? string.Empty).Trim();

        ValidateBookProperties( trimmedIsbn, trimmedTitle, author, publicationYear, availableCopies );



        await VaidateBookUniqueness( id, trimmedIsbn, uniquenessChecker, ct );

        return new Book
        {
            ISBN = trimmedIsbn,
            Title = trimmedTitle,
            Author = author,
            AuthorId = author.Id,
            PublicationYear = publicationYear,
            AvailableCopies = availableCopies
        };
    }

    private static void ValidateBookProperties( string trimmedIsbn, string trimmedTitle, Author author, int publicationYear, int availableCopies )
    {
        //throw new NotImplementedException( );
        var validationResults = new List<DomainValidationResult>{
            //BookSpecifications.CheckAuthorId(author.Id),
            BookSpecifications.CheckPublicationYear(publicationYear),
            BookSpecifications.CheckAvailableCopies(availableCopies),
            BookSpecifications.CheckTitle(trimmedTitle),
            BookSpecifications.CheckISBN(trimmedIsbn),
        };
        foreach(var result in validationResults)
        {
            if(!result.IsValid)
            {
                throw new DomainValidationException( result.Property, result.ErrorMessage! );
            }
        }
    }


    /// <summary>
    /// TODO: Implementiere UpdateAsync für Book.
    /// </summary>
    public async Task UpdateAsync(
        string isbn,
        string title,
        int authorId,
        int publicationYear,
        int availableCopies,
        IBookUniquenessChecker uniquenessChecker,
        CancellationToken ct = default
        )
    {

        throw new NotImplementedException( "Book.UpdateAsync muss noch implementiert werden!" );

    }

    /// <summary>
    /// Reduziert die Anzahl verfügbarer Exemplare (beim Ausleihen).
    /// </summary>
    public void DecreaseCopies( )
    {
        if(AvailableCopies <= 0)

            throw new DomainValidationException( "AvailableCopies", "Keine Exemplare mehr verfügbar." );

        AvailableCopies--;
    }

    /// <summary>
    /// Erhöht die Anzahl verfügbarer Exemplare (beim Zurückgeben).
    /// </summary>
    public void IncreaseCopies( )
    {
        AvailableCopies++;
    }

    public override string ToString( ) => $"{Title} ({ISBN})";
    public static async Task VaidateBookUniqueness( int id,
        string trimmedIsbn,
        IBookUniquenessChecker uniquenessChecker,
        CancellationToken ct = default )
    {
        //throw new NotImplementedException( );
        if(!await uniquenessChecker.IsUniqueAsync( id, trimmedIsbn, ct ))
            throw new DomainValidationException( "ISBN", "Ein Buch mit dieser ISBN existiert bereits." );
    }
}
