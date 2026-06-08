namespace Sarhne.DAL.Entities;

public class UserSetting
{
    public int Id { get; set; }
    public bool AllowAnonymousMessages { get; set; } = true;
    public bool ShowLastSeen { get; set; } = true;
    public bool ShowProfileViews { get; set; } = true;

    //Relations
    public string UserId { get; set; } = string.Empty;

    public User User { get; set; } = null!;
}