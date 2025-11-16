# Hint 1: Navigation Properties - Konzept

## Problem

Deine API gibt ein Buch zurück, aber `Author` ist immer `null`? Oder `AuthorName` ist leer?

```json
{
  "id": 1,
  "isbn": "9780747532699",
  "title": "Harry Potter",
  "authorId": 1,
  "authorName": null,  // ← PROBLEM!
  "publicationYear": 1997,
  "availableCopies": 5
}
```

## Was sind Navigation Properties?

Navigation Properties sind Beziehungen zwischen Entities:

```csharp
public class Book
{
    public int AuthorId { get; set; }           // ← Foreign Key
    public Author Author { get; set; } = null!; // ← Navigation Property
}
```

## Das Problem

**Entity Framework Core lädt Navigation Properties NICHT automatisch!**

Wenn du ein Book aus der DB holst, ist `Author` standardmäßig `null`, auch wenn `AuthorId` gesetzt ist.

## Die Lösung

Du musst EF Core **explizit sagen**, dass es die Navigation Property laden soll:

```csharp
.Include(b => b.Author)
```

## Wo muss das hin?

In deinem **Repository**! Genauer: In `BookRepository.cs`

## Template-Referenz

Schaue dir an:
```
CleanArchitecture_Template/Infrastructure/Persistence/Repositories/SensorRepository.cs
```

Dort siehst du, wie `.Include()` für `Measurements` verwendet wird.

## Nächste Schritte

1. Verstehe das `.Include()` Pattern
2. Finde heraus, welche Methoden in `BookRepository` du anpassen musst
3. Implementiere `.Include(b => b.Author)`

## Weiter mit Hint 2?

Hint 2 zeigt dir **genau**, welche Methoden du ändern musst und wie.

---

**Tipp:** Navigation Properties sind ein häufiger Fehler! Ohne `.Include()` sind sie IMMER null.

