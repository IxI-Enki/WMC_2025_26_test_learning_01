# 💡 Support & Hints Branch

## Purpose

This branch contains **progressive hints** and **troubleshooting guides** for students working on the exercises.

**❗ FOR STUDENTS WHO ARE STUCK!**

---

## Structure

```ascii
support/hints/
├─ hints/
│  ├─ uebung-01/
│  │  ├─ 01-domain-validations/
│  │  │  ├─ hint-1-concept.md       (Level 1: Concept & Direction)
│  │  │  ├─ hint-2-structure.md     (Level 2: Structure & Pattern)
│  │  │  └─ hint-3-complete.md      (Level 3: Complete Solution)
│  │  ├─ 02-commands-handlers/
│  │  └─ 03-controllers/
│  │
│  └─ uebung-02/
│     ├─ 01-csv-seeding/
│     ├─ 02-navigation-properties/  (✅ Complete)
│     └─ 03-repository-methods/
│
├─ common-issues/
│  └─ FAQ.md                         (✅ Complete)
│
└─ README.md                         (This file)
```

---

## 🎯 How to Use Hints

### Progressive 3-Level System

```ascii
┌──────────────────────────────────────────────────────┐
│          PROGRESSIVE HINT STRATEGY                   │
├──────────────────────────────────────────────────────┤
│                                                      │
│  Level 1: Concept & Direction                       │
│  ├─ What needs to be done                           │
│  ├─ Template reference                              │
│  └─ High-level approach                             │
│      ↓                                               │
│  Level 2: Structure & Pattern                       │
│  ├─ Method signatures                               │
│  ├─ Partial code with placeholders                  │
│  └─ Key points                                      │
│      ↓                                               │
│  Level 3: Complete Solution                         │
│  ├─ Full working code                               │
│  ├─ Detailed explanations                           │
│  └─ Common mistakes                                 │
│                                                      │
└──────────────────────────────────────────────────────┘
```

### When Stuck

1. **Try yourself first** (at least 15-20 minutes)
2. **Read Hint 1** (Concept) - understand what you need to do
3. **Try again** with the concept in mind
4. **Read Hint 2** (Structure) - if still stuck
5. **Try again** with the structure
6. **Read Hint 3** (Complete) - only if really necessary

**⚠️ Don't skip to Hint 3 immediately!** You learn more by struggling a bit.

---

## 📁 Available Hints

### ✅ Übung 01: Event Management

#### 01-domain-validations/ ✅
**Problem:** EventSpecifications implementieren

- **Hint 1:** Konzept - Was sind ValidationSpecifications?
- **Hint 2:** Struktur - Signaturen & Logik
- **Hint 3:** Vollständig - Kompletter Code mit Erklärungen

**Status:** Complete

---

### ✅ Übung 02: Library Management

#### 02-navigation-properties/ ✅
**Problem:** Author ist immer null in API Response

- **Hint 1:** Konzept - Was sind Navigation Properties & .Include()
- **Hint 2:** Struktur - Welche Methoden anpassen
- **Hint 3:** Vollständig - Kompletter BookRepository

**Status:** Complete

---

### ⚠️ Coming Soon

- **Übung 01:**
  - 02-commands-handlers/ (Factory Methods, Command Handlers)
  - 03-controllers/ (REST API, Result Pattern)

- **Übung 02:**
  - 01-csv-seeding/ (CSV Import, Foreign Keys)
  - 03-repository-methods/ (Custom Queries)

---

## ❓ FAQ & Common Issues

See: [`common-issues/FAQ.md`](common-issues/FAQ.md) ✅

**Covers:**
- Build & Setup Problems
- Domain Layer Questions
- Application Layer Questions
- Infrastructure Layer (Navigation Properties, CSV Seeding)
- API Layer Issues
- Testing Problems
- General Issues

---

## 🎓 Learning Philosophy

### Why Progressive Hints?

1. **Learning by Doing:** Struggling a bit is GOOD for learning
2. **Understanding Depth:** Each level adds more detail
3. **Avoid Spoilers:** Hint 1 doesn't give away the solution
4. **Template Reference:** Always points to the authoritative source

### Best Practice

```ascii
Problem → Try (15 min) → Hint 1 → Try (15 min) → Hint 2 → Try (15 min) → Hint 3
```

**Not:**
```ascii
Problem → Hint 3 (immediate spoiler)
```

---

## 🔍 How to Find the Right Hint

### By Problem

| Problem | Hint Location |
|---------|---------------|
| EventSpecifications nicht klar | `hints/uebung-01/01-domain-validations/` |
| Author ist null in API | `hints/uebung-02/02-navigation-properties/` |
| CSV-Seeding funktioniert nicht | `common-issues/FAQ.md` (bis Hints erstellt) |
| Build-Fehler | `common-issues/FAQ.md` |
| Tests schlagen fehl | `common-issues/FAQ.md` |

### By Exercise

**Übung 01:**
```
hints/uebung-01/
├─ 01-domain-validations/  ← EventSpecifications
├─ 02-commands-handlers/   ← Create/Update Commands
└─ 03-controllers/         ← REST API Actions
```

**Übung 02:**
```
hints/uebung-02/
├─ 01-csv-seeding/         ← Data Import
├─ 02-navigation-properties/ ← .Include() Problem
└─ 03-repository-methods/  ← Custom Queries
```

---

## 📚 Additional Resources

### Template (OBERSTE AUTORITÄT)
```powershell
git checkout dev
code CleanArchitecture_Template/
```

Always refer to Template for the correct patterns!

### Exercise READMEs

Each exercise has a detailed README:
```
CleanArchitecture_Uebung_XX/README.md
```

### Solution Branches

Complete, working solutions:
```powershell
git checkout solution/uebung-01
git checkout solution/uebung-02
```

**But:** Only look at solutions AFTER you've tried with hints!

---

## 🤝 Contributing Hints

Want to add more hints?

1. **Follow the 3-level structure**
2. **Don't spoil in Level 1**
3. **Reference Template**
4. **Test your hints** (do they actually help?)
5. **Update this README**

---

## ⚠️ Important Notes

- **Hints are NOT the official solution** (see `solution/*` branches for that)
- **Hints guide you to the solution** (step by step)
- **Always try first** before reading hints
- **Template is authority** (hints just explain it)

---

**Branch:** support/hints (orphan)  
**Purpose:** Progressive Help System  
**Status:** ✅ Partial (2 hint sets complete, FAQ complete)  
**Version:** 1.0  
**Last Updated:** 2025-11-16

