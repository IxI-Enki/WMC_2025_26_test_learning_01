# 🔄 Workflow: Neue Übung erstellen

## Overview

Dieser Workflow beschreibt den kompletten Prozess vom Konzept bis zur fertigen Übung auf dem `main` Branch.

```ascii
┌────────────────────────────────────────────────────────────────┐
│                    NEW EXERCISE WORKFLOW                       │
├────────────────────────────────────────────────────────────────┤
│                                                                │
│  1. Konzeption → 2. Development → 3. Solution → 4. Student    │
│                                                                │
│  [dev]           [dev]             [solution]    [main]        │
│                                                                │
└────────────────────────────────────────────────────────────────┘
```

---

## Phase 1: Konzeption (auf dev)

### 1.1 Domain festlegen
```powershell
git checkout dev
mkdir docs/exercises/uebung-XX
```

**Entscheidungen:**
- [ ] Domain-Bereich (z.B. Hotel, Car Rental, etc.)
- [ ] 2-3 Entities mit Beziehungen
- [ ] Schwierigkeitsgrad (Level 1-4)
- [ ] Lernziele definieren
- [ ] Besondere Validierungen

**Dokumentieren in:**
`docs/exercises/uebung-XX/concept.md`

### 1.2 Template als Basis
```powershell
# Template-Muster studieren:
code CleanArchitecture_Template/
```

**Referenzen:**
- Factory-Methoden Pattern
- CQRS Structure
- Repository Pattern
- CSV-Seeding Pattern

### 1.3 CSV-Daten vorbereiten
```csv
Property1;Property2;Property3
Value1;Value2;Value3
```

**Requirements:**
- Min. 5-10 Datensätze
- Realistische Daten
- Foreign Keys korrekt
- Keine Duplikate (außer für Tests)

---

## Phase 2: Development (auf dev)

### 2.1 Solution erstellen
```powershell
cd CleanArchitecture_Uebung_XX
dotnet new sln -n [ExerciseName]
```

### 2.2 Projekte hinzufügen
```powershell
dotnet new classlib -n Domain
dotnet new classlib -n Application
dotnet new classlib -n Infrastructure
dotnet new webapi -n Api
dotnet new xunit -n Domain.Tests
dotnet new xunit -n Api.Tests

dotnet sln add **/*.csproj
```

### 2.3 Domain Layer implementieren
```
Domain/
├─ Entities/
│  ├─ Entity1.cs (Factory-Methode)
│  ├─ Entity2.cs
│  └─ Entity3.cs
├─ ValidationSpecifications/
│  ├─ Entity1Specifications.cs
│  └─ ...
├─ Contracts/
│  └─ IEntity1UniquenessChecker.cs
└─ Exceptions/
   └─ (BaseEntity, etc.)
```

**Checklist:**
- [ ] Factory-Methoden mit Validation
- [ ] ValidationSpecifications vollständig
- [ ] Navigation Properties korrekt
- [ ] Interfaces definiert

### 2.4 Application Layer implementieren
```
Application/
├─ Features/
│  ├─ Entity1/
│  │  ├─ Commands/ (Create, Update, Delete)
│  │  └─ Queries/ (GetAll, GetById)
│  └─ ...
├─ Dtos/
└─ Services/
   └─ Entity1UniquenessChecker.cs
```

**Checklist:**
- [ ] Alle Commands + Handlers + Validators
- [ ] Alle Queries + Handlers
- [ ] DTOs definiert
- [ ] UniquenessChecker Services
- [ ] DI Registration

### 2.5 Infrastructure Layer implementieren
```
Infrastructure/
├─ Persistence/
│  ├─ AppDbContext.cs
│  ├─ Repositories/
│  └─ UnitOfWork.cs
└─ Services/
   └─ StartupDataSeeder.cs
```

**Checklist:**
- [ ] AppDbContext mit OnModelCreating
- [ ] Repositories mit .Include()
- [ ] UnitOfWork
- [ ] CSV-Seeder
- [ ] Migrations

### 2.6 API Layer implementieren
```
Api/
├─ Controllers/
│  ├─ Entity1Controller.cs
│  └─ ...
├─ Extensions/
│  └─ ResultExtensions.cs
└─ Program.cs
```

**Checklist:**
- [ ] Controller mit CRUD
- [ ] Result Pattern
- [ ] Swagger Config
- [ ] Exception Middleware

### 2.7 Tests schreiben
```
Domain.Tests/
├─ Entity1Tests.cs
├─ Entity1SpecificationsTests.cs
└─ ...
```

**Checklist:**
- [ ] Factory-Methode Tests
- [ ] Validation Tests
- [ ] Happy Path
- [ ] Error Cases

### 2.8 Build & Test
```powershell
dotnet build
dotnet test
cd Api && dotnet run
# Swagger: http://localhost:5100/swagger
```

**Verify:**
- [ ] Build erfolgreich
- [ ] Tests grün
- [ ] CSV-Seeding funktioniert
- [ ] API erreichbar
- [ ] Alle Endpoints funktionieren

### 2.9 Commit auf dev
```powershell
git add CleanArchitecture_Uebung_XX/
git commit -m "feat: Add Uebung XX - [Exercise Name]

✅ IMPLEMENTIERT:
- Domain: [Entities]
- Application: Full CRUD
- Infrastructure: CSV seeding
- API: All endpoints

🎯 LERNZIELE:
- [Goal 1]
- [Goal 2]

📊 LEVEL: [1-4]"

git push origin dev
```

---

## Phase 3: Solution Branch (solution/uebung-XX)

### 3.1 Solution Branch erstellen
```powershell
git checkout dev
git checkout -b solution/uebung-XX
```

### 3.2 SOLUTION_NOTES.md erstellen
Siehe: `.ai/prompts/create-solution.md`

**Inhalt:**
- Vollständige Code-Beispiele
- Lern-Schwerpunkte
- Häufige Fehler & Lösungen
- Architecture Highlights
- Erfolgskriterien

### 3.3 Quality Check
```powershell
# Checklist:
code .ai/checklists/code-review.md
```

- [ ] Alle Tests grün
- [ ] Code Quality >90%
- [ ] SOLUTION_NOTES vollständig

### 3.4 Commit & Push
```powershell
git add -A
git commit -m "solution: Complete [Exercise Name]

✅ VOLLSTÄNDIG:
- All features implemented
- All tests green
- SOLUTION_NOTES.md (XXX lines)

💡 KEY CONCEPTS:
- [Concept 1]
- [Concept 2]"

git push -u origin solution/uebung-XX
```

---

## Phase 4: Student Version (main)

### 4.1 Main Branch checkout
```powershell
git checkout main
```

### 4.2 Von solution kopieren
```powershell
# Struktur übernehmen:
git checkout solution/uebung-XX -- CleanArchitecture_Uebung_XX/
```

### 4.3 Scaffolding nach Level

#### Level 1 (Maximum Guidance):
```powershell
# 1 Entity komplett lassen (Referenz)
# Rest: NotImplementedException

# Beispiel:
# - Venue.cs komplett
# - Event.cs: throw new NotImplementedException()
# - Ticket.cs: throw new NotImplementedException()
```

**Änderungen:**
- [ ] 1-2 Entities als Beispiel komplett
- [ ] Rest: Factory-Methoden mit `throw new NotImplementedException()`
- [ ] Commands/Handlers: `throw new NotImplementedException()`
- [ ] Validators: Leer oder Beispiel
- [ ] Controller: 1 komplett, Rest TODO

#### Level 2 (Structural Guidance):
```powershell
# Ordnerstruktur behalten
# Klassen leeren

# Beispiel:
# - Entities: Nur Properties, Factory leer
# - Commands: Nur Definition, Handler leer
# - Validators: Leer
```

**Änderungen:**
- [ ] Entities: Nur Gerüste
- [ ] ValidationSpecifications: Leer
- [ ] Commands/Queries: Nur Dateien, leer
- [ ] Handlers: Leer
- [ ] Validators: Leer
- [ ] Controller: Nur Gerüste

#### Level 3 (Conceptual Guidance):
```powershell
# Nur README + Tests lassen
rm -rf CleanArchitecture_Uebung_XX/Domain/*
rm -rf CleanArchitecture_Uebung_XX/Application/*
# etc.
```

**Behalten:**
- [ ] README.md
- [ ] Tests (als Spezifikation)
- [ ] Leere Solution

### 4.4 README für Studenten
Siehe: `.ai/prompts/generate-exercise.md`

**Struktur:**
```markdown
# [Exercise Name]

## Lernziele
- [Goal 1]
- [Goal 2]

## Aufgabenstellung
[Detailliert nach Level]

## Anforderungen
### Domain Layer
- [ ] Task 1
- [ ] Task 2

### Application Layer
- [ ] Task 1

## Testing
[Anleitung]

## Erfolgskriterien
- [ ] All tests green
- [ ] API works
```

### 4.5 Quality Check
```powershell
code .ai/checklists/exercise-quality.md
```

- [ ] Scaffolding korrekt für Level
- [ ] README vollständig
- [ ] Build erfolgreich
- [ ] Keine Lösungen im Code

### 4.6 Commit & Push
```powershell
git add CleanArchitecture_Uebung_XX/
git commit -m "feat: Add Uebung XX for students (Level [X])

🎓 EXERCISE:
- Level [X]: [Guidance Type]
- Entities: [List]
- Learning Goals: [List]

📚 README: [Detail level]
🧪 TESTS: Present
🏗️ SCAFFOLDING: [Description]"

git push origin main
```

---

## Phase 5: Hints erstellen (support/hints)

### 5.1 Hints Branch
```powershell
git checkout support/hints
```

### 5.2 Hint-Struktur
```
hints/uebung-XX/
├─ 01-domain-validations/
│  ├─ hint-1-concept.md
│  ├─ hint-2-structure.md
│  └─ hint-3-complete.md
└─ ...
```

Siehe: `.ai/prompts/create-hints.md`

### 5.3 Commit
```powershell
git add hints/uebung-XX/
git commit -m "docs: Add hints for Uebung XX"
git push origin support/hints
```

---

## Fertig! 🎉

**Erstellt:**
- ✅ dev: Vollständige Implementierung
- ✅ solution/uebung-XX: Lösung + SOLUTION_NOTES.md
- ✅ main: Student Version (Level X)
- ✅ support/hints: Progressive Hilfe

**Nächste Schritte:**
- Übung testen lassen
- Feedback einarbeiten
- Weitere Übungen erstellen

---

**Version:** 1.0  
**Last Updated:** 2025-11-16

