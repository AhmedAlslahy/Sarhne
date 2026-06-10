
namespace Sarhne.DAL.Entities;

public abstract class BaseEntity<T>
{
    public T Id { get; set; } = default!;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }
}
