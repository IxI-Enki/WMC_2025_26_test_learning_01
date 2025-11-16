# Hint 2: Navigation Properties - Struktur & Pattern

## Welche Methoden anpassen?

In `BookRepository.cs` musst du **3 Methoden** überschreiben:

1. `GetByIdAsync` - Einzelnes Book laden
2. `GetAllAsync` - Alle Books laden
3. `GetByISBNAsync` - Book nach ISBN laden (Custom-Methode)

## Pattern: Override mit .Include()

### 1. GetByIdAsync Override

```csharp
public override async Task<Book?> GetByIdAsync(int id, CancellationToken ct = default)
{
    return await Set
        .Include(b => b.Author)  // ← WICHTIG!
        .FirstOrDefaultAsync(b => b.Id == id, ct);
}
```

**Wichtig:**
- `override` - Überschreibt die GenericRepository Methode
- `.Include(b => b.Author)` - Lädt Author mit
- `.FirstOrDefaultAsync` - Nicht FindAsync! (FindAsync unterstützt kein Include)

---

### 2. GetAllAsync Override

```csharp
public override async Task<IReadOnlyCollection<Book>> GetAllAsync(
    Func<IQueryable<Book>, IOrderedQueryable<Book>>? orderBy = null,
    Expression<Func<Book, bool>>? filter = null,
    CancellationToken ct = default)
{
    IQueryable<Book> query = Set
        .Include(b => b.Author)  // ← WICHTIG!
        .AsNoTracking();
    
    if (filter is not null)
        query = query.Where(filter);
    if (orderBy is not null)
        query = orderBy(query);
    
    return await query.ToListAsync(ct);
}
```

**Wichtig:**
- Include BEFORE AsNoTracking
- Filter und OrderBy AFTER Include

---

### 3. GetByISBNAsync (Custom-Methode)

```csharp
public async Task<Book?> GetByISBNAsync(string isbn, CancellationToken ct = default)
{
    return await Set
        .Include(b => b.Author)  // ← WICHTIG!
        .SingleOrDefaultAsync(b => b.ISBN == isbn, ct);
}
```

**Wichtig:**
- `SingleOrDefaultAsync` - weil ISBN Unique ist
- Wirft Exception bei mehreren Treffern (gut für Validierung!)

---

## Warum Override?

Ohne Override verwendet der Controller die `GenericRepository` Methode, die KEIN `.Include()` hat!

```csharp
// GenericRepository.GetByIdAsync (OHNE Include):
public virtual async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
    => await Set.FindAsync([id], ct);
// ← Author ist null!

// BookRepository.GetByIdAsync (MIT Include):
public override async Task<Book?> GetByIdAsync(int id, CancellationToken ct = default)
{
    return await Set
        .Include(b => b.Author)  // ← Author wird geladen!
        .FirstOrDefaultAsync(b => b.Id == id, ct);
}
```

---

## Mapster Configuration

Wenn du Navigation Properties hast, brauchst du oft auch Mapster-Config:

```csharp
// In BookMappingConfig.cs:
TypeAdapterConfig<Book, GetBookDto>.NewConfig()
    .Map(dest => dest.AuthorName, src => src.Author != null 
        ? $"{src.Author.FirstName} {src.Author.LastName}" 
        : null);
```

Und registrieren in `DependencyInjection.cs`:

```csharp
public static IServiceCollection AddApplication(this IServiceCollection services)
{
    BookMappingConfig.ConfigureBookMappings();  // ← WICHTIG!
    // ...
}
```

---

## Testen

Nach der Implementierung:

```powershell
cd Api
dotnet run
```

Swagger: `http://localhost:5100/swagger`

Test: `GET /api/Books/1`

**Erwartet:**
```json
{
  "authorName": "J.K. Rowling"  // ← Sollte NICHT null sein!
}
```

---

## Weiter mit Hint 3?

Hint 3 zeigt dir den **kompletten BookRepository** Code.

