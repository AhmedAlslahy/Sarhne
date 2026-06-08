using Microsoft.AspNetCore.Identity;
using Sarhne.BLL.Abstraction;

namespace Sarhne.BLL.Services.Interfaces;

public interface IRoleService
{
    Task<Result<IEnumerable<IdentityRole>>> GetAllRolesAsync();

    Task<Result> CreateRoleAsync(string roleName);

    Task<Result> DeleteRoleAsync(string roleId);
}