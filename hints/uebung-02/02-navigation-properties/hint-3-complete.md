# Hint 3: Navigation Properties - Vollständige Lösung

## Kompletter BookRepository Code

```csharp
using Application.Contracts.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository für Book mit Navigation Property Loading.
/// </summary>
public class BookRepository(AppDbContext dbContext) 
    : GenericRepository<Book>(dbContext), IBookRepository
{
    /// <summary>
    /// Lädt ein Book nach ID MIT Author Navigation Property.
    /// </summary>
    public override async Task<Book?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await Set
            .Include(b => b.Author)  // ← Author laden!
            .FirstOrDefaultAsync(b => b.Id == id, ct);
    }

    /// <summary>
    /// Lädt alle Books MIT Author Navigation Properties.
    /// </summary>
    public override async Task<IReadOnlyCollection<Book>> GetAllAsync(
        Func<IQueryable<Book>, IOrderedQueryable<Book>>? orderBy = null,
        Expression<Func<Book, bool>>? filter = null,
        CancellationToken ct = default)
    {
        IQueryable<Book> query = Set
            .Include(b => b.Author)  // ← Author laden!
            .AsNoTracking();
        
        if (filter is not null)
            query = query.Where(filter);
        if (orderBy is not null)
            query = orderBy(query);
        
        return await query.ToListAsync(ct);
    }

    /// <summary>
    /// Lädt ein Book nach ISBN MIT Author Navigation Property.
    /// </summary>
    public async Task<Book?> GetByISBNAsync(string isbn, CancellationToken ct = default)
    {
        return await Set
            .Include(b => b.Author)  // ← Author laden!
            .SingleOrDefaultAsync(b => b.ISBN == isbn, ct);
    }

    /// <summary>
    /// Lädt ein Book nach ID für spezifische Queries MIT Author.
    /// </summary>
    public async Task<Book?> GetByBookIdAsync(int bookId, CancellationToken ct = default)
    {
        return await Set
            .Include(b => b.Author)  // ← Author laden!
            .FirstOrDefaultAsync(b => b.Id == bookId, ct);
    }
}
```

---

## Erklärung

### Warum FirstOrDefaultAsync statt FindAsync?

**FindAsync:**
- Schneller für Primary Key Lookup
- **ABER:** Unterstützt KEIN `.Include()`!

**FirstOrDefaultAsync:**
- Unterstützt `.Include()`
- Kann mit beliebigen Bedingungen verwendet werden

```csharp
// ❌ GEHT NICHT:
await Set.Include(b => b.Author).FindAsync(id);

// ✅ FUNKTIONIERT:
await Set.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);
```

---

### Warum SingleOrDefaultAsync bei ISBN?

**ISBN ist Unique!**

`SingleOrDefaultAsync`:
- Gibt **1** Treffer zurück
- Wirft **Exception** bei mehreren Treffern
- Gut für Validierung: Wenn mehrere Books mit gleicher ISBN → DB-Inkonsistenz!

```csharp
// ISBN ist Unique Constraint:
await Set.SingleOrDefaultAsync(b => b.ISBN == isbn, ct);
```

---

### Include Reihenfolge

```csharp
// ✅ RICHTIG:
IQueryable<Book> query = Set
    .Include(b => b.Author)  // 1. Include
    .AsNoTracking();         // 2. AsNoTracking

// ❌ FALSCH (funktioniert aber ist unüblich):
IQueryable<Book> query = Set
    .AsNoTracking()
    .Include(b => b.Author);
```

**Best Practice:** Include ZUERST, dann Tracking-Behavior.

---

### Filter und OrderBy

```csharp
// Reihenfolge:
// 1. Include
// 2. AsNoTracking
// 3. Filter (Where)
// 4. OrderBy

IQueryable<Book> query = Set
    .Include(b => b.Author)      // 1
    .AsNoTracking();             // 2

if (filter is not null)
    query = query.Where(filter); // 3
if (orderBy is not null)
    query = orderBy(query);      // 4
```

---

## Mapster Configuration

```csharp
// In Application/Common/Mappings/BookMappingConfig.cs:
using Application.Dtos;
using Domain.Entities;
using Mapster;

namespace Application.Common.Mappings;

public static class BookMappingConfig
{
    public static void ConfigureBookMappings()
    {
        TypeAdapterConfig<Book, GetBookDto>.NewConfig()
            .Map(dest => dest.ISBN, src => src.ISBN)
            .Map(dest => dest.AuthorId, src => src.AuthorId)
            .Map(dest => dest.AvailableCopies, src => src.AvailableCopies)
            .Map(dest => dest.AuthorName, src => src.Author != null 
                ? $"{src.Author.FirstName} {src.Author.LastName}" 
                : null);
    }
}
```

**Registrierung:**
```csharp
// In Application/DependencyInjection.cs:
public static IServiceCollection AddApplication(this IServiceCollection services)
{
    BookMappingConfig.ConfigureBookMappings();  // ← Hier aufrufen!
    
    services.AddMediatR(cfg => { ... });
    // ...
    
    return services;
}
```

---

## Häufige Fehler

### ❌ Include vergessen
```csharp
// Author ist null!
return await Set.FirstOrDefaultAsync(b => b.Id == id, ct);
```

### ❌ FindAsync mit Include
```csharp
// Kompiliert nicht!
return await Set.Include(b => b.Author).FindAsync(id);
```

### ❌ Include nach AsNoTracking
```csharp
// Funktioniert, aber unüblich
return await Set
    .AsNoTracking()
    .Include(b => b.Author)  // Besser vorher!
    .FirstOrDefaultAsync(...);
```

### ❌ Mapster Config nicht registriert
```csharp
// AuthorName bleibt null, obwohl Author geladen!
// → BookMappingConfig.ConfigureBookMappings() vergessen
```

---

## Testen

### 1. Unit Test (Domain.Tests)
```csharp
// Test lädt Book und prüft Author
var book = await repository.GetByIdAsync(1, ct);
Assert.NotNull(book);
Assert.NotNull(book.Author);  // ← Sollte nicht null sein!
```

### 2. Integration Test (Swagger)
```bash
GET /api/Books/1
```

**Erwartete Response:**
```json
{
  "id": 1,
  "isbn": "9780747532699",
  "title": "Harry Potter and the Philosopher's Stone",
  "authorId": 1,
  "authorName": "J.K. Rowling",  // ← Nicht null!
  "publicationYear": 1997,
  "availableCopies": 5
}
```

### 3. Alle Books
```bash
GET /api/Books
```

**Alle** Books sollten `authorName` haben!

---

## Warum ist das richtig?

1. **Eager Loading:** Author wird MIT geladen (nicht lazy)
2. **Performance:** Ein Query statt N+1 Queries
3. **Konsistent:** Alle Read-Methoden laden Navigation Property
4. **Template-konform:** Gleiche Struktur wie SensorRepository
5. **Type-Safe:** Compiler prüft `.Include(b => b.Author)`

---

**✅ Fertig! Navigation Properties funktionieren jetzt!**

---

## Zusammenfassung

**Problem:** Author ist null  
**Ursache:** EF Core lädt nicht automatisch  
**Lösung:** `.Include(b => b.Author)` in Repository  
**Pattern:** Override + Include + FirstOrDefaultAsync  
**Bonus:** Mapster Config für computed properties

---

## Nächster Schritt

Jetzt funktionieren deine API-Responses vollständig! 🎉

Schaue dir auch an:
- CSV-Seeding Hints (wenn Books nicht importiert werden)
- Custom Repository Methods (wenn du eigene Queries brauchst)

