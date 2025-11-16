# 🎓 WMC Test - Übungsvorbereitung 2025/26

Dieses Repository enthält Übungen zur Vorbereitung auf den WMC-Test.

---

## 📂 Struktur

```
WMC_2025_26_test_learning_01/
├── CleanArchitecture_Template/      # ⭐ Vollständiges Referenz-Template
│   ├── Domain/
│   ├── Application/
│   ├── Infrastructure/
│   ├── Api/
│   ├── Domain.Tests/
│   └── Api.Tests/
│
├── CleanArchitecture_Uebung_01/     # 📝 Übung 1: Event Management
│   ├── Domain/                      # Venue, Event, Ticket
│   ├── Application/
│   ├── Infrastructure/
│   ├── Api/
│   ├── Domain.Tests/
│   ├── Api.Tests/
│   └── README.md                    # ← Aufgabenstellung hier!
│
├── CleanArchitecture_Uebung_02/     # 📝 Übung 2: Library Management
│   ├── Domain/                      # Book, Author, Loan
│   ├── Application/
│   ├── Infrastructure/
│   ├── Api/
│   ├── Domain.Tests/
│   ├── Api.Tests/
│   └── README.md                    # ← Aufgabenstellung hier!
│
└── README.md                        # ← Du bist hier
```

---

## 🎯 Übungen

### 📚 Übung 1: Event Management System

**Entitäten:** Venue, Event, Ticket

**Fokus:**
- Domain Validations
- Command Handler implementieren
- Controller-Endpunkte vervollständigen
- Tests zum Laufen bringen

**Aufgabenstellung:** [CleanArchitecture_Uebung_01/README.md](CleanArchitecture_Uebung_01/README.md)

**Schwierigkeit:** ⭐⭐⭐ Mittel

---

### 📚 Übung 2: Library Management System

**Entitäten:** Book, Author, Loan

**Fokus:**
- CQRS Commands/Queries von Grund auf
- Navigation Properties
- Repository-Methoden
- Domain Factory-Methoden

**Aufgabenstellung:** [CleanArchitecture_Uebung_02/README.md](CleanArchitecture_Uebung_02/README.md)

**Schwierigkeit:** ⭐⭐⭐⭐ Mittel-Schwer

---

## 🚀 Quick Start

### 1. Template anschauen (WICHTIG!)

```powershell
cd CleanArchitecture_Template
code .
```

**Das Template ist deine Hauptreferenz!** Alle Patterns, die du brauchst, sind dort vollständig implementiert.

### 2. Übung 1 starten

```powershell
cd CleanArchitecture_Uebung_01
# README.md lesen für genaue Aufgabenstellung
code .
```

### 3. Testen

```powershell
# Domain Tests:
cd Domain.Tests
dotnet test

# API Tests:
cd Api.Tests
dotnet test

# API starten:
cd Api
dotnet run
# → Swagger: http://localhost:5100/swagger
```

---

## 📖 Was lerne ich hier?

### Clean Architecture
- ✅ **Domain Layer:** Entities, Validations, Specifications
- ✅ **Application Layer:** CQRS (Commands & Queries), Handlers, DTOs
- ✅ **Infrastructure Layer:** Repositories, DbContext, Data Seeding
- ✅ **API Layer:** Controllers, Middleware, Result Pattern

### Design Patterns
- ✅ **Factory Methods** (Entity-Erstellung mit Validation)
- ✅ **Repository Pattern** (Generic + Specific)
- ✅ **Unit of Work** (Transaction-Management)
- ✅ **CQRS** (Command Query Responsibility Segregation)
- ✅ **Result Pattern** (Fehlerbehandlung)
- ✅ **Mediator Pattern** (MediatR)

### Validierung (3 Ebenen)
1. **Domain Validation** (Grundregeln - immer gültig)
2. **FluentValidation** (UseCase-spezifisch)
3. **External Validation** (Uniqueness-Checks via Interface)

---

## 🎓 Empfohlene Reihenfolge

1. **Schritt 1:** Template komplett durcharbeiten
   - Verstehe Sensor & Measurement
   - Verstehe die Layer-Struktur
   - Verstehe CQRS & Repository Pattern

2. **Schritt 2:** Übung 1 lösen
   - Event Management System
   - Venue ist schon fertig (Referenz!)
   - Event & Ticket implementieren

3. **Schritt 3:** Übung 2 lösen
   - Library Management System
   - Komplexere Navigation Properties
   - Loan-Entity mit Overlap-Validierung

---

## ⚡ Wichtige Konzepte

### Factory Methods

```csharp
// ✅ RICHTIG (Template-Muster):
public static async Task<Entity> CreateAsync(
    string property1,
    string property2,
    IEntityUniquenessChecker uniquenessChecker,
    CancellationToken ct = default)
{
    // 1. Trim
    var trimmed1 = (property1 ?? string.Empty).Trim();
    
    // 2. Internal Validation
    ValidateEntityInternal(trimmed1, property2);
    
    // 3. External Validation (Uniqueness)
    await ValidateEntityExternal(0, trimmed1, uniquenessChecker, ct);
    
    // 4. Create
    return new Entity { Property1 = trimmed1, Property2 = property2 };
}
```

**Wichtig:** Validation SOFORT in der Factory, nicht nachträglich!

### Navigation Properties

```csharp
// ✅ RICHTIG (Repository):
public override async Task<Book?> GetByIdAsync(int id, CancellationToken ct = default)
{
    return await Set
        .Include(b => b.Author)  // ← Navigation Property laden!
        .FirstOrDefaultAsync(b => b.Id == id, ct);
}
```

**Wichtig:** `.Include()` für Navigation Properties, sonst sind sie null!

### CQRS Pattern

```csharp
// Commands (ändern Daten):
public record CreateEntityCommand(...) : IRequest<Result<GetEntityDto>>;

// Queries (lesen nur Daten):
public record GetAllEntitiesQuery() : IRequest<Result<IReadOnlyCollection<GetEntityDto>>>;
```

**Wichtig:** Strikte Trennung zwischen Commands & Queries!

---

## 🧪 Tests

### Domain Tests

```powershell
cd Domain.Tests
dotnet test
```

Testen:
- Factory-Methoden
- Validierungen
- Domain-Logik

### API Tests (Integration)

```powershell
cd Api.Tests
dotnet test
```

Testen:
- Endpoints
- HTTP Status Codes
- End-to-End Flows

### Swagger Testing

```powershell
cd Api
dotnet run
# → http://localhost:5100/swagger
```

Interaktives Testen:
- Alle Endpoints ausprobieren
- Request/Response sehen
- Status Codes prüfen

---

## 📚 Hilfe & Ressourcen

### Template (OBERSTE AUTORITÄT!)

```
CleanArchitecture_Template/
```

Alle Patterns sind dort **vollständig und korrekt** implementiert!

### README der Übungen

- [Übung 1 README](CleanArchitecture_Uebung_01/README.md)
- [Übung 2 README](CleanArchitecture_Uebung_02/README.md)

### Was wenn ich nicht weiterkomme?

1. **Template konsultieren** (z.B. Sensor für Venue, Measurement für Loan)
2. **README der Übung nochmal lesen**
3. **Tests ansehen** (zeigen die erwartete Funktionalität)
4. **Compiler-Fehler analysieren**
5. **Swagger testen** (sehen was funktioniert/nicht funktioniert)

---

## ⚠️ Häufige Fehler

### ❌ Validation NACH Factory
```csharp
// FALSCH:
var entity = new Entity { ... };
ValidateEntity(entity);  // Zu spät!
```

### ❌ Navigation Property nicht laden
```csharp
// FALSCH:
return await Set.FirstOrDefaultAsync(...);
// → Author ist null!

// RICHTIG:
return await Set
    .Include(b => b.Author)
    .FirstOrDefaultAsync(...);
```

### ❌ Entity statt DTO zurückgeben
```csharp
// FALSCH (Controller):
return Ok(entity);  // Entity nach außen!

// RICHTIG:
return Ok(entity.Adapt<GetEntityDto>());  // DTO!
```

---

## ✅ Erfolgskriterien

**Du bist bereit für den Test, wenn:**

- [ ] Alle Tests in Übung 1 grün sind
- [ ] Alle Tests in Übung 2 grün sind
- [ ] Du Factory-Methoden mit Validation implementieren kannst
- [ ] Du Commands + Handlers + Validators erstellen kannst
- [ ] Du Queries + Handlers erstellen kannst
- [ ] Du Controller-Endpunkte implementieren kannst
- [ ] Du Navigation Properties mit `.Include()` laden kannst
- [ ] Du das Repository Pattern verstehst
- [ ] Du CQRS anwenden kannst
- [ ] Du das Template als Referenz nutzen kannst

---

## 🎯 Tipps für den Test

1. **Template ist deine Bibel!** Immer nachschauen bei Unsicherheit
2. **Tests zuerst laufen lassen** - sie zeigen was fehlt
3. **Eine Entität nach der anderen** - nicht alles auf einmal
4. **Domain zuerst** - dann Application - dann API
5. **Compiler-Fehler sind dein Freund** - sie zeigen Probleme sofort
6. **Swagger zum Testen** - interaktiv und schnell

---

**Viel Erfolg beim Üben und beim Test! 🚀**

**Version:** 1.0  
**Branch:** main (Student Version)  
**Letzte Aktualisierung:** 2025-11-16
