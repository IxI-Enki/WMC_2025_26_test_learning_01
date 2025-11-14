using Domain.ValidationSpecifications;
using Xunit;

namespace Domain.Tests;

/// <summary>
/// Unit Tests für EventSpecifications.
/// Diese Tests helfen dir bei der Implementierung der Domain-Validierungen.
/// </summary>
public class EventSpecificationsTests
{
    [Fact]
    public void CheckVenueId_ValidId_ReturnsSuccess()
    {
        // TODO: Implementiere CheckVenueId in EventSpecifications, dann wird dieser Test grün
        var result = EventSpecifications.CheckVenueId(1);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CheckVenueId_InvalidId_ReturnsFailure(int venueId)
    {
        // TODO: Implementiere CheckVenueId in EventSpecifications
        var result = EventSpecifications.CheckVenueId(venueId);
        Assert.False(result.IsValid);
        Assert.Equal("VenueId muss größer als 0 sein.", result.ErrorMessage);
    }

    [Fact]
    public void CheckDateTime_FutureDate_ReturnsSuccess()
    {
        // TODO: Implementiere CheckDateTime in EventSpecifications
        var futureDate = DateTime.Now.AddDays(1);
        var result = EventSpecifications.CheckDateTime(futureDate);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void CheckDateTime_PastDate_ReturnsFailure()
    {
        // TODO: Implementiere CheckDateTime in EventSpecifications
        var pastDate = DateTime.Now.AddDays(-1);
        var result = EventSpecifications.CheckDateTime(pastDate);
        Assert.False(result.IsValid);
        Assert.Equal("DateTime darf nicht in der Vergangenheit liegen.", result.ErrorMessage);
    }

    [Fact]
    public void CheckMaxAttendeesNotExceedVenueCapacity_Valid_ReturnsSuccess()
    {
        // TODO: Implementiere CheckMaxAttendeesNotExceedVenueCapacity in EventSpecifications
        var result = EventSpecifications.CheckMaxAttendeesNotExceedVenueCapacity(100, 200);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void CheckMaxAttendeesNotExceedVenueCapacity_Exceeds_ReturnsFailure()
    {
        // TODO: Implementiere CheckMaxAttendeesNotExceedVenueCapacity in EventSpecifications
        var result = EventSpecifications.CheckMaxAttendeesNotExceedVenueCapacity(300, 200);
        Assert.False(result.IsValid);
        Assert.Equal("MaxAttendees darf die Venue-Kapazität nicht überschreiten.", result.ErrorMessage);
    }
}
