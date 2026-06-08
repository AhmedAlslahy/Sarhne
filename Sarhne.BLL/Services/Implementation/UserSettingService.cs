using Sarhne.BLL.Abstraction;
using Sarhne.BLL.DTOs.UserSetting;
using Sarhne.BLL.Errors;
using Sarhne.BLL.Services.Interfaces;
using Sarhne.DAL.Repository.Interfaces;

namespace Sarhne.BLL.Services.Implementation
{
    public class UserSettingService(IUnitOfWork unitOfWork) : IUserSettingService
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<Result<UserSettingDto>> GetByUserId(string userId, CancellationToken cancellation = default)
        {
            var result = await unitOfWork.UserSettings.GetByUserIdAsync(userId, cancellation);
            if (result == null)
            {
                return Result<UserSettingDto>.Fail(UserErrors.NotFound);
            }
            var data = new UserSettingDto
            {
                AllowAnonymousMessages = result.AllowAnonymousMessages,
                ShowLastSeen = result.ShowLastSeen,
                ShowProfileViews = result.ShowProfileViews,
            };

            return Result<UserSettingDto>.Success(data);
        }

        public async Task<Result> Update(UpdateUserSettingDto dto, string userId, CancellationToken cancellation = default)
        {
            if (dto == null)
            {
                return UserErrors.InvalidSettingData;
            }
            var result = await unitOfWork.UserSettings.GetByUserIdAsync(userId);
            if (result == null)
            {
                return UserErrors.NotFound;
            }

            result.AllowAnonymousMessages = dto.AllowAnonymousMessages;
            result.ShowLastSeen = dto.ShowLastSeen;
            result.ShowProfileViews = dto.ShowProfileViews;

            await unitOfWork.SaveChangesAsync(cancellation);
            return Result.Success();
        }
    }
}