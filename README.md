# Übung für den WMC Test 2025/26

## Struktur

```filesystem
WMC_2025_26_test_learning_01/
├── CleanArchitecture_Template/      # Vollständiges Template als Referenz
│   ├── Domain/
│   ├── Application/
│   ├── Infrastructure/
│   ├── Api/
│   ├── Domain.Tests/
│   └── Api.Tests/
│
├── CleanArchitecture_Uebung_01/     # Übung 1: Event Management System
│   ├── Domain/                      # Venue, Event, Ticket
│   ├── Application/
│   ├── Infrastructure/
│   ├── Api/
│   ├── Domain.Tests/
│   ├── Api.Tests/
│   └── README.md                    # Ausführliche Aufgabenstellung
│
├── CleanArchitecture_Uebung_02/     # Übung 2: Library Management System
│   ├── Domain/                      # Book, Author, Loan
│   ├── Application/
│   ├── Infrastructure/
│   ├── Api/
│   ├── Domain.Tests/
│   ├── Api.Tests/
│   └── README.md                    # Ausführliche Aufgabenstellung
│
├── FruehereAufgabenstellungen/      # Frühere Prüfungsangaben (Referenz)
│   ├── Domain/
│   ├── Application/
│   ├── Infrastructure/
│   └── Api/
│
├── .gitignore
├── Prompt_Fuer_Erste_Uebung.md      # Prompt für Übung 1
└── Prompt_Fuer_Zweite_Uebung.md     # Prompt für Übung 2
```

## Zur Aufgabenstellung

### 📚 Übungen

#### Übung 1: Event Management System
- [CleanArchitecture_Uebung_01/README.md](CleanArchitecture_Uebung_01/README.md)
  > **Entitäten:** Venue, Event, Ticket
  > **Fokus:** NotImplementedException-Stil mit TODOs
  > **Lücken:** Event-Validierungen, Handler, Controller-Methoden

#### Übung 2: Library Management System
- [CleanArchitecture_Uebung_02/README.md](CleanArchitecture_Uebung_02/README.md)
  > **Entitäten:** Book, Author, Loan
  > **Fokus:** Professor-Stil (leere Ordner, leere Controller)
  > **Lücken:** Loan-Validierungen, Commands/Queries von Grund auf, Repository-Methoden

### 🎯 Zur Referenz

- [CleanArchitecture_Template](CleanArchitecture_Template)
  > Vollständiges Template als Referenz für alle Patterns

- [FruehereAufgabenstellungen](FruehereAufgabenstellungen)
  > Frühere Prüfungsangaben vom Professor (zeigt den Stil der Lücken)

---

## 🎓 Unterschiede zwischen den Übungen

| Aspekt | Übung 01 | Übung 02 |
|--------|----------|----------|
| **Domäne** | Event Management | Library Management |
| **Entitäten** | Venue, Event, Ticket | Book, Author, Loan |
| **Lücken-Stil** | NotImplementedException | Leere Ordner (Professor-Stil) |
| **Fokus** | Handler & Validierungen | Commands/Queries von Grund auf |
| **Schwierigkeit** | Mittel | Mittel-Schwer |

---

## ✅ Empfohlene Reihenfolge

1. **Zuerst:** Schaue dir das [CleanArchitecture_Template](CleanArchitecture_Template) an
2. **Dann:** Starte mit [Übung 01](CleanArchitecture_Uebung_01/README.md)
3. **Danach:** Mache [Übung 02](CleanArchitecture_Uebung_02/README.md)
4. **Optional:** Vergleiche mit [FruehereAufgabenstellungen](FruehereAufgabenstellungen)

**Viel Erfolg beim Lernen! 🚀**
