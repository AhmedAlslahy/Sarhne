using Sarhne.BLL.Abstraction;
using Sarhne.BLL.DTOs.UserSetting;
using Sarhne.BLL.Errors;
using Sarhne.BLL.Services.Interfaces;
using Sarhne.DAL.Repository.Interfaces;


namespace Sarhne.BLL.Services.Implementation
{
    public class UserSettingService : IUserSettingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserSettingService(IUnitOfWork _unitOfWork)
        {
            this._unitOfWork = _unitOfWork;
        }

       public async Task<Response<UserSettingDto>> GetByUserId(string userId, CancellationToken cancellation = default)
        {
            var result = await _unitOfWork.UserSettings.GetByUserIdAsync(userId, cancellation);
            if (result == null)
            {
                return Response<UserSettingDto>.Fail(UserErrors.NotFound);
            }
            var data = new UserSettingDto
            {
                AllowAnonymousMessages = result.AllowAnonymousMessages,
                ShowLastSeen = result.ShowLastSeen,
                ShowProfileViews = result.ShowProfileViews,
            };

            return Response<UserSettingDto>.Success(data);
        }

       public async Task<Response> Update(UpdateUserSettingDto dto, string userId, CancellationToken cancellation = default)
        {
            if (dto == null)
            {
                return Response.Fail(UserErrors.InvalidSettingData);
            }
            var result = await _unitOfWork.UserSettings.GetByUserIdAsync(userId);
            if (result == null)
            {
                return Response.Fail(UserErrors.NotFound);
            }

            result.AllowAnonymousMessages = dto.AllowAnonymousMessages;
            result.ShowLastSeen = dto.ShowLastSeen;
            result.ShowProfileViews = dto.ShowProfileViews;

            _unitOfWork.UserSettings.Update(result);
            await _unitOfWork.SaveChangesAsync(cancellation);
            return Response.Success();
        } 
    }
}
