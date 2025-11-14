namespace Domain.Entities;

public interface IBaseEntity
{
    int Id { get; set; }
    byte[] RowVersion { get; set; }
}
