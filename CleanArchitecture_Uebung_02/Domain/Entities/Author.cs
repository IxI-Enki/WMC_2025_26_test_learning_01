using Domain.Common;
using Domain.Exceptions;
using Domain.Specifications;

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

    public static Author Create(
        string firstName,
        string lastName,
        DateTime dateOfBirth )
    {
        //throw new NotImplementedException( "Author.Create muss noch implementiert werden!" );
        ArgumentNullException.ThrowIfNull( firstName, nameof( firstName ) );

        ValidAuthorProperties( firstName, lastName, dateOfBirth );

        return new Author { FirstName = firstName, LastName = lastName, DateOfBirth = dateOfBirth };
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
}

