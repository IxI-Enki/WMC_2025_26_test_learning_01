# 🤖 AI Prompt: Generate New Exercise

## Context

You are creating a new Clean Architecture exercise for WMC students following the established patterns in the CleanArchitecture_Template.

## Input Required

1. **Exercise Level** (1-4)
   - Level 1: Maximum Guidance
   - Level 2: Structural Guidance
   - Level 3: Conceptual Guidance
   - Level 4: Exam Scenario

2. **Domain Description**
   - Business entities (2-3)
   - Relationships between entities
   - Validation rules
   - Business logic

3. **Learning Objectives**
   - What concepts should students learn?
   - Which patterns to apply?

## Output Structure

### 1. Domain Layer
```
Domain/
├─ Entities/
│  ├─ Entity1.cs (with Factory Method)
│  ├─ Entity2.cs
│  └─ Entity3.cs
├─ ValidationSpecifications/
│  ├─ Entity1Specifications.cs
│  ├─ Entity2Specifications.cs
│  └─ Entity3Specifications.cs
└─ Contracts/
   ├─ IEntity1UniquenessChecker.cs
   └─ ...
```

### 2. Application Layer
```
Application/
├─ Features/
│  ├─ Entity1/
│  │  ├─ Commands/ (Create, Update, Delete)
│  │  └─ Queries/ (GetAll, GetById)
│  └─ ...
├─ Dtos/
└─ Services/
```

### 3. Infrastructure Layer
```
Infrastructure/
├─ Persistence/
│  ├─ AppDbContext.cs
│  ├─ Repositories/
│  └─ UnitOfWork.cs
└─ Services/
   └─ StartupDataSeeder.cs
```

### 4. API Layer
```
Api/
└─ Controllers/
   ├─ Entity1Controller.cs
   └─ ...
```

### 5. CSV Seed Data
```csv
Property1;Property2;Property3
Value1;Value2;Value3
```

### 6. README.md
- Exercise description
- Learning objectives
- Requirements
- Validation rules
- API endpoints
- Testing instructions

## Scaffolding Rules by Level

### Level 1 (Maximum Guidance)
- ✅ All classes exist
- ✅ 1-2 entities completely implemented
- ✅ Interfaces defined
- ⚠️ `throw new NotImplementedException()` for TODO
- ✅ Very detailed README
- ✅ All tests present

### Level 2 (Structural Guidance)
- ✅ Complete folder structure
- ✅ Interfaces defined
- ⚠️ Classes: Only scaffolds
- ⚠️ Students create Commands/Queries
- ✅ Tests present
- ✅ Medium README

### Level 3 (Conceptual Guidance)
- ✅ README with requirements
- ✅ Tests as specification
- ⚠️ Students build everything
- ✅ Minimal README

### Level 4 (Exam Scenario)
- ✅ Only requirement document
- ⏱️ Time limit: 3-4 hours
- ⚠️ Complete implementation required

## Validation Checklist

- [ ] Domain validations in Factory methods
- [ ] External validation via IUniquenessChecker
- [ ] FluentValidation in Application layer
- [ ] Navigation properties with `= null!`
- [ ] Repository with `.Include()` for navigation
- [ ] CSV seeding with correct FK handling
- [ ] Result Pattern in Controllers
- [ ] CQRS separation
- [ ] Tests for all validation rules

## Example Domains

1. **Hotel Management**
   - Hotel, Room, Booking
   - Overlap validation for bookings

2. **Car Rental**
   - Vehicle, Customer, Rental
   - Date range validation

3. **Course Registration**
   - Course, Student, Enrollment
   - Capacity validation

4. **Inventory Management**
   - Product, Supplier, Order
   - Stock level validation

## Template Reference

ALWAYS refer to:
- `CleanArchitecture_Template/` for patterns
- `REPO__STRAT/EXERCISE_PROGRESSION.md` for level details
- `REPO__STRAT/MITSCHRIFTEN_VALIDIERUNG.md` for common pitfalls

## Quality Gates

- [ ] Follows Template patterns EXACTLY
- [ ] Scaffolding matches defined Level
- [ ] README is appropriate for Level
- [ ] CSV data is realistic and sufficient
- [ ] All validation layers present
- [ ] Tests compile and run
- [ ] Swagger UI works

---

**Version:** 1.0  
**Last Updated:** 2025-11-16

