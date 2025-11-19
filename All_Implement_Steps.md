# Clean Architecture Implementation Steps - Komplettanleitung

> **Hinweis:** Bei 1:n-Beziehungen immer zuerst die "1"-Seite implementieren, dann die "n"-Seite.  
> **IDE-Features nutzen:** Strg+. für "Generate Method", "Implement Interface", "Add using", etc.

---

## Phase 1: Domain Layer (Entitäten & Geschäftslogik)

### 1.1 Entities - "1"-Seite zuerst (z.B. Author)

1. `Domain/Entities/Author.cs` erstellen, von `BaseEntity` erben
2. Private Properties definieren (Id von BaseEntity, eigene Properties mit `private set`)
3. Private parameterloser Konstruktor für EF Core hinzufügen: `private Author() { }`
4. Navigation Property zur "n"-Seite hinzufügen: `public ICollection<Book> Books { get; private set; } = new List<Book>();`
5. Factory-Methode `CreateAsync()` als `public static async Task<Author>` definieren
6. In Factory: ArgumentNullException für Objekt-Parameter werfen
7. In Factory: String-Parameter trimmen: `var trimmed = (param ?? string.Empty).Trim();`
8. In Factory: Interne Validierung aufrufen: `ValidateAuthorProperties(...)`
9. In Factory: Externe Validierung (Uniqueness) aufrufen: `await AuthorSpecifications.ValidateAuthorExternal(...)`
10. In Factory: Neue Instanz mit private Konstruktor zurückgeben
11. Update-Methode `UpdateAsync()` analog zu Factory implementieren (ohne new, Properties direkt setzen)

### 1.2 Entities - "n"-Seite (z.B. Book)

12. `Domain/Entities/Book.cs` erstellen, von `BaseEntity` erben
13. Properties analog zu 1.2, aber: `public int AuthorId { get; private set; }` für Fremdschlüssel
14. Navigation Property zur "1"-Seite: `public Author Author { get; private set; } = null!;`
15. Bei weiterer "n"-Seite (z.B. Loans): `public ICollection<Loan> Loans { get; private set; } = new List<Loan>();`
16. Factory-Methode mit Author-Objekt als Parameter: `CreateAsync(string isbn, Author author, ...)`
17. In Factory: `Book` und `Author` verknüpfen, aber `AuthorId` NICHT manuell setzen (EF erledigt das)
18. Domain-Methoden für Business-Logik hinzufügen (z.B. `DecreaseCopies()`, `IncreaseCopies()`)

### 1.3 Entities - Weitere "n"-Seite (z.B. Loan)

19. `Domain/Entities/Loan.cs` analog zu Book, mit `BookId` und `Book` Navigation Property
20. Spezielle Properties wie `DateTime? ReturnDate` für optionale Werte
21. Factory-Methode mit Book-Objekt: `Create(Book book, ...)`
22. Business-Methoden: `MarkAsReturned(DateTime returnDate)`, `IsOverdue()`

### 1.4 Domain Specifications - "1"-Seite

23. `Domain/Specifications/AuthorSpecifications.cs` als static class erstellen
24. Konstanten definieren (z.B. `MinLength`, `MaxLength`)
25. Einzelne Check-Methoden als `public static DomainValidationResult CheckFirstName(...)` implementieren
26. Pattern: Prüfung -> `? DomainValidationResult.Success(...) : DomainValidationResult.Failure(...)`
27. `ValidateAuthorInternal()` implementieren: Liste von Checks durchlaufen, bei Fehler `DomainValidationException` werfen
28. `ValidateAuthorExternal()` implementieren: Uniqueness-Check mit `IAuthorUniquenessChecker`, bei Duplikat Exception werfen

### 1.5 Domain Specifications - "n"-Seiten

29. `Domain/Specifications/BookSpecifications.cs` analog zu Author
30. Spezielle Checks für ISBN-Format (13 Ziffern), PublicationYear (Bereich), etc.
31. `Domain/Specifications/LoanSpecifications.cs` mit Checks für Dates, BorrowerName, etc.
32. Konstante für Standard-Ausleih-Dauer: `public const int StandardLoanDurationDays = 14;`

### 1.6 Domain Contracts (Interfaces für Uniqueness-Checker)

33. `Domain/Contracts/IAuthorUniquenessChecker.cs`: `Task<bool> IsUniqueAsync(int id, string fullName, CancellationToken ct);`
34. `Domain/Contracts/IBookUniquenessChecker.cs`: `Task<bool> IsUniqueAsync(int id, string isbn, CancellationToken ct);`

---

## Phase 2: Application Layer (Use Cases)

### 2.1 DTOs (Data Transfer Objects)

35. `Application/Dtos/GetAuthorDto.cs` als `public readonly record struct` mit allen lesbaren Properties
36. `Application/Dtos/GetBookDto.cs` mit zusätzlichem `string? AuthorName` für lesbare Anzeige
37. `Application/Dtos/GetLoanDto.cs` mit `string? BookTitle` und `bool IsOverdue`

### 2.2 Application Interfaces

38. `Application/Interfaces/Repositories/IAuthorRepository.cs` von `IGenericRepository<Author>` erben
39. Zusätzliche Query-Methoden definieren: `GetAuthorsWithBooksAsync()`, `GetByFullName()`, `GetByISBNAsync()`
40. `Application/Interfaces/Repositories/IBookRepository.cs` analog mit `GetBooksByAuthorAsync()`, `GetByISBNAsync()`
41. `Application/Interfaces/Repositories/ILoanRepository.cs` mit `GetLoansByBookIdAsync()`, `GetOverdueLoansAsync()`, `GetActiveLoansByBorrowerAsync()`
42. `Application/Interfaces/IUnitOfWork.cs` erweitern: Properties für alle Repositories (`IAuthorRepository Authors { get; }` etc.)

### 2.3 Commands & Command Handlers - Create

43. Folder erstellen: `Application/Features/Authors/Commands/CreateAuthor/`
44. `CreateAuthorCommand.cs`: `public readonly record struct CreateAuthorCommand(...Parameters...) : IRequest<Result<GetAuthorDto>>;`
45. `CreateAuthorCommandValidator.cs`: Von `AbstractValidator<CreateAuthorCommand>` erben, Rules in Konstruktor definieren
46. `CreateAuthorCommandHandler.cs`: `IRequestHandler<CreateAuthorCommand, Result<GetAuthorDto>>` implementieren (Strg+. "Implement Interface")
47. Handler-Konstruktor: `IUnitOfWork uow, IAuthorUniquenessChecker uniquenessChecker` injizieren
48. Handle-Methode: Factory-Methode aufrufen, `uow.Authors.AddAsync()`, `uow.SaveChangesAsync()`, DTO mit Mapster mappen, `Result<T>.Created()` zurückgeben
49. Analog für Book: `CreateBookCommand`, Validator, Handler mit zusätzlichem Laden des Authors über `uow.Authors.GetByIdAsync()`
50. Analog für Loan: `CreateLoanCommand`, Handler lädt Book, ruft `Loan.Create()`, ruft `book.DecreaseCopies()`, updated beide Entities

### 2.4 Commands & Command Handlers - Update

51. `UpdateAuthorCommand.cs` mit `int Id` als ersten Parameter
52. `UpdateAuthorCommandValidator.cs` mit zusätzlicher `Id > 0` Regel
53. `UpdateAuthorCommandHandler.cs`: Entity laden, NotFound prüfen, `entity.UpdateAsync()` aufrufen, `uow.Authors.Update()`, Save
54. Analog für Book: `UpdateBookCommand`, Validator, Handler

### 2.5 Commands & Command Handlers - Delete

55. `DeleteAuthorCommand.cs`: `public readonly record struct DeleteAuthorCommand(int Id) : IRequest<Result<bool>>;`
56. `DeleteAuthorCommandHandler.cs`: Entity laden, NotFound prüfen, `uow.Authors.Remove()`, Save, `Result<bool>.NoContent()` zurückgeben
57. Analog für Book: `DeleteBookCommand`, Handler

### 2.6 Commands - Spezielle Operationen

58. `ReturnLoanCommand.cs`: `public readonly record struct ReturnLoanCommand(int LoanId, DateTime ReturnDate) : IRequest<Result<bool>>;`
59. `ReturnLoanCommandHandler.cs`: Loan laden (mit Book!), `loan.MarkAsReturned()`, `loan.Book.IncreaseCopies()`, beide updaten, Save

### 2.7 Queries & Query Handlers - GetById

60. `Application/Features/Authors/Queries/GetAuthorById/GetAuthorByIdQuery.cs`: `record struct` mit `int Id`, returns `Result<GetAuthorDto>`
61. `GetAuthorByIdQueryHandler.cs`: `IRequestHandler` implementieren, `uow.Authors.GetByIdAsync()`, null-check, DTO mappen
62. Analog für Book und Loan

### 2.8 Queries & Query Handlers - GetAll

63. `GetAllAuthorsQuery.cs`: `record struct` ohne Parameter, returns `Result<IReadOnlyCollection<GetAuthorDto>>`
64. `GetAllAuthorsQueryHandler.cs`: `uow.Authors.GetAllAsync()`, Collection von DTOs mappen
65. Analog für Books und Loans

### 2.9 Queries - Spezielle Abfragen

66. `GetAuthorByIdQuery` mit Validator für Id > 0
67. `GetLoansByBookQuery.cs`: Query mit `int BookId` Parameter
68. `GetLoansByBookQueryHandler.cs`: `uow.Loans.GetLoansByBookIdAsync()` aufrufen, DTOs mappen
69. `GetOverdueLoansQuery.cs`: Query ohne Parameter
70. `GetOverdueLoansQueryHandler.cs`: `uow.Loans.GetOverdueLoansAsync()` aufrufen

### 2.10 Mapster Configuration

71. `Application/Common/Mappings/AuthorMappingConfig.cs`: Static class mit `ConfigureAuthorMappings()` Methode
72. In Methode: `TypeAdapterConfig<Author, GetAuthorDto>.NewConfig()` für einfache Mappings
73. `Application/Common/Mappings/BookMappingConfig.cs`: Config mit `.Map(dest => dest.AuthorName, src => src.Author != null ? $"{src.Author.FirstName} {src.Author.LastName}" : null)`
74. `Application/Common/Mappings/LoanMappingConfig.cs`: Config mit `.Map(dest => dest.IsOverdue, src => src.IsOverdue())`

### 2.11 Application Dependency Injection

75. In `Application/DependencyInjection.cs` `AddApplication()` Methode: MediatR registrieren
76. FluentValidation registrieren: `services.AddValidatorsFromAssembly(typeof(IUnitOfWork).Assembly);`
77. ValidationBehavior registrieren: `services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));`
78. Mapster-Configs aufrufen: `AuthorMappingConfig.ConfigureAuthorMappings();` etc.

---

## Phase 3: Infrastructure Layer (Datenzugriff)

### 3.1 Persistence - AppDbContext

79. `Infrastructure/Persistence/AppDbContext.cs`: DbSet-Properties hinzufügen (`public DbSet<Author> Authors => Set<Author>();`)
80. DbSets für alle Entities: Authors, Books, Loans
81. In `OnModelCreating()`: Fluent-API für Author konfigurieren (MaxLength, Required, Unique Constraints, RowVersion)
82. Fluent-API für Book: ISBN unique, Beziehung zu Author mit `HasMany(a => a.Books).WithOne(b => b.Author).HasForeignKey(b => b.AuthorId)`
83. Fluent-API für Loan: Beziehung zu Book konfigurieren
84. OnDelete-Verhalten konfigurieren (Cascade, Restrict, etc.)

### 3.2 Repositories - Generic Repository

85. `Infrastructure/Persistence/Repositories/GenericRepository.cs` prüfen/anpassen wenn nötig

### 3.3 Repositories - Spezifische Repositories

86. `Infrastructure/Persistence/Repositories/AuthorRepository.cs`: Von `GenericRepository<Author>` und `IAuthorRepository` erben
87. Konstruktor: `public AuthorRepository(AppDbContext ctx) : base(ctx) { }`
88. Spezielle Methoden implementieren: `GetAuthorsWithBooksAsync()` mit `.Include(a => a.Books)`
89. `GetByFullName()`: Filtern mit `.FirstOrDefaultAsync(a => (a.FirstName + " " + a.LastName) == fullName)`
90. `GetByISBNAsync()`: `.Include(a => a.Books).FirstOrDefaultAsync(a => a.Books.Any(b => b.ISBN == isbn))`
91. `Infrastructure/Persistence/Repositories/BookRepository.cs` analog mit `GetBooksByAuthorAsync()`, `GetByISBNAsync()`
92. BookRepository: `GetByIdAsync()` und `GetAllAsync()` overriden für `.Include(b => b.Author)`
93. `Infrastructure/Persistence/Repositories/LoanRepository.cs` mit `GetLoansByBookIdAsync()`, `GetOverdueLoansAsync()`, alle mit `.Include(l => l.Book)`

### 3.4 Unit of Work

94. `Infrastructure/Persistence/UnitOfWork.cs`: Properties für alle Repositories hinzufügen
95. Lazy Initialization Pattern: `private IAuthorRepository? _authors; public IAuthorRepository Authors => _authors ??= new AuthorRepository(_dbContext);`
96. Analog für Books und Loans

### 3.5 Services - Uniqueness Checker

97. `Infrastructure/Services/AuthorUniquenessChecker.cs`: `IAuthorUniquenessChecker` implementieren (Strg+. "Implement Interface")
98. Konstruktor: `AppDbContext` injizieren
99. `IsUniqueAsync()`: Datenbank-Query ob Author mit gleichem Namen (außer eigene Id) existiert
100. `Infrastructure/Services/BookUniquenessChecker.cs` analog für ISBN-Prüfung

### 3.6 StartupDataSeeder

101. `Infrastructure/Services/StartupDataSeeder.cs`: `IHostedService` implementieren
102. Nested class `SeedDataUniquenessChecker` implementiert alle IXxxUniquenessChecker mit explicit interface implementation
103. `StartAsync()`: CSV einlesen, Entities erstellen, in DB einfügen
104. Entities mit Factory-Methoden erstellen: `await Author.CreateAsync(..., seedChecker)`
105. Beziehungen auflösen (z.B. Author zu Book über ID-Mapping)
106. Alle Entities zu `DbContext` hinzufügen, EINMAL `SaveChangesAsync()` aufrufen

### 3.7 StartupDataSeederOptions

107. `Infrastructure/Services/StartupDataSeederOptions.cs`: Class mit `public string CsvPath { get; set; } = "";`

### 3.8 Infrastructure Dependency Injection

108. `Infrastructure/DependencyInjection.cs` `AddInfrastructure()`: DbContext mit SQL Server registrieren
109. Repository-Registrierungen: `services.AddScoped<IAuthorRepository, AuthorRepository>();` etc.
110. UnitOfWork registrieren: `services.AddScoped<IUnitOfWork, UnitOfWork>();`
111. Uniqueness-Checker registrieren: `services.AddScoped<IAuthorUniquenessChecker, AuthorUniquenessChecker>();` etc.
112. StartupDataSeeder registrieren: `services.AddHostedService<StartupDataSeeder>();`
113. StartupDataSeederOptions konfigurieren: `services.Configure<StartupDataSeederOptions>(configuration.GetSection("StartupDataSeeder"));`

---

## Phase 4: API Layer (REST-Schnittstelle)

### 4.1 Result Extensions

114. `Api/Extensions/ResultExtensions.cs` prüfen/erweitern für `.ToActionResult()` Methode

### 4.2 Controllers - "1"-Seite

115. `Api/Controllers/AuthorsController.cs` erstellen: `[ApiController]`, `[Route("api/[controller]")]`
116. Konstruktor: `IMediator mediator` per primary constructor injizieren
117. `[HttpGet]` GetAll: `await mediator.Send(new GetAllAuthorsQuery(), ct)`, `result.ToActionResult(this)`
118. `[HttpGet("{id:int}")]` GetById mit ProducesResponseType-Attributen
119. `[HttpPost]` Create: Command senden, `result.ToActionResult(this, createdAtAction: nameof(GetById), routeValues: new { id = result?.Value?.Id })`
120. `[HttpPut("{id:int}")]` Update: Id-Prüfung gegen Command.Id, dann Command senden
121. `[HttpDelete("{id:int}")]` Delete: Command senden
122. XML-Kommentare für Swagger hinzufügen

### 4.3 Controllers - "n"-Seiten

123. `Api/Controllers/BooksController.cs` analog zu Authors
124. `Api/Controllers/LoansController.cs` mit speziellen Endpoints:
125. `[HttpPost]` CreateLoan
126. `[HttpPut("{id:int}/return")]` ReturnLoan mit `DateTime returnDate` im Body
127. `[HttpGet("book/{bookId:int}")]` GetLoansByBook
128. `[HttpGet("overdue")]` GetOverdueLoans

### 4.4 Program.cs

129. `Api/Program.cs`: Builder erstellen, Services registrieren
130. `builder.Services.AddApplication();` aufrufen
131. `builder.Services.AddInfrastructure(builder.Configuration);` aufrufen
132. Swagger konfigurieren mit XML-Kommentaren
133. Middleware-Pipeline: UseSwagger, UseSwaggerUI, MapControllers

### 4.5 appsettings.json

134. `Api/appsettings.json`: ConnectionString unter "ConnectionStrings" hinzufügen
135. StartupDataSeeder-Konfiguration: `"StartupDataSeeder": { "CsvPath": "Data/library_seed_data.csv" }`

### 4.6 Seed-Daten

136. `Api/Data/library_seed_data.csv` erstellen mit Spalten für Authors und Books (ID-Mapping beachten!)

---

## Phase 5: Datenbank-Migration

137. Terminal öffnen, zu Api-Projekt navigieren
138. `dotnet ef migrations add InitialCreate` ausführen
139. `dotnet ef database update` ausführen
140. Prüfen ob Datenbank erstellt wurde (SQL Server Object Explorer)

---

## Phase 6: Tests (Domain & Integration)

### 6.1 Domain Tests

141. `Domain.Tests/AuthorTests.cs`: FakeUniquenessChecker-Klassen erstellen
142. Tests für Factory-Methode: `CreateAsync_Succeeds_WithValidData`
143. Tests für Validierungen: `CreateAsync_InvalidFirstName_Throws` etc. mit `[Theory]` und `[InlineData]`
144. Tests für Duplikat-Erkennung: `CreateAsync_DuplicateFullName_Throws`
145. `Domain.Tests/BookTests.cs` analog
146. `Domain.Tests/LoanTests.cs` mit Tests für Business-Logik: `MarkAsReturned_SetsReturnDate`, `IsOverdue_ReturnsTrue_WhenOverdue` etc.
147. `Domain.Tests/AuthorSpecificationsTests.cs` für einzelne Check-Methoden
148. Analog für BookSpecifications und LoanSpecifications

### 6.2 API Integration Tests - Setup

149. `Api.Tests/Utilities/TestWebApplicationFactory.cs`: `WebApplicationFactory<Program>` erben
150. `ConfigureWebHost()`: InMemory-Database konfigurieren, echte SQL Server überschreiben
151. Repositories manuell registrieren (wie in Infrastructure.DI, aber mit InMemory-DB)
152. `db.Database.EnsureCreated()` aufrufen

### 6.3 API Integration Tests - Endpoints

153. `Api.Tests/Books/BooksEndpointTests.cs`: `IClassFixture<TestWebApplicationFactory<Program>>` implementieren
154. `HttpClient _client` im Konstruktor von Factory holen
155. Helper-Methode `GetFirstAuthorId()` um seeded Authors zu finden (über GET /api/authors)
156. Test: `GetAll_ReturnsOk_WithBooks` - einfacher GET-Test
157. Test: `Create_ReturnsCreated_WithValidData` - POST mit Command, Assert auf StatusCode.Created
158. Test: `Create_ReturnsBadRequest_WithInvalidISBN` - Validierungs-Test
159. Test: `Create_ReturnsNotFound_WithNonExistentAuthor` - Entity-Not-Found Test
160. Test: `GetById_ReturnsOk_WhenBookExists` - Erst erstellen, dann abrufen
161. Test: `Update_ReturnsOk_WithValidData` - PUT-Test
162. Test: `Update_ReturnsBadRequest_WhenIdMismatch` - Id-Prüfung testen
163. Test: `Delete_ReturnsNoContent_WhenBookExists` - DELETE-Test
164. Test: `Create_ReturnsConflict_WithDuplicateISBN` - Uniqueness-Test
165. `Api.Tests/Loans/LoansEndpointTests.cs` mit allen Loan-Endpunkt-Tests
166. Test: `CreateLoan_DecreasesAvailableCopies` - Business-Logik testen (GET Book vor/nach Loan)
167. Test: `ReturnLoan_IncreasesAvailableCopies` - Business-Logik testen
168. Test: `GetOverdueLoans_ReturnsOk_WithOverdueLoans` - Loan mit altem Datum erstellen, abrufen
169. Test: `GetLoansByBook_ReturnsOk_WithLoans` - Mehrere Loans für ein Buch erstellen, alle abrufen

---

## Phase 7: Finale Prüfungen

170. Alle Warnings beheben (Strg+. nutzen für Quick Fixes)
171. Solution builden: `dotnet build` - muss erfolgreich sein
172. Alle Tests ausführen: `dotnet test` - alle müssen grün sein
173. API starten: `dotnet run --project Api`
174. Swagger aufrufen: `https://localhost:xxxx/swagger`
175. Alle Endpoints manuell testen: Create Author → Create Book → Create Loan → Get All → GetById → Update → Delete
176. Validierungen testen: Leere Strings, ungültige ISBNs, nicht existierende FKs
177. Business-Logik testen: AvailableCopies ändern sich bei Loan/Return
178. Overdue-Loans abrufen (falls Testdaten vorhanden)
179. Code-Review: Namenskonventionen, XML-Kommentare, async/await korrekt
180. Git: Branch erstellen, alle Änderungen committen mit aussagekräftigen Messages

---

## Bonus: Optimierungen & Best Practices

181. Globale Error-Handler-Middleware für bessere Fehlerbehandlung
182. Logging mit ILogger in Handlers
183. Rate Limiting für API-Endpoints
184. CORS-Policy konfigurieren
185. Health Checks hinzufügen
186. API-Versionierung einführen
187. Pagination für GetAll-Queries (Skip/Take)
188. Soft-Delete statt Hard-Delete implementieren
189. Caching-Layer für häufige Queries
190. Background-Jobs für zeitgesteuerte Operationen (z.B. Überfälligkeits-Emails)

---

## Hilfreiche IDE-Shortcuts (Visual Studio)

- **Strg+.** - Quick Actions (Generate Method, Implement Interface, Add using, etc.)
- **Strg+R, Ctrl+R** - Rename (Refactoring)
- **Strg+K, Strg+D** - Format Document
- **Strg+K, Strg+C** - Comment Selection
- **Strg+K, Strg+U** - Uncomment Selection
- **F12** - Go to Definition
- **Strg+F12** - Go to Implementation
- **Shift+F12** - Find All References
- **Strg+T** - Go to All (Suche nach Typen/Files)
- **Strg+,** - Navigate To (Suche innerhalb Solution)

---

## Typische Fehlerquellen & Lösungen

- **CS0246 (Type not found)**: `using` Statement fehlt → Strg+. "Add using"
- **CS0535 (Interface not implemented)**: Interface nicht vollständig implementiert → Strg+. "Implement Interface"
- **EF Core Migration Fehler**: Falsche ConnectionString oder SQL Server nicht erreichbar
- **Null Reference**: Navigation Properties nicht geladen → `.Include()` verwenden
- **Validation Fehler**: FluentValidation Rules prüfen, DTO-Mapping korrekt?
- **Circular Reference**: DTOs statt Entities in API zurückgeben
- **Foreign Key Constraint**: Entities in korrekter Reihenfolge speichern (1-Seite vor n-Seite)
- **Concurrency Exception**: RowVersion in UpdateAsync prüfen, OptimisticConcurrency in EF konfigurieren

---

**Stand:** 2025-11-19  
**Template-Basis:** CleanArchitecture_Template (Sensor/Measurement)  
**Ziel-Implementierung:** Library Management (Author/Book/Loan)

