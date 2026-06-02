using Microsoft.AspNetCore.Identity;
using Sarhne.BLL.Abstraction;

namespace Sarhne.BLL.Services.Interfaces
{
    public interface IRoleService
    {
        Task<Response<IEnumerable<IdentityRole>>> GetAllRolesAsync();
        Task<Response> CreateRoleAsync(string roleName);
        Task<Response> DeleteRoleAsync(string roleId);
    }
}
