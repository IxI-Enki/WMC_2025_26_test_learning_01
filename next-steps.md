# 🎯 WMC Übung 2 - Implementierungs-Roadmap

## 📊 Entitäten-Abhängigkeiten (wie im Template)

```
┌─────────┐
│ Author  │ ◄─── Keine Abhängigkeiten (standalone)
└────┬────┘
     │
     │ (FK: AuthorId)
     │
     ▼
┌─────────┐
│  Book   │ ◄─── Braucht Author
└────┬────┘
     │
     │ (FK: BookId)
     │
     ▼
┌─────────┐
│  Loan   │ ◄─── Braucht Book
└─────────┘
```

**Vergleich mit Template:**
- Template: `Sensor` (standalone) → `Measurement` (FK: SensorId)
- Übung 2: `Author` (standalone) → `Book` (FK: AuthorId) → `Loan` (FK: BookId)

---

## 🎓 Implementierungs-Strategie

### ⚡ **Warum diese Reihenfolge?**

1. **Author zuerst** → Einfachste Entität, keine FK-Abhängigkeiten
2. **Book danach** → Braucht Author-Objekte für Tests und Queries
3. **Loan zuletzt** → Braucht Book-Objekte (und indirekt Authors)

### 🏗️ **Pro Entität: Layer-by-Layer Ansatz**

Für jede Entität (Author → Book → Loan) diese Schritte durchlaufen:

1. ✅ **Domain** → Validationen & Factory-Methoden
2. ✅ **Application** → Commands, Queries, Handlers, Validators
3. ✅ **Infrastructure** → Repository-Methoden (bereits fertig laut Kollegin!)
4. ✅ **API** → Controller-Endpunkte
5. ✅ **Tests** → Domain-Tests & API-Tests ausführen

---

## 📋 Phase 1: Author (Standalone - Einfachste Entität)

### ✅ Was bereits funktioniert:
- ✅ `Author.Create()` ist bereits implementiert (Domain/Entities/Author.cs)
- ✅ `AuthorSpecifications` sind vorhanden (aber mit NotImplementedException)
- ✅ `AuthorRepository` ist fertig (Infrastructure)

### 🔨 Was zu tun ist:

#### 1️⃣ Domain-Ebene
```
□ Domain/Specifications/AuthorSpecifications.cs
  - CheckFirstName() implementieren
  - CheckLastName() implementieren  
  - CheckDateOfBirth() implementieren
```

**Referenz:** Template `SensorSpecifications.cs`

---

#### 2️⃣ Application-Ebene

**WICHTIG:** Im Template hat `Sensor` **KEINE** Authors/Books-ähnliche Struktur.
Für Authors können wir entweder:
- **Option A:** Nur lesende Queries (GET ALL, GET BY ID)
- **Option B:** Full CRUD wie bei Books

**Empfehlung:** Start mit **Option A** (nur Queries), da:
- Authors werden via DataSeeder angelegt
- Fokus liegt auf Books (laut Übungsstellung)

```
□ Application/Dtos/GetAuthorDto.cs (bereits vorhanden)
  
□ Application/Features/Authors/Queries/GetAllAuthors/
  - GetAllAuthorsQuery.cs
  - GetAllAuthorsQueryHandler.cs
  
□ Application/Features/Authors/Queries/GetAuthorById/
  - GetAuthorByIdQuery.cs
  - GetAuthorByIdQueryHandler.cs
```

**Referenz:** Template `GetAllSensorsQuery.cs` & `GetAllSensorsQueryHandler.cs`

---

#### 3️⃣ API-Ebene
```
□ Api/Controllers/AuthorsController.cs (erstellen!)
  - [HttpGet] GetAll()
  - [HttpGet("{id:int}")] GetById()
```

**Referenz:** Template `SensorsController.cs` (Zeilen 25-45)

---

#### 4️⃣ Tests
```
□ Domain.Tests ausführen → AuthorSpecifications testen
□ Api.Tests für Authors erstellen (optional)
□ Via Swagger testen:
  - GET /api/authors → sollte 5 Autoren zurückgeben (aus DataSeeder)
  - GET /api/authors/1 → sollte J.K. Rowling zurückgeben
```

---

## 📋 Phase 2: Book (Hängt von Author ab)

### ✅ Was bereits funktioniert:
- ✅ `Book.CreateAsync()` ist implementiert
- ✅ `Book.DecreaseCopies()` & `IncreaseCopies()` sind fertig
- ✅ `BookRepository` ist fertig (Infrastructure)
- ⚠️ `GetAllBooksQueryHandler` ist vorhanden, aber wirft NotImplementedException

### 🔨 Was zu tun ist:

#### 1️⃣ Domain-Ebene
```
□ Domain/Specifications/BookSpecifications.cs
  - CheckISBN() implementieren (13 Zeichen, nur Ziffern)
  - CheckTitle() implementieren (min. 1 Zeichen)
  - CheckPublicationYear() implementieren (1450 - heute)
  - CheckAvailableCopies() implementieren (≥ 0)

□ Domain/Entities/Book.cs
  - UpdateAsync() implementieren (analog zu Sensor.UpdateAsync)
```

**Referenz:** Template `SensorSpecifications.cs` & `Sensor.cs` (UpdateAsync Zeile 59-71)

---

#### 2️⃣ Application-Ebene

```
✅ Application/Dtos/GetBookDto.cs (bereits vorhanden)

□ Application/Features/Books/Queries/GetAllBooks/
  ⚠️ GetAllBooksQueryHandler.cs → NotImplementedException entfernen!
     - await uow.Books.GetAllAsync() aufrufen
     - .Include(b => b.Author) für Navigation Property
     - Zu GetBookDto mappen
     - Result.Success() zurückgeben

□ Application/Features/Books/Queries/GetBookById/
  - GetBookByIdQueryHandler.cs erstellen
  
□ Application/Features/Books/Commands/CreateBook/
  ⚠️ CreateBookCommandHandler.cs → NotImplementedException entfernen!
     - Author aus DB laden (uow.Authors.GetByIdAsync)
     - Book.CreateAsync() aufrufen
     - uow.Books.AddAsync()
     - uow.SaveChangesAsync()
  ⚠️ CreateBookCommandValidator.cs → FluentValidation implementieren
  
□ Application/Features/Books/Commands/DeleteBook/
  - DeleteBookCommandHandler.cs erstellen
  
□ Application/Features/Books/Commands/UpdateBook/
  - Folder erstellen + Command, Handler, Validator
```

**Referenz:** 
- Template `GetAllSensorsQueryHandler.cs` für Queries
- Template `CreateSensorCommandHandler.cs` für Commands (Zeile 17-33)

---

#### 3️⃣ API-Ebene
```
□ Api/Controllers/BooksController.cs
  ⚠️ GetAll() → NotImplementedException entfernen
  - GetById() implementieren
  - Create() implementieren
  - Update() implementieren (neu!)
  - Delete() implementieren
```

**Referenz:** Template `SensorsController.cs` (volle Struktur)

---

#### 4️⃣ Tests
```
□ Domain.Tests ausführen
  - BookTests.cs (CreateAsync, UpdateAsync)
  - BookSpecifications prüfen
  
□ Api.Tests ausführen
  - BooksEndpointTests.cs
  
□ Via Swagger testen:
  - POST /api/books → Neues Buch anlegen
  - GET /api/books → Liste mit Books (inkl. Author-Infos!)
  - GET /api/books/1 → Einzelnes Buch
  - PUT /api/books/1 → Buch aktualisieren
  - DELETE /api/books/1 → Buch löschen
```

---

## 📋 Phase 3: Loan (Hängt von Book ab)

### ✅ Was bereits funktioniert:
- ✅ `Loan.MarkAsReturned()` ist fertig
- ✅ `Loan.IsOverdue()` ist fertig
- ✅ `LoanRepository` ist fertig (Infrastructure)

### 🔨 Was zu tun ist:

#### 1️⃣ Domain-Ebene
```
□ Domain/Specifications/LoanSpecifications.cs
  - CheckBorrowerName() implementieren
  - CheckLoanDate() implementieren
  - CheckDueDate() implementieren
  - ValidateLoanInternal() implementieren (wird von Loan.Create aufgerufen)

□ Domain/Entities/Loan.cs
  - Create() implementieren
    • ArgumentNullException.ThrowIfNull(book)
    • borrowerName trimmen
    • LoanSpecifications.ValidateLoanInternal aufrufen
    • DueDate = loanDate.AddDays(14)
    • book.DecreaseCopies() aufrufen
    • Return new Loan { ... }
```

**Referenz:** Template `Measurement.Create()` (Zeile 35-45)

---

#### 2️⃣ Application-Ebene

```
□ Application/Dtos/GetLoanDto.cs erstellen
  - Id, BookId, BookTitle, AuthorName
  - BorrowerName, LoanDate, DueDate, ReturnDate
  - IsOverdue (bool)

□ Application/Features/Loans/Commands/CreateLoan/
  - CreateLoanCommand.cs
  - CreateLoanCommandHandler.cs
    • Book aus DB laden (uow.Books.GetByIdAsync)
    • Loan.Create() aufrufen
    • uow.Loans.AddAsync()
    • uow.SaveChangesAsync()
  - CreateLoanCommandValidator.cs (FluentValidation)
  
□ Application/Features/Loans/Commands/ReturnLoan/
  - ReturnLoanCommand.cs (nur LoanId)
  - ReturnLoanCommandHandler.cs
    • Loan aus DB laden
    • loan.MarkAsReturned(DateTime.Now)
    • loan.Book.IncreaseCopies() aufrufen
    • uow.SaveChangesAsync()

□ Application/Features/Loans/Queries/GetLoansByBook/
  - GetLoansByBookQuery.cs (BookId als Parameter)
  - GetLoansByBookQueryHandler.cs
  
□ Application/Features/Loans/Queries/GetOverdueLoans/
  - GetOverdueLoansQuery.cs
  - GetOverdueLoansQueryHandler.cs
    • Repository-Methode nutzen (GetOverdueLoansAsync)
```

**Referenz:** Commands analog zu `CreateSensorCommand`

---

#### 3️⃣ Infrastructure-Ebene (Optional - spezielle Queries)

**Laut Kollegin:** "Repository-Methoden für spezielle Abfragen müssen wir hinzufügen"

```
□ Infrastructure/Persistence/Repositories/LoanRepository.cs
  - GetLoansByBookIdAsync(int bookId) hinzufügen (falls nicht vorhanden)
  - GetOverdueLoansAsync() hinzufügen
    • Where(l => l.ReturnDate == null && l.DueDate < DateTime.Now)
    • Include(l => l.Book).ThenInclude(b => b.Author)
```

**Referenz:** Template `SensorRepository.cs` (spezielle Queries)

---

#### 4️⃣ API-Ebene
```
□ Api/Controllers/LoansController.cs
  - [HttpPost] CreateLoan()
  - [HttpPost("{id:int}/return")] ReturnLoan()
  - [HttpGet("book/{bookId:int}")] GetLoansByBook()
  - [HttpGet("overdue")] GetOverdueLoans()
```

---

#### 5️⃣ Tests
```
□ Domain.Tests ausführen
  - LoanSpecificationsTests.cs
  - Loan.Create() testen
  
□ Via Swagger testen:
  - POST /api/loans → Buch ausleihen (AvailableCopies sollte -1 werden)
  - GET /api/loans/overdue → Überfällige Ausleihen
  - POST /api/loans/1/return → Buch zurückgeben (AvailableCopies sollte +1 werden)
```

---

## 🔧 Dependency Injection Checklist

**Laut Kollegin:** "Man muss bei der Dependency Injection den Service registrieren"

```
□ Application/DependencyInjection.cs
  - IBookUniquenessChecker registrieren (aktuell auskommentiert!)
    • services.AddScoped<IBookUniquenessChecker, BookUniquenessChecker>();
```

**Referenz:** Template `Application/DependencyInjection.cs`

---

## 📝 Allgemeine Tipps (aus Template gelernt)

### 🎯 Domain-Validierungen
- **Immer** `DomainValidationResult` zurückgeben (nie direkt Exception werfen)
- **Factory-Methoden** sammeln alle Validierungen und werfen dann `DomainValidationException`
- **ArgumentNullException** für null-Checks bei Objekten (z.B. `author`, `book`)

### 🎯 Application-Handler
- **Queries:** Daten aus UoW holen, zu DTOs mappen, `Result.Success()` zurückgeben
- **Commands:** Entität erstellen/updaten, via UoW speichern, DTO zurückgeben
- **Include:** Navigation Properties mit `.Include()` laden

### 🎯 Controller
- **Nie** Business-Logik im Controller
- **Immer** `result.ToActionResult(this)` für konsistente Responses
- **CreatedAtAction** für POST-Requests mit Route zu GetById

### 🎯 Tests
- **Domain-Tests:** Factory-Methoden & Validierungen testen
- **API-Tests:** InMemory-DB nutzen (bereits konfiguriert in TestWebApplicationFactory)

---

## 📚 Wichtige Referenzen im Template

| Was zu implementieren | Template-Referenz                     | Zeilen    |
| --------------------- | ------------------------------------- | --------- |
| Domain Factory        | `Sensor.CreateAsync()`                | 40-55     |
| Domain Update         | `Sensor.UpdateAsync()`                | 59-71     |
| Domain Validierung    | `SensorSpecifications.cs`             | Alle      |
| QueryHandler          | `GetAllSensorsQueryHandler.cs`        | 12-19     |
| CommandHandler        | `CreateSensorCommandHandler.cs`       | 17-33     |
| CommandValidator      | `CreateSensorCommandValidator.cs`     | Alle      |
| Controller GET        | `SensorsController.GetAll()`          | 27-32     |
| Controller POST       | `SensorsController.Create()`          | 71-77     |
| Controller PUT        | `SensorsController.Update()`          | 90-102    |
| Controller DELETE     | `SensorsController.Delete()`          | 110-114   |
| Repository Custom     | `SensorRepository.GetByLocationAsync` | -         |
| Uniqueness Checker    | `SensorUniquenessChecker.cs`          | Alle      |
| Domain Tests          | `SensorTests.cs`                      | Alle      |
| API Tests             | `SensorsEndpointTests.cs`             | Alle      |
| Result Pattern        | `Result.cs` & `ResultExtensions.cs`   | Alle      |
| Mapster Config        | Handler: `.Adapt<DTO>()`              | Inline    |

---

## ⚡ Quick Start

```bash
# 1. Phase 1 starten - Authors
# 2. Tests laufen lassen
dotnet test CleanArchitecture_Uebung_02/Domain.Tests
dotnet test CleanArchitecture_Uebung_02/Api.Tests

# 3. API starten und via Swagger testen
cd CleanArchitecture_Uebung_02/Api
dotnet run

# 4. Swagger öffnen: https://localhost:7085/swagger
```

---

## 🎯 Erfolgskriterien

✅ **Phase 1 (Authors):** GET /api/authors liefert 5 Autoren aus DataSeeder  
✅ **Phase 2 (Books):** POST /api/books erstellt ein Buch, GET zeigt es mit Author-Infos  
✅ **Phase 3 (Loans):** POST /api/loans leiht ein Buch aus (AvailableCopies -1)  
✅ **Alle Tests:** Grün in Domain.Tests und Api.Tests  
✅ **DataSeeder:** Läuft beim Start automatisch und befüllt Authors

---

**Viel Erfolg! 🤓🤜🏻🤛🏻🤖**

*Erstellt am: 2025-11-15*  
*Basierend auf: CleanArchitecture_Template & Kollegin-Notizen*
