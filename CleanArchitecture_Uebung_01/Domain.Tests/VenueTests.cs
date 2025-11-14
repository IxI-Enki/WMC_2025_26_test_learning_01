using Domain.Contracts;
using Domain.Entities;
using Domain.Exceptions;
using Xunit;

namespace Domain.Tests;

/// <summary>
/// Unit Tests für die Venue-Entität.
/// Diese Tests helfen dir zu verstehen, wie die Domain-Validierung funktioniert.
/// </summary>
public class VenueTests
{
    private class FakeUniquenessChecker(bool unique) : IVenueUniquenessChecker
    {
        public Task<bool> IsUniqueAsync(int id, string name, CancellationToken ct = default) 
            => Task.FromResult(unique);
    }

    [Fact]
    public async Task CreateAsync_Succeeds_WithValidData()
    {
        var checker = new FakeUniquenessChecker(true);
        var venue = await Venue.CreateAsync("Wiener Stadthalle", "Roland-Rainer-Platz 1, 1150 Wien", 16000, checker);
        
        Assert.Equal("Wiener Stadthalle", venue.Name);
        Assert.Equal("Roland-Rainer-Platz 1, 1150 Wien", venue.Address);
        Assert.Equal(16000, venue.Capacity);
    }

    [Theory]
    [InlineData(null, "Adresse 1", 100, "Name darf nicht leer sein.")]
    [InlineData("", "Adresse 1", 100, "Name darf nicht leer sein.")]
    [InlineData("AB", "Adresse 1", 100, "Name muss mindestens 3 Zeichen haben.")]
    [InlineData("ValidName", "", 100, "Address darf nicht leer sein.")]
    [InlineData("ValidName", "ABC", 100, "Address muss mindestens 5 Zeichen haben.")]
    [InlineData("ValidName", "ValidAddress", 0, "Capacity muss mindestens 1 sein.")]
    [InlineData("ValidName", "ValidAddress", -5, "Capacity muss mindestens 1 sein.")]
    public async Task CreateAsync_InvalidRules_Throws(string? name, string? address, int capacity, string expectedMessage)
    {
        var checker = new FakeUniquenessChecker(true);
        var ex = await Assert.ThrowsAsync<DomainValidationException>(
            () => Venue.CreateAsync(name ?? string.Empty, address ?? string.Empty, capacity, checker));
        Assert.Equal(expectedMessage, ex.Message);
    }

    [Fact]
    public async Task CreateAsync_Duplicate_Throws()
    {
        var checker = new FakeUniquenessChecker(false);
        var ex = await Assert.ThrowsAsync<DomainValidationException>(
            () => Venue.CreateAsync("Wiener Stadthalle", "Adresse 1", 100, checker));
        Assert.Equal("Ein Veranstaltungsort mit diesem Namen existiert bereits.", ex.Message);
    }

    [Fact]
    public async Task UpdateAsync_ChangesValues_WhenUnique()
    {
        var checker = new FakeUniquenessChecker(true);
        var venue = await Venue.CreateAsync("Old Name", "Old Address", 100, checker);
        
        await venue.UpdateAsync("New Name", "New Address Longer", 200, checker);
        
        Assert.Equal("New Name", venue.Name);
        Assert.Equal("New Address Longer", venue.Address);
        Assert.Equal(200, venue.Capacity);
    }
}
