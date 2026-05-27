

namespace Sarhne.DAL.Entities
{
    public class UserSetting
    {
        public int Id { get; set; }
        public bool AllowAnonymousMessages { get; set; }
        public bool ShowLastSeen { get; set; }
        public bool ShowProfileViews { get; set; }


        //Relations
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = null!;
    }
}
