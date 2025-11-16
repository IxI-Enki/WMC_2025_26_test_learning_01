using Api.Tests.Utilities;
using Application.Dtos;
using Application.Features.Authors.Commands.CreateAuthor;
using Application.Features.Books.Commands.CreateBook;
using Application.Features.Books.Commands.UpdateBook;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Api.Tests.Books;

/// <summary>
/// Integration Tests für Book Endpoints.
/// </summary>
public class BooksEndpointTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public BooksEndpointTests(TestWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    private async Task<GetAuthorDto> CreateTestAuthor()
    {
        var authorCommand = new CreateAuthorCommand("Test", "Author", new DateTime(1970, 1, 1));
        var response = await _client.PostAsJsonAsync("/api/autors", authorCommand);
        return (await response.Content.ReadFromJsonAsync<GetAuthorDto>())!;
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithBooks()
    {
        // Act
        var response = await _client.GetAsync("/api/books");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var books = await response.Content.ReadFromJsonAsync<List<GetBookDto>>();
        books.Should().NotBeNull();
    }

    [Fact]
    public async Task Create_ReturnsCreated_WithValidData()
    {
        // Arrange
        var author = await CreateTestAuthor();
        var command = new CreateBookCommand("9780747532699", "Harry Potter", author.Id, 1997, 5);

        // Act
        var response = await _client.PostAsJsonAsync("/api/books", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var book = await response.Content.ReadFromJsonAsync<GetBookDto>();
        book.Should().NotBeNull();
        book!.ISBN.Should().Be("9780747532699");
        book.Title.Should().Be("Harry Potter");
        book.PublicationYear.Should().Be(1997);
        book.AvailableCopies.Should().Be(5);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WithInvalidISBN()
    {
        // Arrange
        var author = await CreateTestAuthor();
        var command = new CreateBookCommand("123", "Test Book", author.Id, 2020, 1); // Invalid ISBN

        // Act
        var response = await _client.PostAsJsonAsync("/api/books", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_ReturnsNotFound_WithNonExistentAuthor()
    {
        // Arrange
        var command = new CreateBookCommand("9780747532699", "Test Book", 99999, 2020, 1);

        // Act
        var response = await _client.PostAsJsonAsync("/api/books", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenBookExists()
    {
        // Arrange - Create a book first
        var author = await CreateTestAuthor();
        var createCommand = new CreateBookCommand("9780451524935", "1984", author.Id, 1949, 3);
        var createResponse = await _client.PostAsJsonAsync("/api/books", createCommand);
        var createdBook = await createResponse.Content.ReadFromJsonAsync<GetBookDto>();

        // Act
        var response = await _client.GetAsync($"/api/books/{createdBook!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var book = await response.Content.ReadFromJsonAsync<GetBookDto>();
        book.Should().NotBeNull();
        book!.Id.Should().Be(createdBook.Id);
        book.ISBN.Should().Be("9780451524935");
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenBookDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync("/api/books/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_ReturnsOk_WithValidData()
    {
        // Arrange - Create a book first
        var author = await CreateTestAuthor();
        var createCommand = new CreateBookCommand("9780061120084", "To Kill a Mockingbird", author.Id, 1960, 2);
        var createResponse = await _client.PostAsJsonAsync("/api/books", createCommand);
        var createdBook = await createResponse.Content.ReadFromJsonAsync<GetBookDto>();

        var updateCommand = new UpdateBookCommand(
            createdBook!.Id,
            "9780061120084",
            "Updated Title",
            author.Id,
            1960,
            5
        );

        // Act
        var response = await _client.PutAsJsonAsync($"/api/books/{createdBook.Id}", updateCommand);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var updatedBook = await response.Content.ReadFromJsonAsync<GetBookDto>();
        updatedBook!.Title.Should().Be("Updated Title");
        updatedBook.AvailableCopies.Should().Be(5);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenIdMismatch()
    {
        // Arrange
        var author = await CreateTestAuthor();
        var updateCommand = new UpdateBookCommand(1, "9780747532699", "Test", author.Id, 2020, 1);

        // Act
        var response = await _client.PutAsJsonAsync("/api/books/2", updateCommand);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenBookExists()
    {
        // Arrange - Create a book first
        var author = await CreateTestAuthor();
        var createCommand = new CreateBookCommand("9780743273565", "The Great Gatsby", author.Id, 1925, 1);
        var createResponse = await _client.PostAsJsonAsync("/api/books", createCommand);
        var createdBook = await createResponse.Content.ReadFromJsonAsync<GetBookDto>();

        // Act
        var response = await _client.DeleteAsync($"/api/books/{createdBook!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify deletion
        var getResponse = await _client.GetAsync($"/api/books/{createdBook.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenBookDoesNotExist()
    {
        // Act
        var response = await _client.DeleteAsync("/api/books/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_ReturnsConflict_WithDuplicateISBN()
    {
        // Arrange - Create a book with an ISBN
        var author = await CreateTestAuthor();
        var firstCommand = new CreateBookCommand("9780544003415", "The Lord of the Rings", author.Id, 1954, 3);
        await _client.PostAsJsonAsync("/api/books", firstCommand);

        // Try to create another book with the same ISBN
        var duplicateCommand = new CreateBookCommand("9780544003415", "Different Title", author.Id, 2000, 1);

        // Act
        var response = await _client.PostAsJsonAsync("/api/books", duplicateCommand);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}
