using Sarhne.BLL.Abstraction;
using Sarhne.BLL.DTOs.UserSetting;

namespace Sarhne.BLL.Services.Interfaces;

public interface IUserSettingService
{
    Task<Result<UserSettingDto>> GetByUserId(string userId, CancellationToken cancellation = default);

    Task<Result> Update(UpdateUserSettingDto dto, string userId, CancellationToken cancellation = default);
}