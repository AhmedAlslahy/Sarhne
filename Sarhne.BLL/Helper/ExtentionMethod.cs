using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace Sarhne.BLL.Helper;

public static class ExtentionMethod
{
    public static IQueryable<MessageDetailsDto> GetAll(this IQueryable<Message> query, string userId)
    {
        return query.Where(m => m.ReceiverId == userId).Select(item => new MessageDetailsDto
        {
            Id = item.Id,
            IsRead = item.IsRead,
            Content = item.Content,
            CreatedAt = item.CreatedAt,
            IsStarred = item.IsStarred,
            PhotoUrl = item.PhotoUrl,
        }).AsNoTracking();
    }

    public static IQueryable<MessageDetailsDto> GetAllStarred(this IQueryable<Message> query, string userId)
    {
        return query.Where(m => m.ReceiverId == userId && m.IsStarred).Select(item => new MessageDetailsDto
        {
            Id = item.Id,
            IsRead = item.IsRead,
            Content = item.Content,
            CreatedAt = item.CreatedAt,
            IsStarred = item.IsStarred,
            PhotoUrl = item.PhotoUrl,
        }).AsNoTracking();
    }

    public static IQueryable<MessageDetailsDto> GetAllSender(this IQueryable<Message> query, string userId)
    {
        return query.Where(m => m.SenderId == userId).Select(item => new MessageDetailsDto
        {
            Id = item.Id,
            IsRead = item.IsRead,
            Content = item.Content,
            CreatedAt = item.CreatedAt,
            IsStarred = item.IsStarred,
            PhotoUrl = item.PhotoUrl,
        }).AsNoTracking();
    }

    public static IQueryable<T> ById<T>(this IQueryable<T> entity, int id) where T : BaseEntity<int>
    {
        return entity.Where(e => e.Id == id);
    }
}

public static class SeedExtensions
{
    public static async Task CreateAdminAsync(this UserManager<User> userManager
        , string fullName, string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is not null)
            return;

        user = new User
        {
            FullName = fullName,
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            UserSetting = new UserSetting
            {
                AllowAnonymousMessages = true,
                ShowLastSeen = true,
                ShowProfileViews = true
            }
        };

        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, "Admin");
    }

    public static async Task CreateRoleAsync(this RoleManager<IdentityRole> roleManager, string roleName)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}