

namespace Sarhne.DAL.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public string? PhotoUrl { get; set;}
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public bool IsStared { get; set; }


        //Relations
        public string ReceiverId { get; set; } = string.Empty;
        public User Receiver { get; set; } = null!;
    }
}
