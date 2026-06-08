using Microsoft.EntityFrameworkCore;
using Sarhne.DAL.Database;
using Sarhne.DAL.Entities;
using Sarhne.DAL.Repository.Interfaces;

namespace Sarhne.DAL.Repository.Implementation;

public class UserSettingRepo(SarhneDbContext context) : IUserSettingRepo
{
    public async Task<UserSetting?> GetByUserIdAsync(string userId, CancellationToken cancellation = default)
    {
        return await context.UserSettings.FirstOrDefaultAsync(x => x.UserId == userId, cancellation);
    }
}