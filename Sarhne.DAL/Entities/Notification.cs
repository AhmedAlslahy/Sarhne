using System.ComponentModel.DataAnnotations;

namespace Sarhne.DAL.Entities;

public class Notification
{
    public int Id { get; set; }

    [MaxLength(50)]
    public required string Title { get; set; }

    [MaxLength(100)]
    public string Body { get; set; } = string.Empty;

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public bool IsRead { get; set; } = false;

    //Relations
    public string ReceiverId { get; set; } = string.Empty;

    public User Receiver { get; set; } = null!;
    public string SenderId { get; set; } = string.Empty;
    public User Sender { get; set; } = null!;
}