# 🏗️ WMC Clean Architecture - Repository Branch Strategy

## 📊 Repository Übersicht

```ascii
╔════════════════════════════════════════════════════════════════════════════════╗
║                    WMC Clean Architecture Learning Repository                  ║
║                         github.com/your-org/wmc-clean-arch                     ║
╚════════════════════════════════════════════════════════════════════════════════╝
```

---

## 🌳 Branch-Strategie (Vollständige Übersicht)

```ascii
                                              ┌────────────────┐
                                              │   RemoteRepo   │
                                              │     GitHub     │
                                              └────────┬───────┘
                  ┌────────────────────────────────────┼──────────────────────────────┐
                  │                                    │                              │
   ╔══════════════▼═══════════════╗   ╔════════════════▼═════════════════╗   ╔════════▼════════╗
   ║         main / master        ║   ║           ai-workspace           ║   ║       dev       ║
   ║         (Production)         ║   ║            (AI Agent)            ║   ║  (Development)  ║
   ╚══════════════╤═══════════════╝   ╚════════════════╤═════════════════╝   ╚════════╤════════╝
                  │                                    │                              │
           ┌──────┴───────┐                   ┌────────┴───────┐                      ▼
   ┌───────▼──────┐┌──────▼───────┐   ┌───────▼───────┐┌───────▼────────┐
   │  uebung-01   ││   uebung-02  │   │  - Templates  ││  - References  │
   │   Level 1    ││    Level 2   │   │  - Docs       ││  - AI-Prompts  │
   └───────┬──────┘└──────┬───────┘   │  - Utils      ││  - Configs     │
   ╔═══════▼══════╗╔══════▼═══════╗   └───────────────┘└────────────────┘
   ║  solution/   ║║  solution/   ║   ╔═════════════════════════════════╗
   ║  uebung-01   ║║  uebung-02   ║──▶║          support/hints          ║
   ║  (Complete)  ║║  (Complete)  ║   ║          (Help System)          ║
   ╚══════════════╝╚══════════════╝   ╚════════════════╤════════════════╝
                                              ┌────────┴────────┐
                                       ┌──────▼──────┐   ┌──────▼──────┐
                                       │  hints/     │   │  hints/     │
                                       │  uebung-01  │   │  uebung-02  │
                                       └─────────────┘   └─────────────┘
```

---

## 📋 Branch-Details

### 🎯 **1. `main` (oder `master`) - Production Branch**

```ascii
┌─────────────────────────────────────────────────────────────────────┐
│                         MAIN BRANCH                                 │
│                   (Student's Entry Point)                           │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  📁 CleanArchitecture_Template/                  ← ✅ KOMPLETT     │
│     └─ Vollständige Referenz-Implementierung                       │
│                                                                     │
│  📁 CleanArchitecture_Uebung_01/                 ← 🎓 LEVEL 1     │
│     ├─ Domain/                                                     │
│     │  ├─ Entities/              ✅ VORHANDEN                      │
│     │  │  ├─ Venue.cs            ✅ Factory-Methode leer          │
│     │  │  ├─ Event.cs            ⚠️  throw NotImplementedException│
│     │  │  └─ Ticket.cs           ⚠️  throw NotImplementedException│
│     │  ├─ ValidationSpecifications/                                │
│     │  │  ├─ VenueSpecifications.cs        ✅ KOMPLETT            │
│     │  │  ├─ EventSpecifications.cs        ⚠️  TODO               │
│     │  │  └─ TicketSpecifications.cs       ⚠️  TODO               │
│     │  └─ Contracts/             ✅ Interfaces vorhanden          │
│     │                                                              │
│     ├─ Application/                                                │
│     │  ├─ Features/                                                │
│     │  │  ├─ Venues/            ✅ KOMPLETT (Referenz)            │
│     │  │  ├─ Events/            ⚠️  Commands TODO                 │
│     │  │  └─ Tickets/           ⚠️  Commands TODO                 │
│     │  ├─ DTOs/                 ✅ VORHANDEN                       │
│     │  └─ Services/             ⚠️  UniquenessChecker TODO        │
│     │                                                              │
│     ├─ Infrastructure/           ✅ FERTIG (Seeder, Repos)         │
│     ├─ Api/                                                        │
│     │  └─ Controllers/                                             │
│     │     ├─ VenuesController.cs       ✅ KOMPLETT                │
│     │     ├─ EventsController.cs       ⚠️  TODO                   │
│     │     └─ TicketsController.cs      ⚠️  TODO                   │
│     └─ README.md                 ✅ Ausführliche Anleitung         │
│                                                                     │
│  📁 CleanArchitecture_Uebung_02/                 ← 🎓 LEVEL 2     │
│     ├─ Domain/                                                     │
│     │  ├─ Entities/              ⚠️  NUR Interfaces               │
│     │  │  ├─ Author.cs           ⚠️  LEER                         │
│     │  │  ├─ Book.cs             ⚠️  LEER                         │
│     │  │  └─ Loan.cs             ⚠️  LEER                         │
│     │  └─ ValidationSpecifications/   ⚠️  LEER                    │
│     │                                                              │
│     ├─ Application/                                                │
│     │  ├─ Features/              ⚠️  NUR Ordnerstruktur           │
│     │  │  ├─ Authors/           (Commands/Queries Ordner leer)    │
│     │  │  ├─ Books/             (Commands/Queries Ordner leer)    │
│     │  │  └─ Loans/             (Commands/Queries Ordner leer)    │
│     │  └─ DTOs/                 ⚠️  LEER                          │
│     │                                                              │
│     ├─ Infrastructure/           ✅ FERTIG                         │
│     ├─ Api/                      ⚠️  Controller-Gerüste           │
│     └─ README.md                 ✅ Aufgabenstellung              │
│                                                                     │
│  📁 CleanArchitecture_Uebung_03/                 ← 🎓 LEVEL 3     │
│     └─ NUR README.md + leere Solution                             │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

**🎯 Ziel:** Studenten können direkt klonen und mit Übung 1 starten!

---

### 🔧 **2. `dev` - Development Branch**

```ascii
┌─────────────────────────────────────────────────────────────────────┐
│                         DEV BRANCH                                  │
│              (Dein aktiver Arbeitsbereich)                          │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  ✨ STARTET VON: EXAKT deinem aktuellen Stand                      │
│     └─ feature/books-implementation-with-fixes (HEAD)              │
│                                                                     │
│  📁 CleanArchitecture_Uebung_02/                                   │
│     ├─ ✅ VOLLSTÄNDIG implementiert                                │
│     ├─ ✅ CSV-Seeding funktioniert                                 │
│     ├─ ✅ API läuft                                                │
│     ├─ ✅ Books mit Author-Navigation                              │
│     └─ ✅ Alle Tests grün                                          │
│                                                                     │
│  🚧 NÄCHSTE SCHRITTE:                                              │
│     1. UpdateBook Command implementieren                           │
│     2. Loan-Entität + CRUD                                         │
│     3. Übung 03 konzipieren                                        │
│     4. Test-Mitschriften validieren                                │
│                                                                     │
│  💾 COMMITS:                                                       │
│     * c57d7fe fix: Add explicit Mapster mappings                  │
│     * 34b2661 fix: Correct GetBookDto typos                       │
│     * 6963531 refactor: CSV-based seeding                         │
│     * ... (vollständige History)                                   │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

**🎯 Ziel:** Du kannst hier weiterarbeiten, experimentieren, neue Übungen entwickeln!

---

### ✅ **3. `solution/uebung-XX` - Solution Branches**

```ascii
┌─────────────────────────────────────────────────────────────────────┐
│                   SOLUTION BRANCHES                                 │
│              (Complete Reference Solutions)                         │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  📁 solution/uebung-01                                             │
│     └─ CleanArchitecture_Uebung_01/                               │
│        ├─ ✅ ALLE Entities vollständig                             │
│        ├─ ✅ ALLE ValidationSpecifications                         │
│        ├─ ✅ ALLE Commands/Queries/Handlers                        │
│        ├─ ✅ ALLE Controller-Methoden                              │
│        ├─ ✅ ALLE Tests grün                                       │
│        └─ ✅ README mit Lösungshinweisen                           │
│                                                                     │
│  📁 solution/uebung-02                                             │
│     └─ CleanArchitecture_Uebung_02/                               │
│        ├─ ✅ Authors CRUD komplett                                 │
│        ├─ ✅ Books CRUD komplett                                   │
│        ├─ ✅ Loans CRUD komplett                                   │
│        ├─ ✅ CSV-Seeding                                           │
│        ├─ ✅ Domain Validations                                    │
│        ├─ ✅ Integration Tests                                     │
│        └─ ✅ README mit Erklärungen                                │
│                                                                     │
│  📁 solution/uebung-03                                             │
│     └─ CleanArchitecture_Uebung_03/                               │
│        └─ (noch zu definieren)                                     │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

**🎯 Ziel:** Studenten können Lösungen vergleichen, Dozenten können bewerten!

---

### 💡 **4. `support/hints` - Help System Branch**

```ascii
┌─────────────────────────────────────────────────────────────────────┐
│                    SUPPORT/HINTS BRANCH                             │
│              (Progressive Help System)                              │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  📁 hints/uebung-01/                                               │
│     ├─ 01-domain-validations/                                      │
│     │  ├─ hint-1-grundstruktur.md          💡 Level 1             │
│     │  ├─ hint-2-check-methods.md          💡 Level 2             │
│     │  ├─ hint-3-complete-code.md          💡 Level 3             │
│     │  └─ EventSpecifications_PARTIAL.cs   📄 Teilcode            │
│     │                                                              │
│     ├─ 02-commands-handlers/                                       │
│     │  ├─ hint-1-cqrs-pattern.md                                  │
│     │  ├─ hint-2-handler-structure.md                             │
│     │  └─ CreateEventHandler_TEMPLATE.cs                          │
│     │                                                              │
│     ├─ 03-controllers/                                             │
│     │  ├─ hint-1-rest-endpoints.md                                │
│     │  ├─ hint-2-result-extensions.md                             │
│     │  └─ EventsController_SCAFFOLD.cs                            │
│     │                                                              │
│     └─ FAQ.md                            📚 Häufige Fragen        │
│                                                                     │
│  📁 hints/uebung-02/                                               │
│     ├─ 01-csv-seeding/                                             │
│     │  ├─ hint-1-csv-format.md                                    │
│     │  ├─ hint-2-seeder-pattern.md                                │
│     │  └─ StartupDataSeeder_EXAMPLE.cs                            │
│     │                                                              │
│     ├─ 02-navigation-properties/                                   │
│     │  ├─ hint-1-ef-core-includes.md                              │
│     │  ├─ hint-2-mapster-config.md                                │
│     │  └─ BookRepository_PARTIAL.cs                               │
│     │                                                              │
│     └─ TROUBLESHOOTING.md                🔧 Fehlersuche           │
│                                                                     │
│  📁 common-issues/                                                 │
│     ├─ validation-errors.md              ⚠️  Top 10 Fehler        │
│     ├─ ef-core-migrations.md            🗄️  DB-Probleme          │
│     ├─ dependency-injection.md          💉 DI-Fehler              │
│     └─ testing-tips.md                  🧪 Test-Hilfen            │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

**🎯 Ziel:** Gestufte Hilfe - von Hinweisen bis zur vollständigen Lösung!

---

### 🤖 **5. `ai-workspace` - AI Agent Reference Branch**

```ascii
┌─────────────────────────────────────────────────────────────────────┐
│                    AI-WORKSPACE BRANCH                              │
│           (For AI Agent & Repository Management)                    │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  📁 .ai/                                                           │
│     ├─ prompts/                                                    │
│     │  ├─ generate-exercise.md          🤖 Übung generieren       │
│     │  ├─ create-solution.md            ✅ Lösung erstellen       │
│     │  ├─ validate-code.md              🔍 Code validieren        │
│     │  └─ create-tests.md               🧪 Tests generieren       │
│     │                                                              │
│     ├─ templates/                                                  │
│     │  ├─ entity-template.cs            📝 Entity-Vorlage         │
│     │  ├─ command-template.cs           📝 Command-Vorlage        │
│     │  ├─ controller-template.cs        📝 Controller-Vorlage     │
│     │  └─ readme-template.md            📝 README-Vorlage         │
│     │                                                              │
│     ├─ checklists/                                                 │
│     │  ├─ exercise-quality.md           ✓  Qualitätskriterien    │
│     │  ├─ code-review.md                ✓  Code-Review           │
│     │  └─ test-coverage.md              ✓  Test-Abdeckung        │
│     │                                                              │
│     └─ workflows/                                                  │
│        ├─ new-exercise-workflow.md      🔄 Übung erstellen       │
│        ├─ solution-workflow.md          🔄 Lösung erstellen      │
│        └─ hints-workflow.md             🔄 Hilfen erstellen      │
│                                                                     │
│  📁 references/                                                    │
│     ├─ CleanArchitecture_Template/      📚 VOLLE KOPIE           │
│     │  └─ (Komplette Template-Implementierung)                    │
│     │                                                              │
│     ├─ FruehereAufgabenstellungen/      📚 Historische Übungen   │
│     │  ├─ devices.md                   📖 Beispielangabe         │
│     │  └─ measurements.csv             📊 Beispiel-CSV           │
│     │                                                              │
│     └─ Mitschriften_Der_Kollegen/      📝 Student Notes          │
│        ├─ WMC3.txt                     (zu validieren)            │
│        └─ Kollegins_Mitschrift.txt     (zu validieren)            │
│                                                                     │
│  📁 docs/                                                          │
│     ├─ architecture-decisions.md        📐 ADRs                   │
│     ├─ naming-conventions.md            📛 Namenskonventionen    │
│     ├─ validation-layers.md             🔒 Validierungs-Ebenen   │
│     └─ exercise-progression.md          📈 Schwierigkeitsgrade   │
│                                                                     │
│  📁 scripts/                                                       │
│     ├─ create-exercise.ps1              🔧 Übung generieren      │
│     ├─ validate-solution.ps1            🔧 Lösung validieren     │
│     ├─ generate-hints.ps1               🔧 Hilfen generieren     │
│     └─ prepare-main-branch.ps1          🔧 Main vorbereiten      │
│                                                                     │
│  📄 REPOSITORY_STRATEGY.md               📋 Diese Datei!          │
│  📄 BRANCH_MANAGEMENT.md                 📋 Branch-Verwaltung     │
│  📄 CONTRIBUTION_GUIDE.md                📋 Beitrags-Richtlinien  │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

**🎯 Ziel:** Alle Tools, Prompts und Referenzen für effiziente Repository-Verwaltung!

---

## 🔄 Branch-Workflow

```ascii
┌──────────────────────────────────────────────────────────────────────┐
│                    DEVELOPMENT WORKFLOW                              │
└──────────────────────────────────────────────────────────────────────┘

    [1] Neue Übung entwickeln
         │
         ├─▶ dev (neue Features entwickeln)
         │    │
         │    ├─▶ feature/uebung-03-base
         │    │    └─▶ Tests, Implementierung
         │    │
         │    └─▶ merge zurück zu dev
         │
         ▼
    [2] Lösung erstellen
         │
         ├─▶ solution/uebung-03 (von dev branchen)
         │    └─▶ Vollständige Implementierung
         │
         ▼
    [3] Hilfen erstellen
         │
         ├─▶ support/hints (Hints hinzufügen)
         │    └─▶ hints/uebung-03/
         │
         ▼
    [4] Übung vorbereiten
         │
         ├─▶ main (prepared exercise)
         │    ├─▶ NotImplementedExceptions hinzufügen
         │    ├─▶ README erstellen
         │    └─▶ Tests vorbereiten
         │
         └─▶ ✅ Release: Studenten können klonen!


┌──────────────────────────────────────────────────────────────────────┐
│                    BRANCH PROTECTION RULES                           │
├──────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  🔒 main:                                                           │
│     ├─ Require pull request reviews                                │
│     ├─ Require status checks (Build + Tests)                       │
│     ├─ No direct pushes                                             │
│     └─ Only from: dev, solution/* branches                          │
│                                                                      │
│  🔐 solution/*:                                                     │
│     ├─ Protected (no force push)                                    │
│     └─ Require working tests                                        │
│                                                                      │
│  🔓 dev:                                                            │
│     └─ Free development, aber regelmäßige Backups                   │
│                                                                      │
└──────────────────────────────────────────────────────────────────────┘
```

---

## 📊 Übungs-Schwierigkeitsgrade (Progression)

```ascii
┌──────────────────────────────────────────────────────────────────────┐
│                 EXERCISE DIFFICULTY LEVELS                           │
├──────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  🎓 LEVEL 1: Guided Implementation (Uebung_01)                      │
│     ├─ ✅ Alle Klassen vorhanden                                    │
│     ├─ ✅ Interfaces definiert                                      │
│     ├─ ✅ 1-2 vollständige Beispiele (Venue)                        │
│     ├─ ⚠️  throw NotImplementedException für den Rest              │
│     ├─ ✅ Tests vorhanden                                           │
│     └─ ✅ Sehr ausführliches README                                 │
│     │                                                                │
│     └─▶ FOKUS: Domain Validations, Commands/Queries, Controller    │
│                                                                      │
│  🎓 LEVEL 2: Structural Guidance (Uebung_02)                        │
│     ├─ ✅ Ordnerstruktur komplett                                   │
│     ├─ ✅ Interfaces definiert                                      │
│     ├─ ⚠️  Klassen leer oder Gerüste                                │
│     ├─ ⚠️  Student muss Commands/Queries anlegen                    │
│     ├─ ✅ Tests vorhanden                                           │
│     └─ ✅ README mit Aufgabenstellung                               │
│     │                                                                │
│     └─▶ FOKUS: CSV-Seeding, Navigation Properties, Repositories    │
│                                                                      │
│  🎓 LEVEL 3: Independent Implementation (Uebung_03)                 │
│     ├─ ✅ Nur README mit Anforderungen                              │
│     ├─ ✅ Leere Solution                                            │
│     ├─ ⚠️  Student erstellt ALLES selbst                            │
│     ├─ ⚠️  Nur Interface-Definitionen als Hilfe                     │
│     └─ ✅ Tests als Spezifikation                                   │
│     │                                                                │
│     └─▶ FOKUS: Komplexe Business Logic, Advanced Validation        │
│                                                                      │
│  🎓 LEVEL 4: Real-World Scenario (Exam Level)                       │
│     └─ Nur devices.md Style Angabe                                  │
│        ├─ Zeitdruck (3-4 Stunden)                                   │
│        ├─ Komplexe Validierungen                                    │
│        └─ Vollständige eigenständige Implementierung                │
│                                                                      │
└──────────────────────────────────────────────────────────────────────┘
```

---

## 🚀 Initial Setup Commands

```powershell
# 1️⃣ AKTUELLER STAND SICHERN (dev branch)
git checkout -b dev
git add -A
git commit -m "chore: Create dev branch from current implementation state"

# 2️⃣ SOLUTION BRANCH ERSTELLEN (Übung 02)
git checkout -b solution/uebung-02
git add -A
git commit -m "solution: Complete Library Management System implementation"

# 3️⃣ AI WORKSPACE BRANCH
git checkout --orphan ai-workspace
git rm -rf .
# ... AI-spezifische Dateien hinzufügen
git add -A
git commit -m "docs: Initialize AI workspace with templates and prompts"

# 4️⃣ SUPPORT/HINTS BRANCH
git checkout --orphan support/hints
git rm -rf .
# ... Hint-System aufbauen
git add -A
git commit -m "docs: Create progressive hint system for all exercises"

# 5️⃣ MAIN BRANCH VORBEREITEN
git checkout main
# ... NotImplementedException hinzufügen
# ... README für Studenten anpassen
git add -A
git commit -m "feat: Prepare exercises for students (Level 1 & 2)"

# 6️⃣ REMOTE PUSHEN
git push origin main
git push origin dev
git push origin solution/uebung-02
git push origin ai-workspace
git push origin support/hints
```

---

## 📁 Repository Root Structure (main branch)

```ascii
wmc-clean-arch/
├─ 📄 README.md                         ← Übersicht für Studenten
├─ 📄 SETUP.md                          ← Setup-Anleitung
├─ 📄 LEARNING_PATH.md                  ← Lernpfad-Übersicht
├─ 📄 .gitignore
├─ 📄 .editorconfig
│
├─ 📁 CleanArchitecture_Template/       ← ✅ VOLLSTÄNDIG (Referenz)
│  ├─ Api/
│  ├─ Application/
│  ├─ Domain/
│  ├─ Infrastructure/
│  ├─ Tests/
│  └─ README.md
│
├─ 📁 CleanArchitecture_Uebung_01/      ← 🎓 LEVEL 1
│  ├─ Api/
│  ├─ Application/
│  ├─ Domain/
│  ├─ Infrastructure/
│  ├─ Tests/
│  └─ README.md                         ← Sehr ausführlich!
│
├─ 📁 CleanArchitecture_Uebung_02/      ← 🎓 LEVEL 2
│  ├─ Api/
│  ├─ Application/
│  ├─ Domain/
│  ├─ Infrastructure/
│  ├─ Tests/
│  ├─ library_seed_data.csv
│  └─ README.md
│
├─ 📁 CleanArchitecture_Uebung_03/      ← 🎓 LEVEL 3
│  └─ README.md                         ← Nur Anforderungen!
│
└─ 📁 docs/
   ├─ clean-architecture-guide.md
   ├─ cqrs-pattern-explained.md
   ├─ validation-best-practices.md
   └─ common-pitfalls.md
```

---

## ✅ Quality Checklist für main Branch

```ascii
┌──────────────────────────────────────────────────────────────────────┐
│                    MAIN BRANCH QUALITY GATES                         │
├──────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  ✓  Build erfolgreich (dotnet build)                                │
│  ✓  Alle Tests kompilieren (auch wenn sie fehlschlagen)             │
│  ✓  NotImplementedException sinnvoll platziert                      │
│  ✓  README vollständig und verständlich                             │
│  ✓  Kein Solution-Code in Übungs-Branches                           │
│  ✓  CSV-Dateien vorhanden und formatiert                            │
│  ✓  Migrations vorhanden                                            │
│  ✓  Dependencies korrekt (NuGet packages)                           │
│  ✓  Naming Conventions einheitlich                                  │
│  ✓  Kommentare hilfreich aber nicht zu viel                         │
│                                                                      │
└──────────────────────────────────────────────────────────────────────┘
```

---

## 🎯 Next Steps

1. ✅ **DEV Branch erstellen** - EXAKT von aktuellem Stand
2. ✅ **Solution Branch** - Übung 02 komplettieren
3. ⚠️ **Main vorbereiten** - NotImplementedException hinzufügen
4. ⚠️ **Hints erstellen** - Progressive Hilfe-System
5. ⚠️ **AI Workspace** - Templates und Prompts
6. ⚠️ **Tests erweitern** - Integration Tests
7. ⚠️ **README polieren** - Für jeden Schwierigkeitsgrad

---

**Version:** 1.0  
**Erstellt:** 2025-11-16  
**Status:** 🚧 In Arbeit - Bereit für Umsetzung!
