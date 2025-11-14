using Domain.Contracts;
using Domain.Entities;
using Domain.Exceptions;
using Xunit;

namespace Domain.Tests;

/// <summary>
/// TODO: Dieser Test wird erst funktionieren, wenn du die Event.ValidateEventProperties Methode implementiert hast.
/// 
/// HINWEIS: Wenn du die Domain-Validierung für Events implementiert hast, sollten diese Tests grün werden.
/// </summary>
public class EventTests
{
    private class FakeUniquenessChecker(bool unique) : IVenueUniquenessChecker
    {
        public Task<bool> IsUniqueAsync(int id, string name, CancellationToken ct = default) 
            => Task.FromResult(unique);
    }

    [Fact]
    public async Task Create_Succeeds_WithValidData()
    {
        var checker = new FakeUniquenessChecker(true);
        var venue = await Venue.CreateAsync("Test Venue", "Test Address", 1000, checker);
        var futureDate = DateTime.Now.AddDays(30);

        var evt = Event.Create(venue, "Rock Concert", futureDate, 500);

        Assert.Equal("Rock Concert", evt.Name);
        Assert.Equal(futureDate, evt.DateTime);
        Assert.Equal(500, evt.MaxAttendees);
    }

    [Fact]
    public async Task Create_ThrowsException_WhenDateTimeInPast()
    {
        var checker = new FakeUniquenessChecker(true);
        var venue = await Venue.CreateAsync("Test Venue", "Test Address", 1000, checker);
        var pastDate = DateTime.Now.AddDays(-1);

        // TODO: Dieser Test schlägt fehl, bis du die Validierung implementiert hast
        var ex = Assert.Throws<DomainValidationException>(
            () => Event.Create(venue, "Past Event", pastDate, 100));
        
        Assert.Contains("Vergangenheit", ex.Message);
    }

    [Fact]
    public async Task Create_ThrowsException_WhenMaxAttendeesExceedsVenueCapacity()
    {
        var checker = new FakeUniquenessChecker(true);
        var venue = await Venue.CreateAsync("Small Venue", "Test Address", 100, checker);
        var futureDate = DateTime.Now.AddDays(30);

        // TODO: Dieser Test schlägt fehl, bis du die Validierung implementiert hast
        var ex = Assert.Throws<DomainValidationException>(
            () => Event.Create(venue, "Big Event", futureDate, 200));
        
        Assert.Contains("Kapazität", ex.Message);
    }
}
