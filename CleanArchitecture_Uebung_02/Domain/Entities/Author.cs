using Domain.Common;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Specifications;
using System.Xml.Linq;

namespace Domain.Entities;


/// <summary>
/// Repräsentiert einen Autor.
/// </summary>
public class Author : BaseEntity
{
    /// <summary>
    /// Vorname des Autors.
    /// </summary>
    public string FirstName { get; private set; } = string.Empty;

    /// <summary>
    /// Nachname des Autors.
    /// </summary>
    public string LastName { get; private set; } = string.Empty;

    /// <summary>
    /// Geburtsdatum des Autors.
    /// </summary>
    public DateTime DateOfBirth { get; private set; }

    /// <summary>
    /// Sammlung aller Bücher dieses Autors.
    /// </summary>
    public ICollection<Book> Books { get; private set; } = default!;

    private Author( ) { } // Für EF Core

    public static async Task<Author> CreateAsync(
        string firstName,
        string lastName,
        DateTime dateOfBirth,
        IAuthorUniquenessChecker uc,
        CancellationToken ct = default )
    {
        ArgumentNullException.ThrowIfNull( firstName, nameof( firstName ) );
        ArgumentNullException.ThrowIfNull( lastName, nameof( lastName ) );

        ValidAuthorProperties( firstName, lastName, dateOfBirth );
        await ValidateAuthorUniqueness( 0, firstName, lastName, uc, ct );

        return new Author { FirstName = firstName, LastName = lastName, DateOfBirth = dateOfBirth };
    }


    public async Task UpdateAsync(
        string firstName,
        string lastName,
        DateTime dateOfBirth,
        IAuthorUniquenessChecker uc,
        CancellationToken ct = default )
    {
        var trimmedFirstName = (firstName  ?? string.Empty).Trim();
        var trimmedLastName = ( lastName  ?? string.Empty).Trim();

        if(FirstName == trimmedFirstName &&       //
           LastName == trimmedLastName &&          //
           DateOfBirth == dateOfBirth) return;    // Keine Änderung

        ValidAuthorProperties(
            trimmedFirstName,                         //
            trimmedLastName,                          //
            dateOfBirth );                             // Validieren der Eigenschaften

        await ValidateAuthorUniqueness( 
            Id,                                           //
            trimmedFirstName,                         //
            trimmedLastName,                          //
            uc,                                          //
            ct );                                        // Überprüfen der Einzigartigkeit

        FirstName = trimmedFirstName;              //
        LastName = trimmedLastName;                 //
        DateOfBirth = dateOfBirth;                  // Aktualisieren
    }

    private static void ValidAuthorProperties( string firstName, string lastName, DateTime dateOfBirth )
    {
        //throw new NotImplementedException( );
        var validationResults = new List<DomainValidationResult>
        {
            AuthorSpecifications.CheckFirstName(firstName),

            AuthorSpecifications.CheckLastName(lastName),

            AuthorSpecifications.CheckDateOfBirth(dateOfBirth),
        };
        foreach(var result in validationResults)
        {
            if(!result.IsValid)
            {
                throw new DomainValidationException( result.Property, result.ErrorMessage! );
            }
        }
    }

    public string FullName => $"{FirstName} {LastName}";

    public override string ToString( ) => FullName;

    public static async Task ValidateAuthorUniqueness(
        int id,
        string firstName,
        string lastName,
        IAuthorUniquenessChecker uc,
        CancellationToken ct = default )
    {
        string fullName = $"{firstName} {lastName}";

        if(!await uc.IsUniqueAsync( id, fullName, ct ))
            throw new DomainValidationException(
                  "Uniqueness",
                  "Ein Author mit der gleichem Namen existiert bereits." );
    }
}

