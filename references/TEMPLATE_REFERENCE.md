# 📚 CleanArchitecture_Template Reference

## ❗ OBERSTE AUTORITÄT

Das `CleanArchitecture_Template` ist die **oberste Autorität** für alle Pattern und Implementierungen.

---

## Location

```
Branch: dev (oder main)
Path: CleanArchitecture_Template/
```

**Zugriff:**
```powershell
git checkout dev
code CleanArchitecture_Template/
```

---

## Wichtige Referenz-Dateien

### Domain Layer

| Datei | Pattern | Wichtig für |
|-------|---------|-------------|
| `Domain/Entities/Sensor.cs` | Factory Method mit Validation | Entity-Erstellung |
| `Domain/ValidationSpecifications/SensorSpecifications.cs` | Domain Validation | Validation-Logik |
| `Domain/Contracts/ISensorUniquenessChecker.cs` | External Validation | Uniqueness-Checks |
| `Domain/Exceptions/DomainValidationException.cs` | Domain Exceptions | Error Handling |

**Key Concepts:**
- ✅ Factory-Methoden sind `static async Task<Entity>`
- ✅ Validation SOFORT in Factory
- ✅ Internal vor External Validation
- ✅ DomainValidationResult Pattern

### Application Layer

| Datei | Pattern | Wichtig für |
|-------|---------|-------------|
| `Application/Features/Sensors/Commands/CreateSensor/` | CQRS Command | Command Structure |
| `Application/Features/Sensors/Commands/UpdateSensor/` | Update Pattern | Update-Logik |
| `Application/Features/Sensors/Queries/GetAllSensors/` | Query Pattern | Read Operations |
| `Application/Dtos/GetSensorDto.cs` | DTO Pattern | API Responses |
| `Application/Services/SensorUniquenessChecker.cs` | Domain Service | External Validation |
| `Application/Pipeline/ValidationBehavior.cs` | MediatR Pipeline | FluentValidation |

**Key Concepts:**
- ✅ Commands ändern Daten
- ✅ Queries lesen nur
- ✅ Handler haben Single Responsibility
- ✅ FluentValidation in Validators
- ✅ DTOs statt Entities zurückgeben

### Infrastructure Layer

| Datei | Pattern | Wichtig für |
|-------|---------|-------------|
| `Infrastructure/Persistence/AppDbContext.cs` | EF Core Context | DB Configuration |
| `Infrastructure/Persistence/Repositories/GenericRepository.cs` | Generic Repository | Base CRUD |
| `Infrastructure/Persistence/Repositories/SensorRepository.cs` | Specific Repository | Custom Queries |
| `Infrastructure/Persistence/UnitOfWork.cs` | Unit of Work | Transactions |
| `Infrastructure/Services/StartupDataSeeder.cs` | CSV Seeding | Data Import |

**Key Concepts:**
- ✅ Repository Pattern mit .Include()
- ✅ AsNoTracking() bei Read-Only
- ✅ UnitOfWork für Transaktionen
- ✅ CSV-basierter Seeder

### API Layer

| Datei | Pattern | Wichtig für |
|-------|---------|-------------|
| `Api/Controllers/SensorsController.cs` | REST Controller | API Endpoints |
| `Api/Extensions/ResultExtensions.cs` | Result Pattern | HTTP Status Codes |
| `Api/Middleware/ExceptionMiddleware.cs` | Exception Handling | Error Responses |
| `Api/Program.cs` | DI Configuration | Startup |

**Key Concepts:**
- ✅ Controller nur Koordination
- ✅ MediatR für Business Logic
- ✅ Result Pattern für Responses
- ✅ HTTP Status Codes korrekt

---

## Pattern Cheat Sheet

### Factory Method Pattern

```csharp
// FROM: Domain/Entities/Sensor.cs
public static async Task<Sensor> CreateAsync(
    string name,
    string location,
    ISensorUniquenessChecker uc,
    CancellationToken ct = default)
{
    // 1. TRIM
    var trimmedName = (name ?? string.Empty).Trim();
    var trimmedLocation = (location ?? string.Empty).Trim();
    
    // 2. INTERNAL VALIDATION
    ValidateSensorProperties(trimmedName, trimmedLocation);
    
    // 3. EXTERNAL VALIDATION
    await SensorSpecifications.ValidateSensorExternal(0, trimmedName, uc, ct);
    
    // 4. CREATE
    return new Sensor
    {
        Name = trimmedName,
        Location = trimmedLocation
    };
}
```

### Command Handler Pattern

```csharp
// FROM: Application/Features/Sensors/Commands/CreateSensor/CreateSensorCommandHandler.cs
public async Task<Result<GetSensorDto>> Handle(
    CreateSensorCommand request,
    CancellationToken cancellationToken)
{
    // 1. CREATE VIA FACTORY
    var entity = await Sensor.CreateAsync(
        request.Name,
        request.Location,
        _uniquenessChecker,
        cancellationToken);
    
    // 2. PERSIST
    await _uow.Sensors.AddAsync(entity, cancellationToken);
    await _uow.SaveChangesAsync(cancellationToken);
    
    // 3. RETURN DTO
    return Result<GetSensorDto>.Created(entity.Adapt<GetSensorDto>());
}
```

### Repository Pattern with Navigation

```csharp
// FROM: Infrastructure/Persistence/Repositories/SensorRepository.cs
public override async Task<Sensor?> GetByIdAsync(int id, CancellationToken ct = default)
{
    return await Set
        .Include(s => s.Measurements)  // ← IMPORTANT!
        .FirstOrDefaultAsync(s => s.Id == id, ct);
}
```

### CSV Seeding Pattern

```csharp
// FROM: Infrastructure/Services/StartupDataSeeder.cs
var lines = await File.ReadAllLinesAsync(_csvPath, cancellationToken);

for (int i = 1; i < lines.Length; i++)  // Skip header
{
    var parts = lines[i].Split(';');
    
    // Create entity
    var entity = await Entity.CreateAsync(
        parts[0], parts[1], uniquenessChecker, cancellationToken);
    
    // Add to collection
    entities.Add(entity);
}

// Bulk insert
await _uow.Entities.AddRangeAsync(entities, cancellationToken);
await _uow.SaveChangesAsync(cancellationToken);
```

---

## Validation Layers (3-Ebenen)

### 1. Domain Validation (Internal)
```csharp
// Grundregeln, immer gültig
SensorSpecifications.ValidateSensorInternal(name, location);
```

### 2. FluentValidation (Application)
```csharp
// UseCase-spezifische Regeln
public class CreateSensorCommandValidator : AbstractValidator<CreateSensorCommand>
{
    RuleFor(x => x.Name).NotEmpty().MinimumLength(2);
}
```

### 3. External Validation (via Interface)
```csharp
// Datenbank-Checks (Uniqueness)
await SensorSpecifications.ValidateSensorExternal(0, name, uc, ct);
```

---

## Important Decisions from Template

### ✅ Always Trim Input
```csharp
var trimmed = (input ?? string.Empty).Trim();
```

### ✅ Validation BEFORE Object Creation
```csharp
ValidateProperties(...);           // Internal
await ValidateExternal(...);        // External
return new Entity { ... };          // Only if valid!
```

### ✅ Navigation Properties with .Include()
```csharp
return await Set
    .Include(e => e.NavigationProperty)
    .FirstOrDefaultAsync(...);
```

### ✅ Result Pattern
```csharp
Result<T>.Success(data)      → 200 OK
Result<T>.Created(data)      → 201 Created
Result<T>.NoContent()        → 204 No Content
Result<T>.NotFound(msg)      → 404 Not Found
Result<T>.Conflict(msg)      → 409 Conflict
```

---

## CSV Format

```csv
Property1;Property2;Property3;Property4
Value1;Value2;Value3;Value4
```

**Rules:**
- Semicolon (`;`) separator
- Header row (line 1)
- No quotes
- Trim values
- Handle Foreign Keys (parents first!)

---

## Naming Conventions

### Entities
- Singular: `Sensor`, `Measurement`
- PascalCase

### Collections
- Plural: `Sensors`, `Measurements`
- PascalCase

### Commands
- Verb + Entity: `CreateSensorCommand`
- PascalCase

### Queries
- Get + Description: `GetAllSensorsQuery`
- PascalCase

### DTOs
- Get + Entity + Dto: `GetSensorDto`
- PascalCase

### Interfaces
- I + Purpose: `ISensorUniquenessChecker`
- PascalCase

---

## Common Mistakes (from Template)

### ❌ Validation nach Factory
```csharp
// FALSCH:
var entity = new Entity();
ValidateEntity(entity);
```

### ❌ Foreign Key manuell setzen
```csharp
// FALSCH:
return new Book
{
    AuthorId = author.Id  // Kann 0 sein!
};
```

### ❌ Navigation Property nicht laden
```csharp
// FALSCH:
return await Set.FirstOrDefaultAsync(...);
// NavigationProperty ist null!
```

---

## References in Exercises

**ALWAYS mention in README:**
```markdown
## Template-Referenz

Orientiere dich am CleanArchitecture_Template:
- Factory-Methoden: `Domain/Entities/Sensor.cs`
- Commands: `Application/Features/Sensors/Commands/`
- Repository: `Infrastructure/Persistence/Repositories/`
```

---

## When Template Changes

1. **Review changes:**
   ```powershell
   git checkout dev
   git log --oneline CleanArchitecture_Template/
   ```

2. **Update exercises accordingly**

3. **Update this reference**

4. **Update REPO__STRAT docs**

---

**Status:** ✅ Template ist maßgeblich  
**Location:** dev branch  
**Last Checked:** 2025-11-16  
**Version:** 1.0

