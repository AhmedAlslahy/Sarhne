
global using Microsoft.AspNetCore.Identity;
using static Sarhne.DAL.Enums.Enums;
namespace Sarhne.DAL.Entities
{
    public class User : IdentityUser
    {
        public string? FullName { get; set; }
        public Gender? Gender {  get; set;}
        public string? ImageUrl { get; set; }
        public string? PublicLink { get; set; }
        public string? ProfileDescription { get; set; }
        public DateTime? LastSeen { get; set; }
        public int ProfileViewsCount { get; set; }

        //Relations
        public UserSetting UserSetting { get; set; } = null!;
        public ICollection<Message> ReceivedMessages { get; set; } = new HashSet<Message>();
        public ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
    }
}
