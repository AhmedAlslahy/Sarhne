using System.ComponentModel.DataAnnotations;

namespace Sarhne.DAL.Entities;

public class Message
{
    public int Id { get; set; }

    [MaxLength(200)]
    public string? Content { get; set; }

    [MaxLength(500)]
    public string? PhotoUrl { get; set; }

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public bool IsRead { get; set; } = false;
    public bool IsStarred { get; set; } = false;

    //Relations
    public string ReceiverId { get; set; } = string.Empty;

    public User Receiver { get; set; } = null!;
    public string SenderId { get; set; } = string.Empty;
    public User Sender { get; set; } = null!;
}