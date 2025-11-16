using Domain.Contracts;
using Domain.Entities;
using Domain.Exceptions;
using Xunit;

namespace Domain.Tests;

/// <summary>
/// Unit Tests für die Book-Entität.
/// Diese Tests helfen dir zu verstehen, wie die Domain-Validierung funktioniert.
/// </summary>
public class BookTests
{
    private class FakeBookUniquenessChecker( bool unique ) : IBookUniquenessChecker
    {
        public Task<bool> IsUniqueAsync( int id, string isbn, CancellationToken ct = default )
            => Task.FromResult( unique );
    }

    private class FakeAuthorUniquenessChecker : IAuthorUniquenessChecker
    {
        public Task<bool> IsUniqueAsync( int id, string fullName, CancellationToken ct = default )
            => Task.FromResult( true );
    }

    [Fact]
    public async Task CreateAsync_Succeeds_WithValidData( )
    {
        // Dieser Test wird funktionieren, sobald du Author.CreateAsync und Book.CreateAsync implementiert hast
        var authorChecker = new FakeAuthorUniquenessChecker();
        var author = await Author.CreateAsync("J.K.", "Rowling", new DateTime(1965, 7, 31), authorChecker);
        var bookChecker = new FakeBookUniquenessChecker(true);

        var book = await Book.CreateAsync("9780747532699", "Harry Potter", author, 1997, 5, bookChecker);

        Assert.Equal( "9780747532699", book.ISBN );
        Assert.Equal( "Harry Potter", book.Title );
        Assert.Equal( 1997, book.PublicationYear );
        Assert.Equal( 5, book.AvailableCopies );
    }

    [Theory]
    [InlineData( "", "Title", "ISBN darf nicht leer sein." )]
    [InlineData( "123", "Title", "ISBN muss genau 13 Zeichen haben (ohne Bindestriche)." )]
    [InlineData( "97807475326XX", "Title", "ISBN darf nur Ziffern enthalten." )]
    public async Task CreateAsync_InvalidISBN_Throws( string isbn, string title, string expectedMessage )
    {
        // Dieser Test wird funktionieren, sobald du die Domain-Validierungen implementiert hast
        var authorChecker = new FakeAuthorUniquenessChecker();
        var author = await Author.CreateAsync("Test", "Author", DateTime.Now.AddYears(-30), authorChecker);
        var bookChecker = new FakeBookUniquenessChecker(true);

        var ex = await Assert.ThrowsAsync<DomainValidationException>(
            () => Book.CreateAsync(isbn, title, author, 2020, 1, bookChecker));

        Assert.Contains( expectedMessage, ex.Message );
    }

    [Fact]
    public async Task CreateAsync_DuplicateISBN_Throws( )
    {
        // Dieser Test wird funktionieren, sobald du die Domain-Validierungen implementiert hast
        var authorChecker = new FakeAuthorUniquenessChecker();
        var author = await Author.CreateAsync("Test", "Author", DateTime.Now.AddYears(-30), authorChecker);
        var bookChecker = new FakeBookUniquenessChecker(false);

        var ex = await Assert.ThrowsAsync<DomainValidationException>(
            () => Book.CreateAsync("9780747532699", "Title", author, 2020, 1, bookChecker));

        Assert.Equal( "Ein Buch mit dieser ISBN existiert bereits.", ex.Message );
    }
}
