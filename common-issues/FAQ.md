# ❓ Häufig gestellte Fragen (FAQ)

## 🏗️ Build & Setup

### Q: `dotnet build` schlägt fehl mit "CS0246"
**A:** Eine Abhängigkeit fehlt.

```powershell
# Lösung:
dotnet restore
dotnet build
```

### Q: Migrations fehlen / DB wird nicht erstellt
**A:** Migrations noch nicht angewendet.

```powershell
# In Package Manager Console:
Add-Migration Initial
Update-Database

# Oder in Terminal:
dotnet ef migrations add Initial --project Infrastructure --startup-project Api
dotnet ef database update --project Infrastructure --startup-project Api
```

### Q: "No service for type IUnitOfWork"
**A:** Dependency Injection nicht registriert.

**Lösung:**
```csharp
// In Api/Program.cs:
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
```

---

## 📦 Domain Layer

### Q: "throw new NotImplementedException()" - Was tun?
**A:** Das ist deine Aufgabe! 😊

Das `NotImplementedException` markiert Code, den **du** implementieren sollst.

**Wo finde ich Hilfe?**
1. Schaue dir das komplett implementierte Beispiel an (z.B. Venue)
2. Konsultiere das Template
3. Nutze die Hints in diesem Branch

### Q: Factory-Methode vs. Constructor - Wann was?
**A:** **IMMER Factory-Methode** für Domain Entities mit Validation!

```csharp
// ❌ FALSCH:
var book = new Book { ISBN = isbn, Title = title };

// ✅ RICHTIG:
var book = await Book.CreateAsync(isbn, title, author, uc, ct);
```

**Warum?** Factory validiert SOFORT → Keine ungültigen Objekte möglich!

### Q: Wann Internal vs. External Validation?
**A:**

**Internal:** Regeln, die IMMER gelten (Länge, Format, Bereich)  
**External:** Regeln, die DB-Zugriff brauchen (Uniqueness, FK exists)

```csharp
// Internal:
if (name.Length < 2)
    throw new DomainValidationException("Name", "Min. 2 Zeichen");

// External:
if (!await uc.IsUniqueAsync(name))
    throw new DomainValidationException("Name", "Name existiert bereits");
```

---

## 💼 Application Layer

### Q: Command vs. Query - Was ist der Unterschied?
**A:**

**Command:** Ändert Daten (Create, Update, Delete)  
**Query:** Liest nur Daten (GetAll, GetById)

```csharp
// Command:
public record CreateBookCommand(...) : IRequest<Result<GetBookDto>>;

// Query:
public record GetAllBooksQuery() : IRequest<Result<IReadOnlyCollection<GetBookDto>>>;
```

### Q: "Handler for request X is not registered"
**A:** MediatR findet deinen Handler nicht.

**Lösung:**
1. Handler muss `IRequestHandler<TRequest, TResponse>` implementieren
2. MediatR muss registriert sein:
   ```csharp
   services.AddMediatR(cfg => 
       cfg.RegisterServicesFromAssembly(typeof(IUnitOfWork).Assembly));
   ```

### Q: DTO vs. Entity - Wann was zurückgeben?
**A:** **IMMER DTO** in API!

```csharp
// ❌ FALSCH:
return Ok(entity);  // Entity nach außen!

// ✅ RICHTIG:
return Ok(entity.Adapt<GetBookDto>());  // DTO nach außen!
```

**Warum?** Entities sind Domain-intern, DTOs sind für API-Responses.

---

## 💾 Infrastructure Layer

### Q: Navigation Property ist immer null!
**A:** `.Include()` vergessen!

**Lösung:** Siehe Hints → `hints/uebung-02/02-navigation-properties/`

```csharp
// ❌ FALSCH:
return await Set.FirstOrDefaultAsync(b => b.Id == id);

// ✅ RICHTIG:
return await Set
    .Include(b => b.Author)  // ← WICHTIG!
    .FirstOrDefaultAsync(b => b.Id == id);
```

### Q: CSV-Seeding funktioniert nicht
**A:** Mehrere mögliche Ursachen:

1. **CSV-Datei nicht gefunden:**
   ```json
   // In appsettings.json:
   "StartupDataSeeder": {
     "CsvPath": "../library_seed_data.csv"  // Pfad prüfen!
   }
   ```

2. **Foreign Keys falsch:**
   ```csharp
   // Parents ZUERST speichern!
   await uow.Authors.AddAsync(author, ct);
   await uow.SaveChangesAsync(ct);  // ← ID wird generiert!
   
   // Dann Children:
   var book = await Book.CreateAsync(..., author, ...);
   ```

3. **Validation schlägt fehl:**
   ```csharp
   // UniquenessChecker für Seeding:
   internal class SeedDataUniquenessChecker : IBookUniquenessChecker
   {
       Task<bool> IBookUniquenessChecker.IsUniqueAsync(...)
       {
           return Task.FromResult(true);  // Immer true!
       }
   }
   ```

### Q: SingleOrDefaultAsync vs. FirstOrDefaultAsync?
**A:**

**SingleOrDefaultAsync:** Für UNIQUE Constraints (ISBN, Email, etc.)  
→ Wirft Exception bei mehreren Treffern

**FirstOrDefaultAsync:** Für normale Suche  
→ Gibt ersten Treffer zurück

```csharp
// Unique (ISBN):
await Set.SingleOrDefaultAsync(b => b.ISBN == isbn);

// Normal (Title):
await Set.FirstOrDefaultAsync(b => b.Title.Contains(title));
```

---

## 🌐 API Layer

### Q: Swagger zeigt keinen Endpoint
**A:** Controller nicht registriert oder Route falsch.

```csharp
[ApiController]
[Route("api/[controller]")]  // ← Muss da sein!
public class BooksController : ControllerBase
```

### Q: POST gibt immer 400 Bad Request
**A:** Validation schlägt fehl.

**Debug:**
1. Schaue in Swagger → "Response" → Error Messages
2. Prüfe FluentValidation Validator
3. Prüfe Domain Validations

### Q: DELETE gibt 404 obwohl Entity existiert
**A:** ID stimmt nicht oder GetById findet nicht.

```csharp
// Handler prüfen:
var entity = await uow.Books.GetByIdAsync(request.Id, ct);
if (entity == null)
    return Result<bool>.NotFound($"Book with ID {request.Id} not found.");
```

### Q: PUT gibt 400 "Route ID und Command ID stimmen nicht überein"
**A:** IDs in Route und Body sind unterschiedlich.

```csharp
// ✅ RICHTIG:
PUT /api/Books/5
{
  "id": 5,  // ← Muss übereinstimmen!
  "isbn": "...",
  ...
}
```

---

## 🧪 Testing

### Q: Tests schlagen fehl mit "NullReferenceException"
**A:** Mock nicht richtig konfiguriert oder Navigation Property fehlt.

```csharp
// Mock Setup:
_mockUoW.Setup(x => x.Books.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
    .ReturnsAsync(new Book { 
        Id = 1, 
        ISBN = "123",
        Author = new Author { ... }  // ← Navigation Property!
    });
```

### Q: "Expected X but was Y" bei Validation Tests
**A:** Error Message stimmt nicht exakt.

```csharp
// EXAKT übereinstimmen:
"VenueId muss größer als 0 sein."  // ← Punkt am Ende!
```

---

## 🔧 Allgemeine Probleme

### Q: "Cannot convert from X to Y"
**A:** Type Mismatch.

**Häufig:**
```csharp
// ❌ FALSCH:
IRequest<GetBookDto>  // ← Fehlt Result<>!

// ✅ RICHTIG:
IRequest<Result<GetBookDto>>
```

### Q: "Object reference not set to an instance"
**A:** Null Reference.

**Häufige Ursachen:**
1. Navigation Property nicht geladen (`.Include()` vergessen)
2. Author nicht gefunden (null-check fehlt)
3. Dependency Injection nicht registriert

### Q: Performance ist langsam
**A:** N+1 Query Problem.

**Lösung:** `.Include()` für Navigation Properties!

```csharp
// ❌ LANGSAM (N+1 Queries):
var books = await Set.ToListAsync();
// Dann für jedes Book:
var author = await context.Authors.FindAsync(book.AuthorId);

// ✅ SCHNELL (1 Query):
var books = await Set.Include(b => b.Author).ToListAsync();
```

---

## 📚 Wo finde ich Hilfe?

1. **Template:** `CleanArchitecture_Template/` - OBERSTE AUTORITÄT
2. **Hints:** Dieser Branch (`support/hints`)
3. **README:** Jede Übung hat ausführliches README
4. **REPO__STRAT:** Strategie-Dokumentation

---

## 🚨 Notfall-Checklist

Wenn gar nichts geht:

- [ ] `dotnet restore`
- [ ] `dotnet build`
- [ ] Migrations angewendet?
- [ ] DI registriert?
- [ ] Template konsultiert?
- [ ] Hints gelesen?
- [ ] Tests laufen lassen
- [ ] Swagger Error Messages lesen

---

**Version:** 1.0  
**Last Updated:** 2025-11-16  
**Branch:** support/hints

