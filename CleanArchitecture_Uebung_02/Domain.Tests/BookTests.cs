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
    private class FakeUniquenessChecker(bool unique) : IBookUniquenessChecker
    {
        public Task<bool> IsUniqueAsync(int id, string isbn, CancellationToken ct = default) 
            => Task.FromResult(unique);
    }

    [Fact]
    public async Task CreateAsync_Succeeds_WithValidData()
    {
        // Dieser Test wird funktionieren, sobald du Author.Create und Book.CreateAsync implementiert hast
        var author = Author.Create("J.K.", "Rowling", new DateTime(1965, 7, 31));
        var checker = new FakeUniquenessChecker(true);
        
        var book = await Book.CreateAsync("9780747532699", "Harry Potter", author, 1997, 5, checker);
        
        Assert.Equal("9780747532699", book.ISBN);
        Assert.Equal("Harry Potter", book.Title);
        Assert.Equal(1997, book.PublicationYear);
        Assert.Equal(5, book.AvailableCopies);
    }

    [Theory]
    [InlineData("", "Title", "ISBN darf nicht leer sein.")]
    [InlineData("123", "Title", "ISBN muss genau 13 Zeichen haben (ohne Bindestriche).")]
    [InlineData("97807475326XX", "Title", "ISBN darf nur Ziffern enthalten.")]
    public async Task CreateAsync_InvalidISBN_Throws(string isbn, string title, string expectedMessage)
    {
        // Dieser Test wird funktionieren, sobald du die Domain-Validierungen implementiert hast
        var author = Author.Create("Test", "Author", DateTime.Now.AddYears(-30));
        var checker = new FakeUniquenessChecker(true);
        
        var ex = await Assert.ThrowsAsync<DomainValidationException>(
            () => Book.CreateAsync(isbn, title, author, 2020, 1, checker));
        
        Assert.Contains(expectedMessage, ex.Message);
    }

    [Fact]
    public async Task CreateAsync_DuplicateISBN_Throws()
    {
        // Dieser Test wird funktionieren, sobald du die Domain-Validierungen implementiert hast
        var author = Author.Create("Test", "Author", DateTime.Now.AddYears(-30));
        var checker = new FakeUniquenessChecker(false);
        
        var ex = await Assert.ThrowsAsync<DomainValidationException>(
            () => Book.CreateAsync("9780747532699", "Title", author, 2020, 1, checker));
        
        Assert.Equal("Ein Buch mit dieser ISBN existiert bereits.", ex.Message);
    }
}
