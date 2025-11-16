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

        // Books erstellen (mit Author-Referenzen)
        // Die Author-IDs werden von EF Core automatisch gesetzt nach SaveChangesAsync
        var books = new List<Book>
        {
            await Book.CreateAsync("9780747532699", "Harry Potter and the Philosopher's Stone", authors[0], 1997, 5, uc),
            await Book.CreateAsync("9780439064873", "Harry Potter and the Chamber of Secrets", authors[0], 1998, 3, uc),
            await Book.CreateAsync("9780553103540", "A Game of Thrones", authors[1], 1996, 4, uc),
            await Book.CreateAsync("9780553108033", "A Clash of Kings", authors[1], 1998, 2, uc),
            await Book.CreateAsync("9780395595114", "The Fellowship of the Ring", authors[2], 1954, 6, uc),
            await Book.CreateAsync("9780395082560", "The Two Towers", authors[2], 1954, 4, uc),
            await Book.CreateAsync("9780062073501", "Murder on the Orient Express", authors[3], 1934, 5, uc),
            await Book.CreateAsync("9780062073488", "And Then There Were None", authors[3], 1939, 3, uc),
            await Book.CreateAsync("9780307743657", "The Shining", authors[4], 1977, 4, uc),
            await Book.CreateAsync("9781501142970", "IT", authors[4], 1986, 2, uc)
        };
        
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

        // Books erstellen (mit Author-Referenzen aus JSON)
        var books = new List<Book>();
        foreach (var bookDto in dto.Books)
        {
            // Finde den entsprechenden Author (über Index, da IDs noch nicht gesetzt sind)
            if (bookDto.AuthorId > 0 && bookDto.AuthorId <= authors.Count)
            {
                var author = authors[bookDto.AuthorId - 1]; // AuthorId in JSON ist 1-basiert
                var book = await Book.CreateAsync(
                    bookDto.ISBN,
                    bookDto.Title,
                    author,
                    bookDto.PublicationYear,
                    bookDto.AvailableCopies,
                    uc
                );
                books.Add(book);
            }
        }

        return new SeedData
        {
            Authors = authors,
            Books = books
        };
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

/// <summary>
/// NoOp Uniqueness Checker für Seed-Daten.
/// Bypasses Uniqueness-Validierung da beim Seeding die DB noch leer ist.
/// Verwendet explizite Interface-Implementierung, da beide Interfaces die gleiche Methodensignatur haben.
/// </summary>
internal class SeedDataUniquenessChecker : IAuthorUniquenessChecker, IBookUniquenessChecker
{
    // Explizite Interface-Implementierung für IAuthorUniquenessChecker
    Task<bool> IAuthorUniquenessChecker.IsUniqueAsync(int id, string fullName, CancellationToken ct)
    {
        // Für Seed-Daten immer true zurückgeben (keine DB-Validierung)
        return Task.FromResult(true);
    }

    // Explizite Interface-Implementierung für IBookUniquenessChecker
    Task<bool> IBookUniquenessChecker.IsUniqueAsync(int id, string isbn, CancellationToken ct)
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
