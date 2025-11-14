namespace Domain.Entities;

/// <summary>
/// Basisklasse für alle Entitäten mit einer Id und Concurrency-Token.
/// </summary>
public abstract class BaseEntity : IBaseEntity
{
    public int Id { get; set; }
    public byte[] RowVersion { get; set; } = default!;
}
