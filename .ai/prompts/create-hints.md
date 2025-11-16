# 🤖 AI Prompt: Create Progressive Hint System

## Context

You are creating a 3-level progressive hint system for an exercise to help stuck students without giving away the complete solution.

## Hint Level Strategy

```ascii
┌──────────────────────────────────────────────────────────┐
│          PROGRESSIVE HINT SYSTEM                         │
├──────────────────────────────────────────────────────────┤
│                                                          │
│  Level 1: Concept & Direction                           │
│  ├─ Explain WHAT needs to be done                       │
│  ├─ Reference Template location                         │
│  └─ High-level approach                                 │
│                                                          │
│  Level 2: Structure & Pattern                           │
│  ├─ Show method signatures                              │
│  ├─ Explain pattern to use                              │
│  ├─ Partial code with //... placeholders                │
│  └─ Key points to remember                              │
│                                                          │
│  Level 3: Complete Example                              │
│  ├─ Full working code                                   │
│  ├─ Inline comments explaining each step                │
│  └─ Why this solution is correct                        │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

## Input

- Exercise name and level (1-4)
- Specific task (e.g., "Implement EventSpecifications.CheckDateTime")
- Template reference location

## Output Structure

### File Organization
```
hints/uebung-XX/
├─ 01-task-category/
│  ├─ hint-1-concept.md
│  ├─ hint-2-structure.md
│  ├─ hint-3-complete.md
│  └─ Partial_Code_EXAMPLE.cs (optional)
├─ 02-another-category/
│  └─ ...
└─ FAQ.md
```

## Hint Template

### hint-1-concept.md

```markdown
# Hint 1: [Task Name] - Konzept

## Problem
[What is the student trying to achieve?]

## Konzept
[Explain the concept at high level]

## Template Referenz
Schaue dir an:
```[path-to-template-file]```

[Brief explanation of what to look for]

## Nächste Schritte
- Schritt 1
- Schritt 2
- Schritt 3

## Weitere Hilfe?
- Hint 2: [Next hint preview]
- Hint 3: [Full solution preview]
```

### hint-2-structure.md

```markdown
# Hint 2: [Task Name] - Struktur & Pattern

## Pattern
[Explain the specific pattern to use]

## Methoden-Signatur
```csharp
public static [ReturnType] MethodName([Parameters])
{
    // TODO: Implementierung
}
```

## Schritt-für-Schritt
1. **[Step 1]**
   ```csharp
   // Beispiel:
   var trimmed = (input ?? string.Empty).Trim();
   ```

2. **[Step 2]**
   ```csharp
   // ... weitere Implementierung
   ```

## Wichtige Punkte
- ⚠️ [Important point 1]
- ⚠️ [Important point 2]

## Weitere Hilfe?
- Hint 3: Vollständiger Code
```

### hint-3-complete.md

```markdown
# Hint 3: [Task Name] - Vollständige Lösung

## Kompletter Code

```csharp
public static async Task<Entity> CreateAsync(
    string property,
    IEntityUniquenessChecker uc,
    CancellationToken ct = default)
{
    // 1. Trim input
    var trimmedProperty = (property ?? string.Empty).Trim();
    
    // 2. Internal validation
    ValidateEntityProperties(trimmedProperty);
    
    // 3. External validation
    await EntitySpecifications.ValidateEntityExternal(0, trimmedProperty, uc, ct);
    
    // 4. Create object
    return new Entity
    {
        Property = trimmedProperty
    };
}
```

## Erklärung

### Schritt 1: Trim
[Why trim]

### Schritt 2: Internal Validation
[Why internal first]

### Schritt 3: External Validation
[Why external after internal]

### Schritt 4: Object Creation
[Why only create if valid]

## Warum ist das richtig?
- [Reason 1]
- [Reason 2]

## Häufige Fehler
- ❌ [Common mistake 1]
- ❌ [Common mistake 2]
```

## Example Hint Sets

### 1. Domain Validation Task

**Task:** "Implement CheckDateTime validation"

**Hint 1:** Concept
- What: Date validation in domain layer
- Template: VenueSpecifications.CheckCapacity
- Approach: Return DomainValidationResult

**Hint 2:** Structure
```csharp
public static DomainValidationResult CheckDateTime(DateTime dateTime)
{
    if (/* condition */)
        return DomainValidationResult.Invalid("Property", "Message");
    
    return DomainValidationResult.Valid();
}
```

**Hint 3:** Complete code with explanation

### 2. Factory Method Task

**Task:** "Implement Event.CreateAsync"

**Hint 1:** Factory Method pattern explanation
**Hint 2:** Structure with validation order
**Hint 3:** Complete implementation

### 3. Command Handler Task

**Task:** "Implement CreateEventCommandHandler"

**Hint 1:** CQRS pattern + MediatR
**Hint 2:** Handler structure with UoW
**Hint 3:** Full handler with error handling

### 4. Repository Task

**Task:** "Add .Include() for Navigation Property"

**Hint 1:** EF Core Lazy Loading explanation
**Hint 2:** Include syntax
**Hint 3:** Complete override

## Common Issues Hints

### FAQ.md Structure

```markdown
# Häufige Probleme & Lösungen

## Navigation Property ist null
**Problem:** ...
**Lösung:** ...
**Siehe:** Hint für Repository Include

## Validation Exception beim Seeding
**Problem:** ...
**Lösung:** ...
**Siehe:** SeedDataUniquenessChecker

## Build-Fehler
**Problem:** ...
**Lösung:** ...

## Test schlägt fehl
**Problem:** ...
**Lösung:** ...
```

## Troubleshooting Hints

### TROUBLESHOOTING.md Structure

```markdown
# Fehlersuche Guide

## Compiler Errors

### "Cannot convert from X to Y"
[Common causes and fixes]

### "Object reference not set to an instance"
[Navigation Property issue?]

## Runtime Errors

### DomainValidationException
[Check validation logic]

### NotFoundException
[Check ID exists]

## Test Failures

### "Expected X but was Y"
[Check validation messages]

### "NullReferenceException in test"
[Check mock setup]
```

## Hint Creation Checklist

For each task:
- [ ] Hint 1: Concept clear and helpful?
- [ ] Hint 2: Structure shows pattern without solution?
- [ ] Hint 3: Complete code with good explanations?
- [ ] Template references correct?
- [ ] Progressive difficulty maintained?
- [ ] No solution spoilers in Hint 1 or 2?

## Commit Message

```
docs: Add progressive hints for [Exercise/Task]

📚 HINTS ERSTELLT:
- Hint Level 1: Concept & Direction
- Hint Level 2: Structure & Pattern
- Hint Level 3: Complete Solution

🎯 TASKS COVERED:
- [Task 1]
- [Task 2]
- [Task 3]

✅ FAQ und Troubleshooting included
```

---

**Version:** 1.0  
**Last Updated:** 2025-11-16

