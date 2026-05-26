
namespace Sarhne.DAL.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }

        //Relations
        public string ReceiverId { get; set; } = string.Empty;
        public User Receiver { get; set; } = null!;
    }
}
