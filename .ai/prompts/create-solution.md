# 🤖 AI Prompt: Create Solution Branch

## Context

You are creating a complete, working solution for an existing exercise on a `solution/uebung-XX` branch.

## Input

- Exercise in `main` branch (student version)
- Level specification (1-4)
- Requirements from README

## Output

### 1. Complete Implementation

```
All TODO markers replaced with:
- ✅ Factory methods with validation
- ✅ All ValidationSpecifications
- ✅ All Commands + Handlers + Validators
- ✅ All Queries + Handlers
- ✅ DTOs complete
- ✅ UniquenessCheckers
- ✅ Repository methods
- ✅ Controller actions
- ✅ Tests (all green)
```

### 2. SOLUTION_NOTES.md

Create comprehensive solution notes:

```markdown
# 📚 [Exercise Name] - Lösungshinweise

## ✅ Vollständige Implementierung

### 1. Domain Layer
(Code examples for all entities)

### 2. Application Layer
(Code examples for Commands/Queries)

### 3. Infrastructure Layer
(Code examples for Repositories)

### 4. API Layer
(Code examples for Controllers)

## 🎓 Lern-Schwerpunkte

### 1. Key Concept 1
(Explanation + code example)

### 2. Key Concept 2
(Explanation + code example)

## 🐛 Häufige Fehler (und wie sie gelöst wurden)

### 1. Error Name
**Problem:** ...
**Lösung:** ...

## ✅ Erfolgskriterien (alle erfüllt)
- [x] ...
```

## Implementation Checklist

### Domain Layer
- [ ] All entities with Factory methods
- [ ] All ValidationSpecifications complete
- [ ] All Contracts (IUniquenessChecker)
- [ ] Navigation properties correct
- [ ] Domain exceptions used

### Application Layer
- [ ] All Commands (Create, Update, Delete)
- [ ] All CommandHandlers
- [ ] All CommandValidators (FluentValidation)
- [ ] All Queries (GetAll, GetById, custom)
- [ ] All QueryHandlers
- [ ] All DTOs
- [ ] All UniquenessChecker Services
- [ ] Mapster configuration (if needed)
- [ ] DependencyInjection registration

### Infrastructure Layer
- [ ] AppDbContext with OnModelCreating
- [ ] All Repositories (Generic + Specific)
- [ ] Repository overrides for Navigation Properties
- [ ] UnitOfWork
- [ ] StartupDataSeeder
- [ ] CSV seed data (realistic, sufficient)

### API Layer
- [ ] All Controllers
- [ ] All CRUD actions
- [ ] Result Pattern used
- [ ] ProducesResponseType attributes
- [ ] Swagger documentation

### Tests
- [ ] Domain tests (all green)
- [ ] Integration tests (optional but recommended)
- [ ] All validation scenarios covered

## Code Quality Standards

### 1. Factory Methods
```csharp
public static async Task<Entity> CreateAsync(
    params...,
    IEntityUniquenessChecker uc,
    CancellationToken ct = default)
{
    // 1. Trim
    var trimmedProperty = (property ?? string.Empty).Trim();
    
    // 2. Internal Validation
    ValidateEntityProperties(...);
    
    // 3. External Validation
    await EntitySpecifications.ValidateEntityExternal(0, key, uc, ct);
    
    // 4. Create
    return new Entity { ... };
}
```

### 2. Navigation Properties
```csharp
// Repository MUST include:
return await Set
    .Include(e => e.NavigationProperty)
    .FirstOrDefaultAsync(...);
```

### 3. CSV Seeding
```csharp
// Save parent entities FIRST:
await uow.Parents.AddAsync(parent, ct);
await uow.SaveChangesAsync(ct);  // ID generated!

// Then create children:
var child = await Child.CreateAsync(..., parent, ...);
```

### 4. Result Pattern
```csharp
[HttpPost]
public async Task<IActionResult> Create([FromBody] CreateCommand cmd, CancellationToken ct)
{
    var result = await mediator.Send(cmd, ct);
    return result.ToActionResult(this, 
        createdAtAction: nameof(GetById), 
        routeValues: new { id = result?.Value?.Id });
}
```

## Testing the Solution

### 1. Build
```bash
dotnet build
# Must succeed without warnings
```

### 2. Run Tests
```bash
cd Domain.Tests
dotnet test
# All must pass
```

### 3. API Testing (Swagger)
```bash
cd Api
dotnet run
# Browser: http://localhost:5100/swagger
```

Test scenarios:
- ✅ Create with valid data → 201 Created
- ✅ Create with invalid data → 400 Bad Request
- ✅ Create with duplicate key → 409 Conflict
- ✅ Get by existing ID → 200 OK with full DTO
- ✅ Get by non-existing ID → 404 Not Found
- ✅ Get all → 200 OK with collection
- ✅ Update with valid data → 200 OK
- ✅ Update with mismatched ID → 400 Bad Request
- ✅ Delete existing → 204 No Content
- ✅ Delete non-existing → 404 Not Found

### 4. CSV Seeding
- ✅ Database created on startup
- ✅ All seed data loaded
- ✅ Foreign keys correct
- ✅ Navigation properties work

## Documentation in SOLUTION_NOTES.md

### Must Include:
1. **Complete code examples** for all layers
2. **Learning focus points** with explanations
3. **Common mistakes** and solutions
4. **Architecture highlights**
5. **Success criteria checklist**

### Code Example Format:
```markdown
### Entity.cs Factory Method

```csharp
public static async Task<Entity> CreateAsync(...)
{
    // Explanation comments inline
}
```

**💡 Wichtige Konzepte:**
- Concept 1 explanation
- Concept 2 explanation
```

## Commit Message Format

```
solution: Complete [Exercise Name] implementation

✅ IMPLEMENTIERT:
- Domain: All entities with Factory methods
- Application: Full CRUD for all entities
- Infrastructure: CSV seeding with [X] records
- API: All endpoints Swagger-tested

💡 HIGHLIGHTS:
- [Key concept 1]
- [Key concept 2]

🧪 TESTS: All green
📝 DOCUMENTATION: SOLUTION_NOTES.md (XXX lines)
```

## Quality Gates Before Push

- [ ] `dotnet build` success (no warnings)
- [ ] All tests green
- [ ] Swagger UI works
- [ ] CSV seeding works
- [ ] All DTOs have correct data
- [ ] Navigation properties loaded
- [ ] SOLUTION_NOTES.md complete
- [ ] No `NotImplementedException` remaining
- [ ] Code follows Template patterns

---

**Version:** 1.0  
**Last Updated:** 2025-11-16

