using Domain.Specifications;
using Xunit;

namespace Domain.Tests;

/// <summary>
/// Unit Tests für LoanSpecifications.
/// Diese Tests helfen dir bei der Implementierung der Domain-Validierungen für Loans.
/// </summary>
public class LoanSpecificationsTests
{
    [Fact]
    public void CheckBookId_ValidId_ReturnsSuccess()
    {
        // TODO: Dieser Test wird erst funktionieren, wenn du CheckBookId implementiert hast
        var result = LoanSpecifications.CheckBookId(1);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CheckBookId_InvalidId_ReturnsFailure(int bookId)
    {
        // TODO: Implementiere CheckBookId in LoanSpecifications
        var result = LoanSpecifications.CheckBookId(bookId);
        Assert.False(result.IsValid);
        Assert.Equal("BookId muss größer als 0 sein.", result.ErrorMessage);
    }

    [Fact]
    public void CheckBorrowerName_ValidName_ReturnsSuccess()
    {
        var result = LoanSpecifications.CheckBorrowerName("John Doe");
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("")]
    [InlineData("A")]
    public void CheckBorrowerName_InvalidName_ReturnsFailure(string name)
    {
        var result = LoanSpecifications.CheckBorrowerName(name);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void CheckLoanDate_ValidDate_ReturnsSuccess()
    {
        var result = LoanSpecifications.CheckLoanDate(DateTime.Now);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void CheckLoanDate_FutureDate_ReturnsFailure()
    {
        var futureDate = DateTime.Now.AddDays(1);
        var result = LoanSpecifications.CheckLoanDate(futureDate);
        Assert.False(result.IsValid);
        Assert.Equal("LoanDate darf nicht in der Zukunft liegen.", result.ErrorMessage);
    }
}

