# 🤖 AI-Workspace Branch

## Purpose

This branch contains all AI-specific resources, templates, prompts, and references needed for efficient repository management and exercise creation.

**❗ THIS BRANCH IS NOT FOR STUDENTS!**

---

## Structure

```ascii
ai-workspace/
├─ .ai/
│  ├─ prompts/              ← AI Prompts for common tasks
│  ├─ templates/            ← Code templates (Entity, Command, etc.)
│  ├─ checklists/           ← Quality checklists
│  └─ workflows/            ← Step-by-step workflows
│
├─ references/              ← Reference materials
│  ├─ CleanArchitecture_Template/
│  ├─ FruehereAufgabenstellungen/
│  ├─ Mitschriften_Der_Kollegen/
│  └─ REPO__STRAT/
│
├─ scripts/                 ← Automation scripts
│  ├─ create-exercise.ps1
│  ├─ validate-solution.ps1
│  └─ generate-hints.ps1
│
├─ docs/                    ← Documentation
│  ├─ architecture-decisions.md
│  ├─ naming-conventions.md
│  └─ validation-layers.md
│
└─ README.md               ← This file
```

---

## 📁 Directory Details

### `.ai/prompts/`

AI prompts for common tasks:

| File | Purpose |
|------|---------|
| `generate-exercise.md` | Create new exercise from concept |
| `create-solution.md` | Implement complete solution |
| `create-hints.md` | Generate progressive hint system |

**Usage:**
```
When asking AI to create a new exercise:
"Please create a new exercise following @generate-exercise.md"
```

### `.ai/templates/`

Code templates with placeholders:

| File | Purpose |
|------|---------|
| `entity-template.cs` | Domain Entity with Factory Method |
| `command-template.cs` | CQRS Command definition |
| `command-handler-template.cs` | Command Handler |
| `validator-template.cs` | FluentValidation Validator |
| `query-template.cs` | CQRS Query definition |
| `query-handler-template.cs` | Query Handler |
| `controller-template.cs` | REST API Controller |

**Usage:**
Replace placeholders like `[ENTITY_NAME]`, `[PROPERTY]`, etc.

### `.ai/checklists/`

Quality assurance checklists:

| File | Purpose |
|------|---------|
| `exercise-quality.md` | Quality gates for exercises |
| `code-review.md` | Code review checklist |
| `test-coverage.md` | Testing completeness |

**Usage:**
Go through before committing to main or solution branches.

### `.ai/workflows/`

Step-by-step workflows:

| File | Purpose |
|------|---------|
| `new-exercise-workflow.md` | Complete workflow from concept to student version |
| `solution-workflow.md` | Create solution branch |
| `hints-workflow.md` | Create progressive hints |

**Usage:**
Follow step-by-step when creating new content.

### `references/`

Reference materials (copied from dev branch):

| Directory | Content |
|-----------|---------|
| `CleanArchitecture_Template/` | **OBERSTE AUTORITÄT** - Pattern reference |
| `FruehereAufgabenstellungen/` | Previous exercise examples |
| `Mitschriften_Der_Kollegen/` | Student notes (validated) |
| `REPO__STRAT/` | Repository strategy docs |

**Usage:**
Always consult Template for patterns!

### `scripts/`

Automation scripts (PowerShell):

| File | Purpose |
|------|---------|
| `create-exercise.ps1` | Scaffold new exercise structure |
| `validate-solution.ps1` | Run all quality checks |
| `generate-hints.ps1` | Generate hint structure |

**Usage:**
```powershell
./scripts/create-exercise.ps1 -Name "Hotel Management" -Level 2
```

### `docs/`

Additional documentation:

| File | Purpose |
|------|---------|
| `architecture-decisions.md` | ADRs for important choices |
| `naming-conventions.md` | Consistent naming rules |
| `validation-layers.md` | 3-layer validation explained |

---

## 🎯 Common Tasks

### Create New Exercise

1. **Checkout ai-workspace:**
   ```powershell
   git checkout ai-workspace
   ```

2. **Read prompt:**
   ```powershell
   code .ai/prompts/generate-exercise.md
   ```

3. **Consult Template:**
   ```powershell
   code references/CleanArchitecture_Template/
   ```

4. **Switch to dev and implement:**
   ```powershell
   git checkout dev
   # Create exercise following workflow
   ```

5. **Use checklist:**
   ```powershell
   # Before commit:
   code .ai/checklists/exercise-quality.md
   ```

### Create Solution

1. **Read prompt:**
   ```powershell
   git checkout ai-workspace
   code .ai/prompts/create-solution.md
   ```

2. **Create solution branch:**
   ```powershell
   git checkout dev
   git checkout -b solution/uebung-XX
   ```

3. **Implement & document:**
   ```
   - Complete all TODOs
   - Create SOLUTION_NOTES.md
   - Run all tests
   ```

4. **Review:**
   ```powershell
   # Check against:
   code .ai/checklists/code-review.md
   ```

### Create Hints

1. **Read prompt:**
   ```powershell
   git checkout ai-workspace
   code .ai/prompts/create-hints.md
   ```

2. **Switch to hints branch:**
   ```powershell
   git checkout support/hints
   ```

3. **Create 3-level hints:**
   ```
   hints/uebung-XX/
   ├─ hint-1-concept.md
   ├─ hint-2-structure.md
   └─ hint-3-complete.md
   ```

---

## 🔑 Key Principles

### 1. Template is Authority
```
❗ ALWAYS refer to CleanArchitecture_Template/
```

### 2. Validation in Factory
```csharp
// ✅ CORRECT:
public static async Task<Entity> CreateAsync(...)
{
    ValidateProperties(...);     // ← IMMEDIATELY
    await ValidateExternal(...);  // ← IMMEDIATELY
    return new Entity { ... };
}
```

### 3. Navigation Properties
```csharp
// ✅ Repository MUST include:
return await Set
    .Include(e => e.NavigationProperty)
    .FirstOrDefaultAsync(...);
```

### 4. CSV Seeding
```csharp
// ✅ Save parents FIRST:
await uow.Parents.AddAsync(parent, ct);
await uow.SaveChangesAsync(ct);  // ID generated!
```

---

## 📚 Important References

### From REPO__STRAT/

| Document | Purpose |
|----------|---------|
| `GITHUB_REPOSITORY_STRAT.md` | Branch strategy |
| `BRANCH_MANAGEMENT.md` | Branch workflows |
| `EXERCISE_PROGRESSION.md` | Level 1-4 definitions |
| `MITSCHRIFTEN_VALIDIERUNG.md` | Validated student notes |

### From Template

| Location | Pattern |
|----------|---------|
| `Domain/Entities/Sensor.cs` | Factory Method |
| `Application/Features/Sensors/` | CQRS Structure |
| `Infrastructure/Persistence/` | Repository Pattern |
| `Api/Controllers/` | REST API Pattern |

---

## 🚀 Getting Started

### First Time Setup

1. **Clone this branch:**
   ```powershell
   git clone <repo-url>
   git checkout ai-workspace
   ```

2. **Read all prompts:**
   ```powershell
   cat .ai/prompts/*.md
   ```

3. **Study Template:**
   ```powershell
   code references/CleanArchitecture_Template/
   ```

4. **Review checklists:**
   ```powershell
   cat .ai/checklists/*.md
   ```

### Regular Usage

```powershell
# Switch to ai-workspace to consult resources:
git checkout ai-workspace
code .ai/prompts/[relevant-prompt].md

# Then switch to working branch:
git checkout dev  # or solution/*, support/hints
```

---

## 🔄 Keeping Updated

### Update References from dev

```powershell
git checkout ai-workspace
git checkout dev -- REPO__STRAT/
mv REPO__STRAT references/
git add references/REPO__STRAT/
git commit -m "docs: Update REPO__STRAT references"
git push origin ai-workspace
```

### Add New Templates

```powershell
git checkout ai-workspace
# Create new template in .ai/templates/
git add .ai/templates/new-template.cs
git commit -m "feat: Add new template for X"
git push origin ai-workspace
```

---

## 📊 Statistics

**Current Content:**
- ✅ 3 AI Prompts
- ✅ 7 Code Templates
- ✅ 2 Quality Checklists
- ✅ 1 Workflow
- ✅ Reference materials
- ⚠️ Scripts (TODO)
- ⚠️ Additional docs (TODO)

---

## 🤝 Contributing

When adding to ai-workspace:

1. **Follow naming conventions**
2. **Document purpose clearly**
3. **Reference Template patterns**
4. **Test templates before committing**
5. **Update this README**

---

## ⚠️ Important Notes

- **This branch is orphan** (no commit history from main/dev)
- **Not for students!**
- **Always sync with Template changes**
- **Keep prompts updated**
- **Validate checklists regularly**

---

**Branch:** ai-workspace (orphan)  
**Purpose:** AI & Management Resources  
**Status:** ✅ Active  
**Version:** 1.0  
**Last Updated:** 2025-11-16

