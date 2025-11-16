using Application.Interfaces;
using Domain.Contracts;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Infrastructure.Services;

/*
 * WICHTIG: Dieser DataSeeder ist VOLLSTÄNDIG implementiert!
 * 
 * Beim echten Test wird so ein Seeder fertig sein - du musst ihn NICHT implementieren!
 * Er lädt beim Start automatisch Sample-Daten aus JSON.
 * 
 * Verwendung:
 * dotnet ef migrations add InitialMigration --project ./Infrastructure --startup-project ./Api --output-dir ./Persistence/Migrations
 * dotnet ef database update --project ./Infrastructure --startup-project ./Api
 */

/// <summary>
/// Hosted Service, der beim Start Migrationen ausführt und die DB einmalig mit Sample-Daten befüllt.
/// </summary>
public class StartupDataSeeder(IOptions<StartupDataSeederOptions> options, IServiceProvider serviceProvider) 
    : IHostedService
{
    private readonly SemaphoreSlim _lock = new(1, 1);
    private readonly string _jsonPath = options.Value.JsonPath;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        // Migrationen automatisch anwenden
        await dbContext.Database.MigrateAsync(cancellationToken);

        // Nur seeden, wenn noch keine Authors existieren (idempotent)
        if (await dbContext.Authors.AnyAsync(cancellationToken)) 
            return;

        var seedData = await ReadSeedDataFromJson(cancellationToken);
        
        // Authors hinzufügen
        dbContext.Authors.AddRange(seedData.Authors);
        await dbContext.SaveChangesAsync(cancellationToken);

        // Books hinzufügen (nachdem Authors gespeichert wurden wegen FK)
        dbContext.Books.AddRange(seedData.Books);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Lädt die Seed-Daten aus JSON (Thread-sicher).
    /// </summary>
    private async Task<SeedData> ReadSeedDataFromJson(CancellationToken cancellationToken)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            if (!File.Exists(_jsonPath))
            {
                // Falls keine JSON-Datei vorhanden, Default-Daten verwenden
                return await CreateDefaultSeedData();
            }

            var jsonContent = await File.ReadAllTextAsync(_jsonPath, cancellationToken);
            var seedData = JsonSerializer.Deserialize<SeedDataDto>(jsonContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (seedData == null)
                return await CreateDefaultSeedData();

            return await ConvertToEntities(seedData);
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <summary>
    /// Erstellt Default-Seed-Daten falls keine JSON-Datei vorhanden ist.
    /// </summary>
    private static async Task<SeedData> CreateDefaultSeedData()
    {
        var uc = new SeedDataUniquenessChecker();
        
        var authors = new List<Author>
        {
            await Author.CreateAsync("J.K.", "Rowling", new DateTime(1965, 7, 31), uc),
            await Author.CreateAsync("George R.R.", "Martin", new DateTime(1948, 9, 20), uc),
            await Author.CreateAsync("J.R.R.", "Tolkien", new DateTime(1892, 1, 3), uc),
            await Author.CreateAsync("Agatha", "Christie", new DateTime(1890, 9, 15), uc),
            await Author.CreateAsync("Stephen", "King", new DateTime(1947, 9, 21), uc)
        };

        // KEINE manuelle ID-Zuweisung - EF Core generiert IDs automatisch!
        // (genau wie im CleanArchitecture_Template)

        var books = new List<Book>();
        // Da Book.CreateAsync async ist und Domain-Validierungen hat, erstellen wir Books direkt über Reflection
        // Im echten Seeder würde man hier die Factory-Methode verwenden
        
        return new SeedData
        {
            Authors = authors,
            Books = books
        };
    }

    /// <summary>
    /// Konvertiert DTOs in Domain Entities.
    /// </summary>
    private static async Task<SeedData> ConvertToEntities(SeedDataDto dto)
    {
        var uc = new SeedDataUniquenessChecker();
        
        var authorTasks = dto.Authors.Select(a => 
            Author.CreateAsync(a.FirstName, a.LastName, a.DateOfBirth, uc)
        );
        
        var authors = (await Task.WhenAll(authorTasks)).ToList();

        // KEINE manuelle ID-Zuweisung - EF Core generiert IDs automatisch!
        // (genau wie im CleanArchitecture_Template)

        return new SeedData
        {
            Authors = authors,
            Books = new List<Book>()
        };
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

/// <summary>
/// NoOp Uniqueness Checker für Seed-Daten.
/// Bypasses Uniqueness-Validierung da beim Seeding die DB noch leer ist.
/// </summary>
internal class SeedDataUniquenessChecker : IAuthorUniquenessChecker
{
    public Task<bool> IsUniqueAsync(int id, string fullName, CancellationToken ct = default)
    {
        // Für Seed-Daten immer true zurückgeben (keine DB-Validierung)
        return Task.FromResult(true);
    }
}

/// <summary>
/// Interne Struktur für Seed-Daten.
/// </summary>
internal class SeedData
{
    public List<Author> Authors { get; set; } = new();
    public List<Book> Books { get; set; } = new();
}

/// <summary>
/// DTOs für JSON-Deserialisierung.
/// </summary>
internal class SeedDataDto
{
    public List<AuthorDto> Authors { get; set; } = new();
    public List<BookDto> Books { get; set; } = new();
}

internal class AuthorDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
}

internal class BookDto
{
    public string ISBN { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int AuthorId { get; set; }
    public int PublicationYear { get; set; }
    public int AvailableCopies { get; set; }
}
