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
    
    private Author() { } // Für EF Core

    /// <summary>
    /// Erstellt einen neuen Autor.
    /// </summary>
    public static Author Create(string firstName, string lastName, DateTime dateOfBirth)
    {
        var trimmedFirstName = (firstName ?? string.Empty).Trim();
        var trimmedLastName = (lastName ?? string.Empty).Trim();
        
        AuthorSpecifications.ValidateAuthorInternal(trimmedFirstName, trimmedLastName, dateOfBirth);
        
        return new Author
        {
            FirstName = trimmedFirstName,
            LastName = trimmedLastName,
            DateOfBirth = dateOfBirth
        };
    }

    public string FullName => $"{FirstName} {LastName}";
    
    public override string ToString() => FullName;
}

