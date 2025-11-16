# ✅ Exercise Quality Checklist

## Vor dem Commit auf main Branch

### 📋 **Domain Layer**
- [ ] Alle Entities haben Factory-Methoden
- [ ] Factory-Methoden haben Validation (Internal + External)
- [ ] Navigation Properties korrekt (`= null!` für Required)
- [ ] Collections initialisiert (`= new List<>()`)
- [ ] ValidationSpecifications vollständig
- [ ] Alle DomainValidationResult checks implementiert
- [ ] IUniquenessChecker Interfaces definiert
- [ ] Keine public Setters ohne Validation

### 📋 **Application Layer**
- [ ] Commands für Create, Update, Delete
- [ ] CommandHandlers implementiert/TODO
- [ ] CommandValidators (FluentValidation)
- [ ] Queries für GetAll, GetById
- [ ] QueryHandlers implementiert/TODO
- [ ] DTOs definiert (Get[Entity]Dto)
- [ ] UniquenessChecker Services implementiert/TODO
- [ ] DependencyInjection registriert
- [ ] Mapster Config (falls Navigation Properties)

### 📋 **Infrastructure Layer**
- [ ] AppDbContext mit OnModelCreating
- [ ] Unique Indexes definiert
- [ ] RowVersion für Concurrency
- [ ] Generic Repository vorhanden
- [ ] Spezifische Repositories mit .Include()
- [ ] UnitOfWork vollständig
- [ ] StartupDataSeeder implementiert
- [ ] CSV-Daten vorhanden und realistisch
- [ ] CSV-Format korrekt (min. 5-10 Datensätze)

### 📋 **API Layer**
- [ ] Controller für jede Entity
- [ ] Alle CRUD-Actions
- [ ] ProducesResponseType Attributes
- [ ] Result Pattern verwendet
- [ ] CreatedAtAction für POST
- [ ] ID-Check bei PUT
- [ ] XML-Dokumentation Kommentare

### 📋 **Tests**
- [ ] Domain.Tests Projekt vorhanden
- [ ] Tests für ValidationSpecifications
- [ ] Tests für Factory-Methoden
- [ ] Tests kompilieren (dürfen fehlschlagen bei NotImpl)
- [ ] Api.Tests Projekt (optional)

### 📋 **Dokumentation**
- [ ] README.md vorhanden
- [ ] Lernziele beschrieben
- [ ] Architektur-Übersicht
- [ ] Aufgaben klar formuliert
- [ ] Schritt-für-Schritt für Level 1
- [ ] Testing-Anleitung
- [ ] Erfolgskriterien definiert

### 📋 **Build & Run**
- [ ] `dotnet build` erfolgreich
- [ ] Keine Compiler-Warnings
- [ ] Migrations vorhanden
- [ ] Database wird erstellt
- [ ] CSV-Seeding funktioniert
- [ ] Swagger UI läuft
- [ ] API erreichbar (http://localhost:5100)

### 📋 **Scaffolding (nach Level)**

#### Level 1:
- [ ] 1-2 Entities KOMPLETT implementiert
- [ ] Rest: `throw new NotImplementedException()`
- [ ] Sehr ausführliches README
- [ ] Alle Tests vorhanden

#### Level 2:
- [ ] Ordnerstruktur komplett
- [ ] Klassen vorhanden (nur Gerüste)
- [ ] Commands/Queries LEER
- [ ] README mittel-detailliert

#### Level 3:
- [ ] Nur README + Tests
- [ ] Leere Solution
- [ ] Minimal README

### 📋 **Quality Gates**
- [ ] Template-Patterns befolgt
- [ ] Naming Conventions einheitlich
- [ ] Keine Rechtschreibfehler
- [ ] Code gut lesbar
- [ ] Kommentare hilfreich
- [ ] Keine Code-Duplikation
- [ ] DRY-Prinzip befolgt

### 📋 **Git**
- [ ] Commit Message aussagekräftig
- [ ] Keine Debug-Dateien committed
- [ ] .gitignore korrekt
- [ ] Keine sensiblen Daten
- [ ] Branch-Name korrekt

---

## Score System

**✅ Alle Pflicht-Items:** Exercise bereit für main  
**⚠️ 1-3 Fehler:** Nacharbeit nötig  
**❌ >3 Fehler:** Überarbeitung erforderlich

---

**Version:** 1.0  
**Last Updated:** 2025-11-16

