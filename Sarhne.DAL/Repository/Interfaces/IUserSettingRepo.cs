

using Sarhne.DAL.Entities;

namespace Sarhne.DAL.Repository.Interfaces
{
    public interface IUserSettingRepo
    {
        Task<UserSetting> GetByUserIdAsync(string userId);
        Task UpdateAsync(UserSetting userSetting);
    }
}
