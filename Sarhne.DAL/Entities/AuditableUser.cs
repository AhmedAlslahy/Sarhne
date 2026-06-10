
namespace Sarhne.DAL.Entities;

public abstract class AuditableUser : IdentityUser
{
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } 
}