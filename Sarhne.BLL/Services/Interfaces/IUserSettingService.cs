
using Sarhne.BLL.Abstraction;
using Sarhne.BLL.DTOs.UserSetting;

namespace Sarhne.BLL.Services.Interfaces
{
    public interface IUserSettingService
    {
        Task<Response<UserSettingDto>> GetByUserId(string userId, CancellationToken cancellation = default);
        Task <Response> Update(UpdateUserSettingDto dto, string userId, CancellationToken cancellation = default);
    }
}
