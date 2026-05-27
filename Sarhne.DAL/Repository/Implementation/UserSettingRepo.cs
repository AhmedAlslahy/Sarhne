
using Microsoft.EntityFrameworkCore;
using Sarhne.DAL.Database;
using Sarhne.DAL.Entities;
using Sarhne.DAL.Repository.Interfaces;

namespace Sarhne.DAL.Repository.Implementation
{
    public class UserSettingRepo : IUserSettingRepo
    {
        SarhneDbContext _context;
        public UserSettingRepo(SarhneDbContext context)
        {
            _context = context;
        }

       public Task<UserSetting?> GetByUserIdAsync(string userId)
        {
            return _context.UserSettings.FirstOrDefaultAsync(x => x.UserId == userId);
        }
        public async Task UpdateAsync(UserSetting userSetting)
        {
            await _context.UserSettings.Where(n=>n.UserId==userSetting.UserId)
                .ExecuteUpdateAsync(setter => setter
                .SetProperty(n=>n.ShowProfileViews , userSetting.ShowProfileViews)
                .SetProperty(n => n.AllowAnonymousMessages , userSetting.AllowAnonymousMessages)
                .SetProperty(n => n.ShowLastSeen , userSetting.ShowLastSeen));
        }
    }
}
