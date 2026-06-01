
using Sarhne.DAL.Enums;

namespace Sarhne.BLL.DTOs.User
{
    public class UserDetailsDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public Gender? Gender { get; set; }
        public string? ProfileDescription { get; set; }
        public string? PhoneNumber { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = null;
        public string? PublicLink { get; set; } = null;
        public DateTime? LastSeen { get; set; }
        public int ProfileViewsCount { get; set; }
    }
}
