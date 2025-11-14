using Api.Tests.Utilities;
using Application.Features.Dtos;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Api.Tests.Venues;

/// <summary>
/// Integration Tests für Venue Endpoints.
/// Diese Tests helfen dir zu verstehen, wie die API funktionieren soll.
/// </summary>
public class VenuesEndpointTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public VenuesEndpointTests(TestWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WhenCalled()
    {
        // Act
        var response = await _client.GetAsync("/api/venues");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var venues = await response.Content.ReadFromJsonAsync<List<GetVenueDto>>();
        venues.Should().NotBeNull();
    }

    [Fact]
    public async Task Create_ReturnsCreated_WithValidData()
    {
        // Arrange
        var command = new
        {
            Name = "Test Venue " + Guid.NewGuid(),
            Address = "Test Address 123",
            Capacity = 500
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/venues", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var venue = await response.Content.ReadFromJsonAsync<GetVenueDto>();
        venue.Should().NotBeNull();
        venue!.Name.Should().Be(command.Name);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WithInvalidData()
    {
        // Arrange - Name zu kurz (muss mindestens 3 Zeichen haben)
        var command = new
        {
            Name = "AB",
            Address = "Test Address 123",
            Capacity = 500
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/venues", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_ReturnsConflict_WithDuplicateName()
    {
        // Arrange
        var uniqueName = "Unique Venue " + Guid.NewGuid();
        var command1 = new
        {
            Name = uniqueName,
            Address = "Address 1",
            Capacity = 100
        };
        var command2 = new
        {
            Name = uniqueName,
            Address = "Address 2",
            Capacity = 200
        };

        // Act
        await _client.PostAsJsonAsync("/api/venues", command1);
        var response2 = await _client.PostAsJsonAsync("/api/venues", command2);

        // Assert
        response2.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}
