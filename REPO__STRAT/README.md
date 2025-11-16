# 📚 WMC Clean Architecture - Repository Strategy Overview

## 🎯 Was ist hier dokumentiert?

Dieses Verzeichnis enthält die **vollständige Strategie** für den Aufbau und die Verwaltung des WMC Clean Architecture Learning Repository.

---

## 📄 Dokumente

### 1. [GITHUB_REPOSITORY_STRAT.md](./GITHUB_REPOSITORY_STRAT.md)
**Die Hauptstrategie - START HIER!**

```ascii
┌─────────────────────────────────────────────────┐
│      BRANCH STRATEGIE MIT ASCII-ART             │
├─────────────────────────────────────────────────┤
│  • 5 Branch-Typen definiert                     │
│  • Repository-Struktur visualisiert             │
│  • Schwierigkeitsgrade (Level 1-4) erklärt      │
│  • Quality Gates pro Branch                     │
│  • Initial Setup Commands                       │
└─────────────────────────────────────────────────┘
```

**Inhalt:**
- 🌳 **Branch-Strategie** mit vollständiger Visualisierung
- 📁 **Repository-Struktur** für master, dev, solution/*, support/hints, ai-workspace
- 🎓 **Exercise Levels** (1: Guided → 4: Exam)
- 🚀 **Setup-Commands** für initiale Branch-Erstellung
- ✅ **Quality Checklists** für jeden Branch

---

### 2. [BRANCH_MANAGEMENT.md](./BRANCH_MANAGEMENT.md)
**Praktische Anleitung für Branch-Verwaltung**

```ascii
┌─────────────────────────────────────────────────┐
│      WORKFLOWS & DAILY OPERATIONS               │
├─────────────────────────────────────────────────┤
│  • Schritt-für-Schritt Setup                    │
│  • Standard-Workflows                           │
│  • Branch-Protection Rules                      │
│  • Notfall-Szenarien                            │
└─────────────────────────────────────────────────┘
```

**Inhalt:**
- 🔧 **Initiales Setup** - Wie erstelle ich alle Branches?
- 🔄 **Workflows** - Neue Übung, Lösung, Hints erstellen
- 🛡️ **Branch Protection** - GitHub Settings
- 🔍 **Branch-Status** - Wie überprüfe ich alles?
- 🚨 **Notfall-Szenarien** - Was tun bei Problemen?
- ✅ **Pre-Push Checklists** - Vor jedem Push prüfen

---

### 3. [MITSCHRIFTEN_VALIDIERUNG.md](./MITSCHRIFTEN_VALIDIERUNG.md)
**Validierung der Kollegen-Notizen gegen Template**

```ascii
┌─────────────────────────────────────────────────┐
│      STUDENT NOTES VALIDATION                   │
├─────────────────────────────────────────────────┤
│  ✅ Bestätigte Konzepte                         │
│  ⚠️  Zu klärende Punkte                         │
│  ❌ Korrekturbedarf                             │
│  📊 Validierungs-Matrix                         │
└─────────────────────────────────────────────────┘
```

**Inhalt:**
- ✅ **Bestätigt**: Validierungs-Ebenen, CQRS, Result Pattern, Repository Pattern
- ⚠️ **Zu klären**: ModelCreating, DataSeeder, Update ID-Check
- ❌ **WICHTIGE KORREKTUR**: "Validation gehört IN die Factory-Methode, nicht nachträglich!"
- 📊 **Validierungs-Matrix** - Was stimmt, was nicht?
- 🎯 **Empfehlungen** für Übungs-Entwicklung

**Wichtigste Erkenntnis:**
```csharp
// ❌ FALSCH (laut Mitschrift):
// "ALS ERSTES ENTITIES ANLEGEN (OHNE VALIDATION AM ANFANG!)"

// ✅ RICHTIG (laut Template):
public static async Task<Book> CreateAsync(...)
{
    ValidateBookProperties(...);  // ← SOFORT!
    await ValidateBookExternal(...);
    return new Book { ... };
}
```

---

### 4. [EXERCISE_PROGRESSION.md](./EXERCISE_PROGRESSION.md)
**Detaillierte Schwierigkeitsgrad-Definition**

```ascii
┌─────────────────────────────────────────────────┐
│      SCAFFOLDING PRINCIPLE                      │
│      (Gerüst-Reduktion)                         │
├─────────────────────────────────────────────────┤
│  Level 1: Maximum Guidance     ████████         │
│  Level 2: Structural Guidance  ██████░░         │
│  Level 3: Conceptual Guidance  ███░░░░░         │
│  Level 4: Exam Scenario        █░░░░░░░         │
└─────────────────────────────────────────────────┘
```

**Inhalt:**
- 🎓 **Level 1**: Guided Implementation (Event Management)
  - Alle Klassen vorhanden, 1-2 Beispiele komplett
  - Student implementiert nur TODO-Methoden
  - Sehr ausführliches README
  
- 🎓 **Level 2**: Structural Guidance (Library Management)
  - Ordnerstruktur + Gerüste
  - Student erstellt Commands/Queries selbst
  - Mittel-detailliertes README
  
- 🎓 **Level 3**: Conceptual Guidance (Device Management)
  - Nur README + Tests
  - Student baut ALLES selbst
  - Minimal README
  
- 🎓 **Level 4**: Exam Scenario
  - Nur Anforderungsdokument
  - Zeitlimit 3-4h
  - Keine Hilfe

**Progression-Matrix:**
| Aspect | L1 | L2 | L3 | L4 |
|--------|----|----|----|----|
| Entities | ✅ | ⚠️ | ⚠️ | ⚠️ |
| Commands | ⚠️ | ⚠️ | ⚠️ | ⚠️ |
| Repos | ✅ | ⚠️ | ⚠️ | ⚠️ |
| README | ✅✅✅ | ✅✅ | ✅ | ⚠️ |

---

## 🚀 Quick Start - Nächste Schritte

### 1. **DEV BRANCH ERSTELLEN** (SOFORT!)

```powershell
# WICHTIG: Von EXAKT diesem Stand!
git checkout feature/books-implementation-with-fixes
git checkout -b dev
git push -u origin dev

# Jetzt kannst du auf dev weiterarbeiten!
```

**Warum wichtig?**
- ✅ Übung 02 ist vollständig implementiert
- ✅ CSV-Seeding funktioniert
- ✅ Books mit Author-Navigation korrekt
- ✅ Alle Fixes sind drin

### 2. **SOLUTION BRANCH** (Übung 02)

```powershell
git checkout dev
git checkout -b solution/uebung-02

# README für Lösungs-Branch anpassen:
# - Lösungshinweise hinzufügen
# - Erklärungen zu komplexen Teilen
# - Best Practices dokumentieren

git add -A
git commit -m "solution: Complete Library Management System"
git push -u origin solution/uebung-02
```

### 3. **MAIN BRANCH VORBEREITEN** (Level 2)

```powershell
git checkout master

# Übung 02 von solution übernehmen:
git checkout solution/uebung-02 -- CleanArchitecture_Uebung_02/

# JETZT: Level 2 Style anwenden:
# 1. Commands/Handlers/Validators LÖSCHEN (nur Ordner behalten)
# 2. Entities: Nur Gerüste lassen
# 3. ValidationSpecifications: LEEREN
# 4. DTOs: LÖSCHEN
# 5. Services: LEEREN
# 6. Controller: Nur Gerüste

# README.md für Studenten schreiben (siehe EXERCISE_PROGRESSION.md)

git add -A
git commit -m "feat: Prepare Uebung 02 for students (Level 2)"
git push origin master
```

### 4. **AI-WORKSPACE BRANCH** (für dich)

```powershell
git checkout --orphan ai-workspace
git rm -rf .

# Struktur erstellen:
mkdir -p .ai/prompts .ai/templates .ai/checklists .ai/workflows
mkdir -p references scripts docs

# Template kopieren:
xcopy /E /I CleanArchitecture_Template references\CleanArchitecture_Template
xcopy /E /I FruehereAufgabenstellungen references\FruehereAufgabenstellungen
xcopy /E /I Mitschriften_Der_Kollegen references\Mitschriften_Der_Kollegen

# REPO__STRAT Dokumente kopieren:
xcopy /E /I REPO__STRAT docs\

git add -A
git commit -m "docs: Initialize AI workspace with references"
git push -u origin ai-workspace
```

### 5. **SUPPORT/HINTS BRANCH** (Hilfe-System)

```powershell
git checkout --orphan support/hints
git rm -rf .

# Hint-Struktur:
mkdir -p hints\uebung-01\01-domain-validations
mkdir -p hints\uebung-01\02-commands-handlers
mkdir -p hints\uebung-01\03-controllers
mkdir -p hints\uebung-02\01-csv-seeding
mkdir -p hints\uebung-02\02-navigation-properties
mkdir -p hints\uebung-02\03-repository-methods
mkdir -p common-issues

# Hints erstellen (siehe unten)

git add -A
git commit -m "docs: Create hint system for exercises"
git push -u origin support/hints
```

---

## 📝 Hint-System Beispiel

### hints/uebung-02/02-navigation-properties/hint-1-grundlagen.md

```markdown
# Hint 1: Navigation Properties Grundlagen

## Problem
BookRepository gibt Books zurück, aber `Author` ist immer `null`?

## Erklärung
Entity Framework lädt standardmäßig KEINE Navigation Properties!

## Lösung
Verwende `.Include()` in deinen Repository-Methoden:

```csharp
public override async Task<Book?> GetByIdAsync(int id, CancellationToken ct = default)
{
    return await Set
        .Include(b => b.Author)  // ← Wichtig!
        .FirstOrDefaultAsync(b => b.Id == id, ct);
}
```

## Weitere Schritte
- Hint 2: Mapster Configuration
- Hint 3: Vollständiger Code
```

---

## 🎯 Übersicht: Was ist wo?

| Branch | Zweck | Status | Für wen? |
|--------|-------|--------|----------|
| **master** | Student Entry Point | ⚠️ TODO | Studenten |
| **dev** | Development | ✅ Bereit | Du |
| **solution/uebung-01** | Vollständige Lösung 01 | ⚠️ TODO | Dozent/Student |
| **solution/uebung-02** | Vollständige Lösung 02 | ✅ Bereit | Dozent/Student |
| **support/hints** | Hilfe-System | ⚠️ TODO | Studenten |
| **ai-workspace** | AI References | ⚠️ TODO | Du + AI |

---

## ✅ Checklist für Repository-Aufbau

### Phase 1: Branches erstellen
- [ ] `dev` Branch von aktuellem Stand
- [ ] `solution/uebung-02` Branch mit vollständiger Lösung
- [ ] `ai-workspace` Branch (orphan) mit Referenzen
- [ ] `support/hints` Branch (orphan) mit Hilfe-System

### Phase 2: master Branch vorbereiten
- [ ] Übung 01: Level 1 Style (Guided Implementation)
- [ ] Übung 02: Level 2 Style (Structural Guidance)
- [ ] Übung 03: Konzipieren (Level 3?)

### Phase 3: Lösungen erstellen
- [ ] `solution/uebung-01` komplettieren
- [ ] `solution/uebung-02` dokumentieren
- [ ] Tests für beide Übungen

### Phase 4: Hints erstellen
- [ ] Übung 01: 3 Hint-Level pro Aufgabe
- [ ] Übung 02: 3 Hint-Level pro Aufgabe
- [ ] Common Issues Dokumentation

### Phase 5: AI-Workspace
- [ ] Prompts für Übungs-Generierung
- [ ] Templates für Code-Scaffolding
- [ ] Checklists für Quality Gates
- [ ] Workflows dokumentieren

### Phase 6: GitHub Setup
- [ ] Branch Protection Rules aktivieren
- [ ] README.md im Root
- [ ] CONTRIBUTING.md
- [ ] Tags für Semester-Versionen

---

## 🔥 WICHTIGSTE ERKENNTNISSE

### 1. **Validation gehört IN Factory-Methoden!**
```csharp
// ✅ RICHTIG:
public static async Task<Entity> CreateAsync(...)
{
    ValidateProperties(...);     // ← SOFORT
    await ValidateExternal(...);  // ← SOFORT
    return new Entity { ... };
}
```

### 2. **Navigation Properties + .Include()**
```csharp
// ✅ In Repository:
return await Set
    .Include(b => b.Author)
    .FirstOrDefaultAsync(...);
```

### 3. **3 Validierungs-Ebenen**
- **Domain**: Grundregeln (immer)
- **FluentValidation**: UseCase-spezifisch
- **External**: Via Services (Uniqueness)

### 4. **Scaffolding-Prinzip**
Level 1 → 2 → 3 → 4: Gerüst wird schrittweise reduziert

### 5. **Template ist OBERSTE AUTORITÄT**
Bei Widersprüchen: Template > Mitschriften

---

## 📚 Weitere Ressourcen

- **Template:** `CleanArchitecture_Template/` - IMMER als Referenz!
- **Übung 01:** `CleanArchitecture_Uebung_01/` - Teilweise implementiert
- **Übung 02:** `CleanArchitecture_Uebung_02/` - Vollständig (auf dev)
- **Devices:** `FruehereAufgabenstellungen/devices.md` - Zielformat für Angaben

---

## 🤝 Kontakt & Support

Bei Fragen zur Strategie:
1. Lese zuerst [GITHUB_REPOSITORY_STRAT.md](./GITHUB_REPOSITORY_STRAT.md)
2. Prüfe [BRANCH_MANAGEMENT.md](./BRANCH_MANAGEMENT.md) für Workflows
3. Konsultiere [EXERCISE_PROGRESSION.md](./EXERCISE_PROGRESSION.md) für Level-Details

---

**Status:** ✅ Strategie vollständig dokumentiert  
**Nächster Schritt:** Dev Branch erstellen und weiterarbeiten!  
**Version:** 1.0  
**Erstellt:** 2025-11-16
