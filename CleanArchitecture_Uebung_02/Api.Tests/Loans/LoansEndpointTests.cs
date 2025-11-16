using Api.Tests.Utilities;
using Application.Dtos;
using Application.Features.Authors.Commands.CreateAuthor;
using Application.Features.Books.Commands.CreateBook;
using Application.Features.Loans.Commands.CreateLoan;
using Application.Features.Loans.Commands.ReturnLoan;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Api.Tests.Loans;

/// <summary>
/// Integration Tests für Loan Endpoints.
/// </summary>
public class LoansEndpointTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public LoansEndpointTests(TestWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    private async Task<GetAuthorDto> CreateTestAuthor()
    {
        var authorCommand = new CreateAuthorCommand("Test", "Author", new DateTime(1970, 1, 1));
        var response = await _client.PostAsJsonAsync("/api/autors", authorCommand);
        return (await response.Content.ReadFromJsonAsync<GetAuthorDto>())!;
    }

    private async Task<GetBookDto> CreateTestBook(int authorId, string isbn = "9780747532699")
    {
        var bookCommand = new CreateBookCommand(isbn, "Test Book", authorId, 2020, 5);
        var response = await _client.PostAsJsonAsync("/api/books", bookCommand);
        return (await response.Content.ReadFromJsonAsync<GetBookDto>())!;
    }

    [Fact]
    public async Task CreateLoan_ReturnsCreated_WithValidData()
    {
        // Arrange
        var author = await CreateTestAuthor();
        var book = await CreateTestBook(author.Id, "9780451524935");
        var command = new CreateLoanCommand(book.Id, "John Doe", DateTime.Now);

        // Act
        var response = await _client.PostAsJsonAsync("/api/loans", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var loan = await response.Content.ReadFromJsonAsync<GetLoanDto>();
        loan.Should().NotBeNull();
        loan!.BookId.Should().Be(book.Id);
        loan.BorrowerName.Should().Be("John Doe");
        loan.ReturnDate.Should().BeNull();
        loan.DueDate.Should().BeCloseTo(DateTime.Now.AddDays(14), TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task CreateLoan_ReturnsBadRequest_WithInvalidBorrowerName()
    {
        // Arrange
        var author = await CreateTestAuthor();
        var book = await CreateTestBook(author.Id, "9780061120084");
        var command = new CreateLoanCommand(book.Id, "", DateTime.Now); // Empty borrower name

        // Act
        var response = await _client.PostAsJsonAsync("/api/loans", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateLoan_ReturnsNotFound_WithNonExistentBook()
    {
        // Arrange
        var command = new CreateLoanCommand(99999, "John Doe", DateTime.Now);

        // Act
        var response = await _client.PostAsJsonAsync("/api/loans", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateLoan_ReturnsBadRequest_WithFutureLoanDate()
    {
        // Arrange
        var author = await CreateTestAuthor();
        var book = await CreateTestBook(author.Id, "9780743273565");
        var command = new CreateLoanCommand(book.Id, "John Doe", DateTime.Now.AddDays(1));

        // Act
        var response = await _client.PostAsJsonAsync("/api/loans", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ReturnLoan_ReturnsNoContent_WithValidData()
    {
        // Arrange - Create a loan first
        var author = await CreateTestAuthor();
        var book = await CreateTestBook(author.Id, "9780544003415");
        var createCommand = new CreateLoanCommand(book.Id, "Jane Doe", DateTime.Now.AddDays(-5));
        var createResponse = await _client.PostAsJsonAsync("/api/loans", createCommand);
        var createdLoan = await createResponse.Content.ReadFromJsonAsync<GetLoanDto>();

        var returnDate = DateTime.Now;

        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/loans/{createdLoan!.Id}/return",
            returnDate
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task ReturnLoan_ReturnsNotFound_WhenLoanDoesNotExist()
    {
        // Act
        var response = await _client.PutAsJsonAsync("/api/loans/99999/return", DateTime.Now);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetLoansByBook_ReturnsOk_WithLoans()
    {
        // Arrange - Create a book and some loans
        var author = await CreateTestAuthor();
        var book = await CreateTestBook(author.Id, "9781234567890");

        var loan1 = new CreateLoanCommand(book.Id, "Alice", DateTime.Now.AddDays(-10));
        var loan2 = new CreateLoanCommand(book.Id, "Bob", DateTime.Now.AddDays(-5));

        await _client.PostAsJsonAsync("/api/loans", loan1);
        await _client.PostAsJsonAsync("/api/loans", loan2);

        // Act
        var response = await _client.GetAsync($"/api/loans/book/{book.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var loans = await response.Content.ReadFromJsonAsync<List<GetLoanDto>>();
        loans.Should().NotBeNull();
        loans!.Should().HaveCountGreaterOrEqualTo(2);
        loans.All(l => l.BookId == book.Id).Should().BeTrue();
    }

    [Fact]
    public async Task GetLoansByBook_ReturnsEmptyList_WhenNoLoansExist()
    {
        // Arrange
        var author = await CreateTestAuthor();
        var book = await CreateTestBook(author.Id, "9789876543210");

        // Act
        var response = await _client.GetAsync($"/api/loans/book/{book.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var loans = await response.Content.ReadFromJsonAsync<List<GetLoanDto>>();
        loans.Should().NotBeNull();
        loans.Should().BeEmpty();
    }

    [Fact]
    public async Task GetOverdueLoans_ReturnsOk_WithOverdueLoans()
    {
        // Arrange - Create an overdue loan (30 days ago, due date is 16 days ago)
        var author = await CreateTestAuthor();
        var book = await CreateTestBook(author.Id, "9781111111111");
        var overdueCommand = new CreateLoanCommand(book.Id, "Charlie", DateTime.Now.AddDays(-30));

        await _client.PostAsJsonAsync("/api/loans", overdueCommand);

        // Act
        var response = await _client.GetAsync("/api/loans/overdue");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var loans = await response.Content.ReadFromJsonAsync<List<GetLoanDto>>();
        loans.Should().NotBeNull();
        loans!.Should().HaveCountGreaterOrEqualTo(1);
        
        var overdueLoan = loans.FirstOrDefault(l => l.BookId == book.Id);
        overdueLoan.Should().NotBeNull();
        overdueLoan!.IsOverdue.Should().BeTrue();
        overdueLoan.ReturnDate.Should().BeNull();
    }

    [Fact]
    public async Task GetOverdueLoans_ExcludesReturnedLoans()
    {
        // Arrange - Create an overdue loan and then return it
        var author = await CreateTestAuthor();
        var book = await CreateTestBook(author.Id, "9782222222222");
        var overdueCommand = new CreateLoanCommand(book.Id, "David", DateTime.Now.AddDays(-30));
        var createResponse = await _client.PostAsJsonAsync("/api/loans", overdueCommand);
        var createdLoan = await createResponse.Content.ReadFromJsonAsync<GetLoanDto>();

        // Return the loan
        await _client.PutAsJsonAsync($"/api/loans/{createdLoan!.Id}/return", DateTime.Now);

        // Act
        var response = await _client.GetAsync("/api/loans/overdue");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var loans = await response.Content.ReadFromJsonAsync<List<GetLoanDto>>();
        loans.Should().NotBeNull();
        
        // The returned loan should not be in the overdue list
        loans!.Should().NotContain(l => l.Id == createdLoan.Id);
    }

    [Fact]
    public async Task CreateLoan_DecreasesAvailableCopies()
    {
        // Arrange
        var author = await CreateTestAuthor();
        var book = await CreateTestBook(author.Id, "9783333333333");
        var initialCopies = book.AvailableCopies;

        var command = new CreateLoanCommand(book.Id, "Emma", DateTime.Now);

        // Act
        await _client.PostAsJsonAsync("/api/loans", command);

        // Assert - Check that available copies decreased
        var getResponse = await _client.GetAsync($"/api/books/{book.Id}");
        var updatedBook = await getResponse.Content.ReadFromJsonAsync<GetBookDto>();
        updatedBook!.AvailableCopies.Should().Be(initialCopies - 1);
    }

    [Fact]
    public async Task ReturnLoan_IncreasesAvailableCopies()
    {
        // Arrange - Create a loan
        var author = await CreateTestAuthor();
        var book = await CreateTestBook(author.Id, "9784444444444");
        var createCommand = new CreateLoanCommand(book.Id, "Frank", DateTime.Now.AddDays(-5));
        var createResponse = await _client.PostAsJsonAsync("/api/loans", createCommand);
        var createdLoan = await createResponse.Content.ReadFromJsonAsync<GetLoanDto>();

        // Get current copies
        var getBookResponse = await _client.GetAsync($"/api/books/{book.Id}");
        var currentBook = await getBookResponse.Content.ReadFromJsonAsync<GetBookDto>();
        var currentCopies = currentBook!.AvailableCopies;

        // Act - Return the loan
        await _client.PutAsJsonAsync($"/api/loans/{createdLoan!.Id}/return", DateTime.Now);

        // Assert - Check that available copies increased
        var getUpdatedResponse = await _client.GetAsync($"/api/books/{book.Id}");
        var updatedBook = await getUpdatedResponse.Content.ReadFromJsonAsync<GetBookDto>();
        updatedBook!.AvailableCopies.Should().Be(currentCopies + 1);
    }
}

