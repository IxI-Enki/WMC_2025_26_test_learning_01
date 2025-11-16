#  Branch Management Guide

##  Schnellreferenz

```ascii
┌────────────────────────────────────────────────────────────────┐
│                     BRANCH QUICK REFERENCE                     │
├────────────────────────────────────────────────────────────────┤
│  Branch               │ Zweck            │ Schutz │ Push       │
├───────────────────────┼──────────────────┼────────┼────────────┤
│  master              │ Student Entry    │  Ja   │ PR only    │
│  dev                 │ Development      │  Nein │ Direct OK  │
│  solution/uebung-XX  │ Full Solutions   │  Ja   │ PR review  │
│  support/hints       │ Help System      │  Ja   │ PR review  │
│  ai-workspace        │ AI References    │  Nein │ Direct OK  │
└───────────────────────┴──────────────────┴────────┴────────────┘
```

<!--
    

              

               
               
              
      
-->

---

## Initiales Setup (Erste Ausführung)

### Schritt 1: Dev Branch von aktuellem Stand erstellen

```powershell
# Aktuellen Stand committen
git status  # Überprüfen was offen ist
git add -A
git commit -m "chore: Prepare for branch restructure - current working state"

# Dev Branch erstellen (VON HIER AUS!)
git checkout -b dev
git push -u origin dev

# Zurück zu master für weitere Vorbereitung
git checkout master
```

### Schritt 2: Solution Branch für Übung 02 erstellen

```powershell
# Von dev branchen (hat die komplette Implementierung)
git checkout dev

# Solution Branch erstellen
git checkout -b solution/uebung-02

# README für Lösungsbranch anpassen
# ... (Lösungshinweise hinzufügen)

git add -A
git commit -m "solution: Complete Library Management System with explanations"
git push -u origin solution/uebung-02
```

### Schritt 3: AI Workspace Branch (Orphan)

```powershell
# Komplett neuer Branch ohne History
git checkout --orphan ai-workspace

# Alles entfernen
git rm -rf .

# Nur AI-spezifische Inhalte hinzufügen
mkdir -p .ai/prompts .ai/templates .ai/checklists .ai/workflows
mkdir -p references scripts docs

# Template kopieren
cp -r ../CleanArchitecture_Template references/
cp -r ../FruehereAufgabenstellungen references/
cp -r ../Mitschriften_Der_Kollegen references/

# AI-Prompts erstellen (siehe unten)
# ...

git add -A
git commit -m "docs: Initialize AI workspace with templates and references"
git push -u origin ai-workspace
```

### Schritt 4: Support/Hints Branch (Orphan)

```powershell
# Neuer Hints Branch
git checkout --orphan support/hints

git rm -rf .

# Hint-Struktur erstellen
mkdir -p hints/uebung-01/{01-domain-validations,02-commands-handlers,03-controllers}
mkdir -p hints/uebung-02/{01-csv-seeding,02-navigation-properties,03-repository-methods}
mkdir -p common-issues

# Hints erstellen (siehe unten)
# ...

git add -A
git commit -m "docs: Create progressive hint system for exercises"
git push -u origin support/hints
```

### Schritt 5: Main Branch für Studenten vorbereiten

```powershell
git checkout master

# Übung 01: NotImplementedException hinzufügen
# Übung 02: Mehr Code entfernen, struktureller machen

# Student-README erstellen
# ...

git add -A
git commit -m "feat: Prepare exercises for students with guided implementations"
git push origin master
```

---

## Standard-Workflows

### Workflow 1: Neue Übung entwickeln

```ascii
[Start: dev]
    │
    ├─▶ Neue Entitäten konzipieren
    ├─▶ Domain Layer implementieren
    ├─▶ Application Layer aufbauen
    ├─▶ Infrastructure konfigurieren
    ├─▶ API Controller erstellen
    ├─▶ Tests schreiben
    │
    └─▶ git commit -m "feat: Add Uebung 03 - Device Management"

[Branch: solution/uebung-03]
    │
    ├─▶ git checkout -b solution/uebung-03
    ├─▶ Vollständige Lösung + README
    ├─▶ Lösungskommentare hinzufügen
    │
    └─▶ git commit -m "solution: Complete Device Management implementation"

[Branch: master - Übung vorbereiten]
    │
    ├─▶ git checkout master
    ├─▶ Von solution/uebung-03 Struktur übernehmen
    ├─▶ NotImplementedException hinzufügen
    ├─▶ Student-README erstellen
    │
    └─▶ git commit -m "feat: Add Uebung 03 (Level 2) for students"

[Branch: support/hints]
    │
    ├─▶ git checkout support/hints
    ├─▶ Hint-Ordner erstellen: hints/uebung-03/
    ├─▶ Progressive Hilfen schreiben
    │
    └─▶ git commit -m "docs: Add hints for Uebung 03"
```

### Workflow 2: Bestehende Übung verbessern

```powershell
# Auf dev arbeiten
git checkout dev

# Improvements in Übung 01
cd CleanArchitecture_Uebung_01
# ... Verbesserungen

git add -A
git commit -m "improve: Enhance Uebung 01 validation examples"

# Lösung aktualisieren
git checkout solution/uebung-01
git merge dev --strategy-option theirs  # Nur wenn sinnvoll
# Oder manuell anpassen

git checkout master
# Student-Version entsprechend anpassen
```

### Workflow 3: Fehler in Lösung fixen

```powershell
# Direkt auf solution branch
git checkout solution/uebung-02

# Fix implementieren
# ...

git add -A
git commit -m "fix: Correct BookRepository eager loading"
git push origin solution/uebung-02

# Auch master prüfen ob Fix nötig
git checkout master
# Eventuell NotImplementedException-Bereich betroffen?
```

---

## Branch-Status überprüfen

```powershell
# Alle Branches anzeigen
git branch -a

# Branch-Unterschiede visualisieren
git log --oneline --graph --all --decorate -20

# Aktuellen Branch Status
git status

# Commits zwischen Branches vergleichen
git log master..dev --oneline
git log solution/uebung-02..master --oneline

# Dateien zwischen Branches vergleichen
git diff master:CleanArchitecture_Uebung_02/Domain/Entities/Book.cs solution/uebung-02:CleanArchitecture_Uebung_02/Domain/Entities/Book.cs
```

---

## Branch Protection Rules (GitHub)

### master Branch

```yaml
Protection Rules:
  - Require pull request reviews: 
    - Required approvals: 1
    - Dismiss stale reviews: 
  
  - Require status checks: 
    - dotnet build
    - dotnet test (kann fehlschlagen bei NotImplementedException)
  
  - Require branches to be up to date: 
  
  - Restrict who can push: 
    - Only: Repository Admin
  
  - Allow force pushes: ❌
  - Allow deletions: ❌
```

### solution/* Branches

```yaml
Protection Rules:
  - Require pull request reviews: 
    - Required approvals: 1
  
  - Require status checks: 
    - dotnet build: MUST PASS 
    - dotnet test: MUST PASS 
  
  - Allow force pushes: ❌
  - Allow deletions: ❌
```

### dev Branch

```yaml
Protection Rules:
  - Require pull request reviews: ❌ (freies Entwickeln)
  - Require status checks: ⚠️ Optional
  - Allow force pushes: ⚠️ Mit Vorsicht
  - Allow deletions: ❌
```

---

##  Branch-Synchronisation

###  Main ← Solution (Übung vorbereiten)

```powershell
# Solution hat fertige Implementierung
# Main braucht Student-Version

git checkout master
git checkout solution/uebung-02 -- CleanArchitecture_Uebung_02/

# JETZT: NotImplementedException hinzufügen
# Dateien bearbeiten...

git add -A
git commit -m "feat: Prepare Uebung 02 from solution (Level 2 difficulty)"
```

###  Dev ← Solution (Features zurückführen)

```powershell
# Gute Features aus Solution zurück zu dev

git checkout dev
git cherry-pick <commit-hash-from-solution>

# Oder selektiv Dateien
git checkout solution/uebung-03 -- path/to/useful/file.cs
```

###  Hints ← Solution (Hilfen extrahieren)

```powershell
# Aus Solution Hints generieren
git checkout support/hints

# Lösung ansehen
git show solution/uebung-02:CleanArchitecture_Uebung_02/Domain/Entities/Book.cs > temp-book.cs

# Daraus Hints erstellen (manuell)
# hint-1: Nur Signatur
# hint-2: Signatur + Kommentare
# hint-3: Vollständiger Code
```

---

## Regelmäßige Wartung

### Wöchentlich

```powershell
# 1.  Dev Branch Status
git checkout dev
git log --since="1 week ago" --oneline
git status

# 2.  Solution Branches prüfen
for branch in solution/uebung-01 solution/uebung-02; do
    git checkout $branch
    dotnet build
    dotnet test
done

# 3.  Main Branch testen
git checkout master
cd CleanArchitecture_Uebung_01 && dotnet build
cd ../CleanArchitecture_Uebung_02 && dotnet build
```

### Vor jedem Semester

```powershell
# 1. Alle Dependencies aktualisieren
git checkout dev
# Update NuGet packages in allen Projekten

# 2. Tests aktualisieren
# 3. README überarbeiten
# 4. Neue Übung hinzufügen?

# 5. Solution Branches updaten
# 6. Main Branch aktualisieren
# 7. Tag für Semester erstellen
git tag -a v2025-ws -m "Winter Semester 2025/26"
git push origin v2025-ws
```

---

## Notfall-Szenarien

### Versehentlich auf master gepusht

```powershell
# SOFORT:
git checkout master
git reset --hard origin/master

# Fix auf dev machen
git checkout dev
# ... Änderungen
git commit -m "fix: ..."

# Dann sauber via PR zu master
```

### Dev Branch kaputt

```powershell
# Backup erstellen
git checkout dev
git branch dev-backup-$(date +%Y%m%d)

# Von letztem guten Commit neu starten
git reset --hard <good-commit-hash>

# Oder von solution neu aufbauen
git checkout solution/uebung-02
git checkout -b dev-new
# ... aufräumen
git branch -D dev
git branch -m dev-new dev
```

### Solution und master out of sync

```powershell
# 1. Welche Version ist richtig?
git diff solution/uebung-02 master -- CleanArchitecture_Uebung_02/

# 2. Solution ist führend → master aktualisieren
git checkout master
git checkout solution/uebung-02 -- CleanArchitecture_Uebung_02/
# NotImplementedException hinzufügen
git commit -m "sync: Update master from solution/uebung-02"
```

---

##  Pre-Push Checklist

### Vor Push zu `master`

```checklist
□ dotnet build erfolgreich in ALLEN Übungen
□ Tests kompilieren (dürfen fehlschlagen bei NotImpl)
□ README aktualisiert und vollständig
□ Keine Lösungs-Code-Reste in Student-Version
□ CSV-Dateien vorhanden
□ Migrations up-to-date
□ Git History sauber (kein Merge-Chaos)
```

### Vor Push zu `solution/*`

```checklist
□ dotnet build erfolgreich
□ ALLE Tests grün 
□ Kein NotImplementedException
□ README mit Lösungshinweisen
□ Code gut dokumentiert
□ Best Practices befolgt
□ Validierungen vollständig
```

### Vor Push zu `dev`

```checklist
□ Build erfolgreich (reicht)
□ Keine Breaking Changes ohne Grund
□ Commit Messages aussagekräftig
```

---

##  Weitere Ressourcen

- [GITHUB_REPOSITORY_STRAT.md](./GITHUB_REPOSITORY_STRAT.md) - Vollständige Strategie
- [EXERCISE_PROGRESSION.md](./EXERCISE_PROGRESSION.md) - Schwierigkeitsgrade
- [QUALITY_GATES.md](./QUALITY_GATES.md) - Qualitätskriterien

---

**Version:** 1.0  
**Letzte Aktualisierung:** 2025-11-16
