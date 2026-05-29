

namespace Sarhne.BLL.DTOs.Message
{
    public class MessageDetailsDto
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public string? PhotoUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public bool IsStarred { get; set; }
        public string ReceiverId { get; set; } = string.Empty;
    }
}
