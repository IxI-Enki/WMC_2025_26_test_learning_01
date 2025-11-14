namespace Infrastructure.Services;

/// <summary>
/// Konfigurationsoptionen für den DataSeeder.
/// </summary>
public sealed class StartupDataSeederOptions
{
    /// <summary>
    /// Pfad zur JSON-Datei mit Seed-Daten.
    /// </summary>
    public string JsonPath { get; set; } = string.Empty;
}

