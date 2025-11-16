using Domain.Common;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.ValidationSpecifications;

namespace Domain.Entities;

/// <summary>
/// [Entity Description]
/// </summary>
public class [ENTITY_NAME] : BaseEntity
{
    // ============================================================================
    // PROPERTIES
    // ============================================================================
    
    public string [Property1] { get; set; } = string.Empty;
    public int [Property2] { get; set; }
    public DateTime [Property3] { get; set; }
    
    // Navigation Properties (if any)
    public [ParentEntity] [ParentEntity] { get; set; } = null!;  // Required
    public int [ParentEntity]Id { get; set; }
    
    // Collection (if any)
    public ICollection<[ChildEntity]> [ChildEntities] { get; set; } = new List<[ChildEntity]>();
    
    // ============================================================================
    // FACTORY METHOD
    // ============================================================================
    
    /// <summary>
    /// Erstellt eine neue [ENTITY_NAME] Instanz mit Validierung.
    /// </summary>
    /// <param name="[param1]">[Description]</param>
    /// <param name="[param2]">[Description]</param>
    /// <param name="uc">Uniqueness Checker für externe Validierung</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Validierte [ENTITY_NAME] Instanz</returns>
    /// <exception cref="DomainValidationException">Wenn Validierung fehlschlägt</exception>
    public static async Task<[ENTITY_NAME]> CreateAsync(
        string [param1],
        int [param2],
        [ParentEntity] [parentEntity],
        I[ENTITY_NAME]UniquenessChecker uc,
        CancellationToken ct = default)
    {
        // 1. TRIM & PREPARE
        var trimmed[Param1] = ([param1] ?? string.Empty).Trim();
        
        // 2. INTERNAL VALIDATION (Domain Rules)
        Validate[ENTITY_NAME]Properties(trimmed[Param1], [param2], [parentEntity]);
        
        // 3. EXTERNAL VALIDATION (Uniqueness)
        await [ENTITY_NAME]Specifications.Validate[ENTITY_NAME]External(
            0, trimmed[Param1], uc, ct);
        
        // 4. CREATE OBJECT (only if valid!)
        return new [ENTITY_NAME]
        {
            [Property1] = trimmed[Param1],
            [Property2] = [param2],
            [ParentEntity] = [parentEntity]
            // EF Core sets [ParentEntity]Id automatically via navigation property
        };
    }
    
    // ============================================================================
    // UPDATE METHOD
    // ============================================================================
    
    /// <summary>
    /// Aktualisiert die [ENTITY_NAME] Instanz mit Validierung.
    /// </summary>
    public async Task UpdateAsync(
        string [param1],
        int [param2],
        I[ENTITY_NAME]UniquenessChecker uc,
        CancellationToken ct = default)
    {
        // 1. TRIM & PREPARE
        var trimmed[Param1] = ([param1] ?? string.Empty).Trim();
        
        // 2. INTERNAL VALIDATION
        Validate[ENTITY_NAME]Properties(trimmed[Param1], [param2], [ParentEntity]);
        
        // 3. EXTERNAL VALIDATION (with existing ID)
        await [ENTITY_NAME]Specifications.Validate[ENTITY_NAME]External(
            Id, trimmed[Param1], uc, ct);
        
        // 4. UPDATE PROPERTIES
        [Property1] = trimmed[Param1];
        [Property2] = [param2];
    }
    
    // ============================================================================
    // PRIVATE VALIDATION HELPER
    // ============================================================================
    
    private static void Validate[ENTITY_NAME]Properties(
        string [param1],
        int [param2],
        [ParentEntity] [parentEntity])
    {
        [ENTITY_NAME]Specifications.Validate[ENTITY_NAME]Internal(
            [param1], [param2], [parentEntity].Id);
    }
}

