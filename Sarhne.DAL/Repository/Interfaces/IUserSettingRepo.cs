namespace Sarhne.DAL.Repository.Interfaces;

public interface IUserSettingRepo
{
    Task<UserSetting?> GetByUserIdAsync(string userId, CancellationToken cancellation = default);
}