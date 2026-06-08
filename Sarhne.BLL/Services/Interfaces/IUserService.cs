using Sarhne.BLL.Abstraction;
using Sarhne.BLL.DTOs.User;

namespace Sarhne.BLL.Services.Interfaces;

public interface IUserService
{
    Task<Result<IEnumerable<UserDetailsDto>>> GetAllAsync();

    Task<Result<UserDetailsDto>> GetByLinkAsync(string publicLink);

    Task<Result> UpdateAsync(UserUpdateDto dto, string userId);

    Task<Result> DeleteAsync(string userId);

    Task<Result> AddAdminRole(string userId);
}