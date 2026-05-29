

namespace Sarhne.BLL.DTOs.Notification
{
    public class CreateNotificationDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Body { get; set; }
        public string UserId { get; set; }
    }
}
