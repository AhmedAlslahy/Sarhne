using Microsoft.AspNetCore.Identity;
using Sarhne.BLL.Abstraction;
using Microsoft.EntityFrameworkCore;
using Sarhne.BLL.Errors;
using Sarhne.BLL.Services.Interfaces;
using Sarhne.DAL.Enums;
using static Sarhne.BLL.Abstraction.Errors;

namespace Sarhne.BLL.Services.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<Response<IEnumerable<IdentityRole>>> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            if (!roles.Any())
            {
                return Response<IEnumerable<IdentityRole>>
                    .Fail(RoleErrors.NotFound);
            }
            return Response<IEnumerable<IdentityRole>>.Success(roles);
        }

        public async Task<Response> CreateRoleAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return Response.Fail(RoleErrors.InvalidData);
            }

            if (await _roleManager.RoleExistsAsync(roleName))
            {
                return Response.Fail(RoleErrors.AlreadyExists);
            }

           var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Response.Fail(new Error("Create Failed", errors, ErrorType.BadRequest));
            }
            return Response.Success();
        }

        public async Task<Response> DeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                return Response.Fail(RoleErrors.NotFound);
            }

            var result = await _roleManager.DeleteAsync(role);
            return Response.Success();
        }
    }
}
