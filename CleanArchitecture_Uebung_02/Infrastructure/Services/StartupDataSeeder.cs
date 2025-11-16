using Application.Interfaces;
using Domain.Contracts;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace Infrastructure.Services;

/*
 * CSV-basierter DataSeeder nach Template-Pattern!
 * 
 * dotnet ef migrations add InitialMigration --project ./Infrastructure --startup-project ./Api --output-dir ./Persistence/Migrations
 * dotnet ef database update --project ./Infrastructure --startup-project ./Api
 */

/// <summary>
/// Hosted Service, der beim Start Migrationen ausführt und die DB einmalig aus CSV befüllt.
/// Verwendet das EXAKTE Pattern des CleanArchitecture_Template!
/// </summary>
public class StartupDataSeeder(IOptions<StartupDataSeederOptions> options, IServiceProvider serviceProvider) 
    : IHostedService
{
    private readonly SemaphoreSlim _lock = new(1, 1);
    private readonly string _csvPath = options.Value.CsvPath;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (dbContext.Database.CanConnect())
        {
            // Nur seeden, wenn noch keine Authors existieren (idempotent)
            if (await dbContext.Authors.AnyAsync(cancellationToken)) return;
            await dbContext.Database.EnsureDeletedAsync(cancellationToken);
        }

        await dbContext.Database.MigrateAsync(cancellationToken: cancellationToken);
        await dbContext.Database.EnsureCreatedAsync(cancellationToken);

        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var allBooks = await ReadBooksFromCsv(uow, cancellationToken);
        dbContext.Books.AddRange(allBooks);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Lädt die CSV nur einmalig (Thread-sicher) und baut Author- und Book-Objekte.
    /// Pattern: Authors werden SOFORT gespeichert (um IDs zu bekommen), dann Books erstellt.
    /// </summary>
    private async Task<IEnumerable<Book>> ReadBooksFromCsv(IUnitOfWork uow, CancellationToken cancellationToken)
    {
        await _lock.WaitAsync(cancellationToken);
        var authorChecker = new SeedDataUniquenessChecker();
        var bookChecker = new SeedDataUniquenessChecker();
        
        try
        {
            if (!File.Exists(_csvPath))
            {
                throw new Exception($"CSV file {_csvPath} doesn't exist");
            }

            var lines = await File.ReadAllLinesAsync(_csvPath, cancellationToken);
            var authors = new Dictionary<string, Author>(); // Key: "FirstName|LastName"
            var books = new List<Book>();

            for (int i = 1; i < lines.Length; i++) // i=1: Header überspringen
            {
                var line = lines[i];
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(';');
                if (parts.Length < 7) continue;

                // Parse CSV: FirstName;LastName;DateOfBirth;ISBN;Title;PublicationYear;AvailableCopies
                var firstName = parts[0].Trim();
                var lastName = parts[1].Trim();
                var dateOfBirthStr = parts[2].Trim();
                var isbn = parts[3].Trim();
                var title = parts[4].Trim();
                var publicationYearStr = parts[5].Trim();
                var availableCopiesStr = parts[6].Trim();

                if (!DateTime.TryParse(dateOfBirthStr, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateOfBirth))
                    continue;
                if (!int.TryParse(publicationYearStr, out var publicationYear))
                    continue;
                if (!int.TryParse(availableCopiesStr, out var availableCopies))
                    continue;

                // Author holen oder erstellen (und SOFORT speichern!)
                var authorKey = $"{firstName}|{lastName}";
                if (!authors.TryGetValue(authorKey, out var author))
                {
                    author = await Author.CreateAsync(firstName, lastName, dateOfBirth, authorChecker, cancellationToken);
                    authors[authorKey] = author;
                    await uow.Authors.AddAsync(author, cancellationToken);
                    await uow.SaveChangesAsync(cancellationToken); // ✅ SOFORT speichern - Author bekommt ID!
                }

                // Book mit gespeichertem Author erstellen
                var book = await Book.CreateAsync(isbn, title, author, publicationYear, availableCopies, bookChecker, cancellationToken);
                books.Add(book);
            }

            return books;
        }
        finally
        {
            _lock.Release();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

/// <summary>
/// NoOp Uniqueness Checker für Seed-Daten.
/// Bypasses Uniqueness-Validierung da beim Seeding die DB noch leer/kontrolliert ist.
/// Verwendet explizite Interface-Implementierung.
/// </summary>
internal class SeedDataUniquenessChecker : IAuthorUniquenessChecker, IBookUniquenessChecker
{
    Task<bool> IAuthorUniquenessChecker.IsUniqueAsync(int id, string fullName, CancellationToken ct)
        => Task.FromResult(true);

    Task<bool> IBookUniquenessChecker.IsUniqueAsync(int id, string isbn, CancellationToken ct)
        => Task.FromResult(true);
}
