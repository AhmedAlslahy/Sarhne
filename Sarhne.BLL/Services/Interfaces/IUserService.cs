
using Sarhne.BLL.Abstraction;
using Sarhne.BLL.DTOs.User;

namespace Sarhne.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<Response<IEnumerable<UserDetailsDto>>> GetAllAsync();
        Task<Response<UserDetailsDto>> GetByLinkAsync(string userId);
        Task<Response> UpdateAsync(UserUpdateDto dto);
        Task<Response> DeleteAsync(string userId);
        Task<Response> AddAdminRole(string userId);
    }
}
