using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sarhne.BLL.DTOs.Notification;
using Sarhne.BLL.Services.Interfaces;

namespace Sarhne.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        public AdminController(IRoleService _roleService, INotificationService _notificationService, IUserService _userService)
        {
            this._roleService = _roleService;
            this._notificationService = _notificationService;
            this._userService = _userService;
        }

        //--------------------- Role ----------------------------------------------
        [HttpGet("roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await _roleService.GetAllRolesAsync();
            if (!result.IsSuccess)
            {
                return BadRequest(result.Failure);
            }
            return Ok(result.Data);
        }

        [HttpPost("roles")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var result = await _roleService.CreateRoleAsync(roleName);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Failure);
            }
            return Ok(result.IsSuccess);
        }

        [HttpDelete("roles/{roleId}")]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var result = await _roleService.DeleteRoleAsync(roleId);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Failure);
            }
            return Ok(result.IsSuccess);
        }

        [HttpPost("roles/{userId}")]
        public async Task<IActionResult> AddAdminRole(string userId)
        {
            var result = await _userService.AddAdminRole(userId);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Failure);
            }
            return Ok(result.IsSuccess);
        }

        //-------------------------------  notification   -----------------------------------------

        [HttpPost("notifications")]
        public async Task<IActionResult> CreateNotification(CreateNotificationDto dto)
        {
            var result = await _notificationService.Create(dto);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Failure);
            }
            return Ok(result.IsSuccess);
        }

        //---------------------------- user ----------------------------------------------------------

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllAsync();
            if (!result.IsSuccess)
            {
                return BadRequest(result.Failure);
            }
            return Ok(result.Data);
        }

        [HttpDelete("users/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _userService.DeleteAsync(userId);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Failure);
            }
            return Ok(result.IsSuccess);
        }
    }
}