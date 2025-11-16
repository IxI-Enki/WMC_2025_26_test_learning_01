using Api.Tests.Utilities;
using Application.Dtos;
using Application.Features.Authors.Commands.CreateAuthor;
using Application.Features.Authors.Commands.UpdateAuthor;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Api.Tests.Authors;

/// <summary>
/// Integration Tests für Author Endpoints.
/// </summary>
public class AuthorsEndpointTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthorsEndpointTests(TestWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithAuthors()
    {
        // Act
        var response = await _client.GetAsync("/api/autors");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var authors = await response.Content.ReadFromJsonAsync<List<GetAuthorDto>>();
        authors.Should().NotBeNull();
    }

    [Fact]
    public async Task Create_ReturnsCreated_WithValidData()
    {
        // Arrange
        var command = new CreateAuthorCommand("Stephen", "King", new DateTime(1947, 9, 21));

        // Act
        var response = await _client.PostAsJsonAsync("/api/autors", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var author = await response.Content.ReadFromJsonAsync<GetAuthorDto>();
        author.Should().NotBeNull();
        author!.FirstName.Should().Be("Stephen");
        author.LastName.Should().Be("King");
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WithInvalidData()
    {
        // Arrange - Empty FirstName is invalid
        var command = new CreateAuthorCommand("", "King", new DateTime(1947, 9, 21));

        // Act
        var response = await _client.PostAsJsonAsync("/api/autors", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenAuthorExists()
    {
        // Arrange - Create an author first
        var createCommand = new CreateAuthorCommand("J.K.", "Rowling", new DateTime(1965, 7, 31));
        var createResponse = await _client.PostAsJsonAsync("/api/autors", createCommand);
        var createdAuthor = await createResponse.Content.ReadFromJsonAsync<GetAuthorDto>();

        // Act
        var response = await _client.GetAsync($"/api/autors/{createdAuthor!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var author = await response.Content.ReadFromJsonAsync<GetAuthorDto>();
        author.Should().NotBeNull();
        author!.Id.Should().Be(createdAuthor.Id);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenAuthorDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync("/api/autors/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_ReturnsOk_WithValidData()
    {
        // Arrange - Create an author first
        var createCommand = new CreateAuthorCommand("Test", "Author", new DateTime(1980, 1, 1));
        var createResponse = await _client.PostAsJsonAsync("/api/autors", createCommand);
        var createdAuthor = await createResponse.Content.ReadFromJsonAsync<GetAuthorDto>();

        var updateCommand = new UpdateAuthorCommand(createdAuthor!.Id, "Updated", "Name", new DateTime(1980, 1, 1));

        // Act
        var response = await _client.PutAsJsonAsync($"/api/autors/{createdAuthor.Id}", updateCommand);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var updatedAuthor = await response.Content.ReadFromJsonAsync<GetAuthorDto>();
        updatedAuthor!.FirstName.Should().Be("Updated");
        updatedAuthor.LastName.Should().Be("Name");
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenIdMismatch()
    {
        // Arrange
        var updateCommand = new UpdateAuthorCommand(1, "Test", "Author", new DateTime(1980, 1, 1));

        // Act
        var response = await _client.PutAsJsonAsync("/api/autors/2", updateCommand);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenAuthorExists()
    {
        // Arrange - Create an author first
        var createCommand = new CreateAuthorCommand("Delete", "Me", new DateTime(1990, 1, 1));
        var createResponse = await _client.PostAsJsonAsync("/api/autors", createCommand);
        var createdAuthor = await createResponse.Content.ReadFromJsonAsync<GetAuthorDto>();

        // Act
        var response = await _client.DeleteAsync($"/api/autors/{createdAuthor!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify deletion
        var getResponse = await _client.GetAsync($"/api/autors/{createdAuthor.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenAuthorDoesNotExist()
    {
        // Act
        var response = await _client.DeleteAsync("/api/autors/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}

