
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

       public async Task<UserSetting?> GetByUserIdAsync(string userId, CancellationToken cancellation = default)
        {
            return await _context.UserSettings.FirstOrDefaultAsync(x => x.UserId == userId, cancellation);
        }
        public void Update(UserSetting userSetting)
        {
            _context.UserSettings.Update(userSetting);
        }
    }
}
