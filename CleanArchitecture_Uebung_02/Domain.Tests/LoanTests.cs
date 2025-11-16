using Domain.Contracts;
using Domain.Entities;
using Domain.Exceptions;
using Xunit;

namespace Domain.Tests;

/// <summary>
/// Unit Tests für die Loan-Entität.
/// </summary>
public class LoanTests
{
    private class FakeAuthorUniquenessChecker : IAuthorUniquenessChecker
    {
        public Task<bool> IsUniqueAsync(int id, string fullName, CancellationToken ct = default)
            => Task.FromResult(true);
    }

    private class FakeBookUniquenessChecker : IBookUniquenessChecker
    {
        public Task<bool> IsUniqueAsync(int id, string isbn, CancellationToken ct = default)
            => Task.FromResult(true);
    }

    [Fact]
    public async Task Create_Succeeds_WithValidData()
    {
        // Arrange
        var authorChecker = new FakeAuthorUniquenessChecker();
        var author = await Author.CreateAsync("Test", "Author", DateTime.Now.AddYears(-30), authorChecker);
        var bookChecker = new FakeBookUniquenessChecker();
        var book = await Book.CreateAsync("9780747532699", "Test Book", author, 2020, 5, bookChecker);
        var loanDate = DateTime.Now;

        // Act
        var loan = Loan.Create(book, "John Doe", loanDate);

        // Assert
        Assert.NotNull(loan);
        Assert.Equal(book.Id, loan.BookId);
        Assert.Equal("John Doe", loan.BorrowerName);
        Assert.Equal(loanDate, loan.LoanDate);
        Assert.Equal(loanDate.AddDays(14), loan.DueDate);
        Assert.Null(loan.ReturnDate);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("A")]
    public async Task Create_InvalidBorrowerName_Throws(string borrowerName)
    {
        // Arrange
        var authorChecker = new FakeAuthorUniquenessChecker();
        var author = await Author.CreateAsync("Test", "Author", DateTime.Now.AddYears(-30), authorChecker);
        var bookChecker = new FakeBookUniquenessChecker();
        var book = await Book.CreateAsync("9780747532699", "Test Book", author, 2020, 5, bookChecker);

        // Act & Assert
        var ex = Assert.Throws<DomainValidationException>(() => 
            Loan.Create(book, borrowerName, DateTime.Now));
        
        Assert.Contains("BorrowerName", ex.Message);
    }

    [Fact]
    public async Task Create_FutureLoanDate_Throws()
    {
        // Arrange
        var authorChecker = new FakeAuthorUniquenessChecker();
        var author = await Author.CreateAsync("Test", "Author", DateTime.Now.AddYears(-30), authorChecker);
        var bookChecker = new FakeBookUniquenessChecker();
        var book = await Book.CreateAsync("9780747532699", "Test Book", author, 2020, 5, bookChecker);
        var futureDate = DateTime.Now.AddDays(1);

        // Act & Assert
        var ex = Assert.Throws<DomainValidationException>(() => 
            Loan.Create(book, "John Doe", futureDate));
        
        Assert.Contains("LoanDate", ex.Message);
    }

    [Fact]
    public async Task MarkAsReturned_SetsReturnDate()
    {
        // Arrange
        var authorChecker = new FakeAuthorUniquenessChecker();
        var author = await Author.CreateAsync("Test", "Author", DateTime.Now.AddYears(-30), authorChecker);
        var bookChecker = new FakeBookUniquenessChecker();
        var book = await Book.CreateAsync("9780747532699", "Test Book", author, 2020, 5, bookChecker);
        var loan = Loan.Create(book, "John Doe", DateTime.Now.AddDays(-5));
        var returnDate = DateTime.Now;

        // Act
        loan.MarkAsReturned(returnDate);

        // Assert
        Assert.Equal(returnDate, loan.ReturnDate);
    }

    [Fact]
    public async Task IsOverdue_ReturnsFalse_WhenNotOverdue()
    {
        // Arrange
        var authorChecker = new FakeAuthorUniquenessChecker();
        var author = await Author.CreateAsync("Test", "Author", DateTime.Now.AddYears(-30), authorChecker);
        var bookChecker = new FakeBookUniquenessChecker();
        var book = await Book.CreateAsync("9780747532699", "Test Book", author, 2020, 5, bookChecker);
        var loan = Loan.Create(book, "John Doe", DateTime.Now);

        // Act
        var isOverdue = loan.IsOverdue();

        // Assert
        Assert.False(isOverdue);
    }

    [Fact]
    public async Task IsOverdue_ReturnsTrue_WhenOverdue()
    {
        // Arrange
        var authorChecker = new FakeAuthorUniquenessChecker();
        var author = await Author.CreateAsync("Test", "Author", DateTime.Now.AddYears(-30), authorChecker);
        var bookChecker = new FakeBookUniquenessChecker();
        var book = await Book.CreateAsync("9780747532699", "Test Book", author, 2020, 5, bookChecker);
        var loan = Loan.Create(book, "John Doe", DateTime.Now.AddDays(-20));

        // Act
        var isOverdue = loan.IsOverdue();

        // Assert
        Assert.True(isOverdue);
    }

    [Fact]
    public async Task IsOverdue_ReturnsFalse_WhenReturned()
    {
        // Arrange
        var authorChecker = new FakeAuthorUniquenessChecker();
        var author = await Author.CreateAsync("Test", "Author", DateTime.Now.AddYears(-30), authorChecker);
        var bookChecker = new FakeBookUniquenessChecker();
        var book = await Book.CreateAsync("9780747532699", "Test Book", author, 2020, 5, bookChecker);
        var loan = Loan.Create(book, "John Doe", DateTime.Now.AddDays(-20));
        loan.MarkAsReturned(DateTime.Now);

        // Act
        var isOverdue = loan.IsOverdue();

        // Assert
        Assert.False(isOverdue);
    }
}

