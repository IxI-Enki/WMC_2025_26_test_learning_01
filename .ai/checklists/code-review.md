# 🔍 Code Review Checklist

## Vor dem Merge zur solution/* Branch

### 🎯 **Architecture Compliance**
- [ ] Clean Architecture Layers strikt getrennt
- [ ] Keine Domain-Abhängigkeit von Application
- [ ] Keine Application-Abhängigkeit von Infrastructure
- [ ] Keine Infrastructure-Abhängigkeit von API
- [ ] Dependency Injection korrekt verwendet

### 🏗️ **Domain Layer Review**
- [ ] Factory-Methoden sind `static async Task<Entity>`
- [ ] Validation SOFORT in Factory (nicht nachträglich)
- [ ] Internal vor External Validation
- [ ] DomainValidationException bei Fehlern
- [ ] Keine Anemic Domain Models
- [ ] Rich Domain Logic vorhanden
- [ ] Navigation Properties korrekt definiert
- [ ] Keine Business Logic in Setters

### 📦 **Application Layer Review**
- [ ] Commands ändern Daten
- [ ] Queries lesen nur Daten
- [ ] Strikte CQRS-Trennung
- [ ] Handler haben Single Responsibility
- [ ] FluentValidation für UseCase-Regeln
- [ ] Keine Domain-Validations in Validators
- [ ] DTOs statt Entities zurückgeben
- [ ] Mapster richtig konfiguriert

### 💾 **Infrastructure Layer Review**
- [ ] Repository-Pattern korrekt
- [ ] .Include() für Navigation Properties
- [ ] AsNoTracking() bei Read-Only
- [ ] SingleOrDefaultAsync für Unique
- [ ] FirstOrDefaultAsync sonst
- [ ] Transactions via UnitOfWork
- [ ] CSV-Seeding robust
- [ ] Parent-Entities SOFORT gespeichert

### 🌐 **API Layer Review**
- [ ] Controller nur Koordination
- [ ] MediatR für Business Logic
- [ ] Result Pattern verwendet
- [ ] HTTP Status Codes korrekt
- [ ] 201 Created mit Location Header
- [ ] 204 No Content bei DELETE
- [ ] 404 Not Found bei fehlenden Entities
- [ ] 409 Conflict bei Uniqueness-Verletzung
- [ ] 400 Bad Request bei Validation
- [ ] ProducesResponseType vollständig

### 🧪 **Testing Review**
- [ ] Domain Tests vorhanden
- [ ] Validation Tests abgedeckt
- [ ] Factory-Methode Tests
- [ ] Happy Path getestet
- [ ] Error Cases getestet
- [ ] Edge Cases getestet
- [ ] Tests sind deterministisch
- [ ] Keine Test-Abhängigkeiten

### 📝 **Code Quality**
- [ ] DRY (Don't Repeat Yourself)
- [ ] KISS (Keep It Simple, Stupid)
- [ ] YAGNI (You Aren't Gonna Need It)
- [ ] Single Responsibility Principle
- [ ] Open/Closed Principle
- [ ] Liskov Substitution Principle
- [ ] Interface Segregation Principle
- [ ] Dependency Inversion Principle

### 🔒 **Security Review**
- [ ] Keine SQL-Injection möglich
- [ ] Input Validation vorhanden
- [ ] Keine sensiblen Daten geloggt
- [ ] Keine Passwörter im Code
- [ ] Keine Secrets committed

### ⚡ **Performance Review**
- [ ] Eager Loading wo nötig (.Include())
- [ ] Lazy Loading vermieden
- [ ] N+1 Queries vermieden
- [ ] AsNoTracking() bei Read-Only
- [ ] AddRangeAsync für Bulk-Inserts
- [ ] Paging bei großen Datasets (optional)

### 📚 **Documentation Review**
- [ ] XML-Kommentare auf public Members
- [ ] README vollständig
- [ ] SOLUTION_NOTES.md vorhanden
- [ ] Code ist selbst-dokumentierend
- [ ] Komplexe Logik kommentiert
- [ ] TODOs entfernt

### 🔧 **Maintenance Review**
- [ ] Naming Conventions befolgt
- [ ] Code gut lesbar
- [ ] Keine Magic Numbers
- [ ] Constants definiert
- [ ] Keine Code-Duplikation
- [ ] Refactoring-Bedarf markiert

---

## Common Anti-Patterns (vermeiden!)

### ❌ **Validation nach Factory**
```csharp
// FALSCH:
var entity = new Entity();
ValidateEntity(entity);
```

### ❌ **Entities in API zurückgeben**
```csharp
// FALSCH:
return Ok(entity);
```

### ❌ **Business Logic im Controller**
```csharp
// FALSCH:
if (book.AvailableCopies <= 0)
    return BadRequest("No copies available");
```

### ❌ **Navigation Property nicht laden**
```csharp
// FALSCH:
return await Set.FirstOrDefaultAsync(...);
// Author ist null!
```

### ❌ **Foreign Key manuell setzen**
```csharp
// FALSCH:
return new Book
{
    AuthorId = author.Id  // Kann 0 sein!
};
```

---

## Review Score

| Kategorie | Gewichtung | Check |
|-----------|------------|-------|
| Architecture | 20% | ___ / 5 |
| Domain Layer | 20% | ___ / 8 |
| Application Layer | 15% | ___ / 7 |
| Infrastructure | 15% | ___ / 8 |
| API Layer | 10% | ___ / 9 |
| Testing | 10% | ___ / 8 |
| Code Quality | 10% | ___ / 8 |

**Gesamt:** ____%

**✅ >90%:** Bereit für Merge  
**⚠️ 75-90%:** Kleinere Anpassungen  
**❌ <75%:** Überarbeitung nötig

---

**Version:** 1.0  
**Last Updated:** 2025-11-16

