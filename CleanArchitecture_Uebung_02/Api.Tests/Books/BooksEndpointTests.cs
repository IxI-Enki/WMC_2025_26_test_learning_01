using Api.Tests.Utilities;
using Application.Dtos;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Api.Tests.Books;

/// <summary>
/// Integration Tests für Book Endpoints.
/// Diese Tests helfen dir zu verstehen, wie die API funktionieren soll.
/// </summary>
public class BooksEndpointTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public BooksEndpointTests(TestWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WhenCalled()
    {
        var response = await _client.GetAsync("/api/books");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var books = await response.Content.ReadFromJsonAsync<List<GetBookDto>>();
        books.Should().NotBeNull();
    }
}

