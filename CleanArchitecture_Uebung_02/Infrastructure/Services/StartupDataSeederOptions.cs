namespace Infrastructure.Services;

/// <summary>
/// Konfigurationsoptionen für den DataSeeder.
/// </summary>
public sealed class StartupDataSeederOptions
{
    /// <summary>
    /// Pfad zur CSV-Datei mit Seed-Daten.
    /// </summary>
    public string CsvPath { get; set; } = string.Empty;
}

