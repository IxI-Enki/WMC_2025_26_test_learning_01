using Domain.Common;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Specifications;

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
    
    private Book() { } // Für EF Core

    /// <summary>
    /// Erstellt asynchron ein neues Buch mit den angegebenen Eigenschaften.
    /// </summary>
    public static async Task<Book> CreateAsync(string isbn, string title, Author author, 
        int publicationYear, int availableCopies, IBookUniquenessChecker uniquenessChecker, 
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(author);
        
        var trimmedIsbn = (isbn ?? string.Empty).Trim();
        var trimmedTitle = (title ?? string.Empty).Trim();
        
        BookSpecifications.ValidateBookInternal(trimmedIsbn, trimmedTitle, author.Id, publicationYear, availableCopies);
        await BookSpecifications.ValidateBookExternal(0, trimmedIsbn, uniquenessChecker, ct);
        
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

    /// <summary>
    /// Aktualisiert die Eigenschaften des Buches.
    /// </summary>
    public async Task UpdateAsync(string isbn, string title, int authorId, int publicationYear, 
        int availableCopies, IBookUniquenessChecker uniquenessChecker, CancellationToken ct = default)
    {
        var trimmedIsbn = (isbn ?? string.Empty).Trim();
        var trimmedTitle = (title ?? string.Empty).Trim();
        
        if (ISBN == trimmedIsbn && Title == trimmedTitle && AuthorId == authorId && 
            PublicationYear == publicationYear && AvailableCopies == availableCopies)
            return; // Keine Änderung
        
        BookSpecifications.ValidateBookInternal(trimmedIsbn, trimmedTitle, authorId, publicationYear, availableCopies);
        await BookSpecifications.ValidateBookExternal(Id, trimmedIsbn, uniquenessChecker, ct);
        
        ISBN = trimmedIsbn;
        Title = trimmedTitle;
        AuthorId = authorId;
        PublicationYear = publicationYear;
        AvailableCopies = availableCopies;
    }

    /// <summary>
    /// Reduziert die Anzahl verfügbarer Exemplare (beim Ausleihen).
    /// </summary>
    public void DecreaseCopies()
    {
        if (AvailableCopies <= 0)
            throw new DomainValidationException("AvailableCopies", "Keine Exemplare mehr verfügbar.");
        AvailableCopies--;
    }

    /// <summary>
    /// Erhöht die Anzahl verfügbarer Exemplare (beim Zurückgeben).
    /// </summary>
    public void IncreaseCopies()
    {
        AvailableCopies++;
    }

    public override string ToString() => $"{Title} ({ISBN})";
}
