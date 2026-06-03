using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sarhne.BLL.DTOs.User;
using Sarhne.BLL.DTOs.UserSetting;
using Sarhne.BLL.Services.Interfaces;
using System.Security.Claims;

namespace Sarhne.API.Controllers
{
    [Route("api/users")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserSettingService _userSettingService;
        public UserController(IUserService _userService, IUserSettingService _userSettingService)
        {
            this._userService = _userService;
            this._userSettingService = _userSettingService;
        }

        //------------------------- user --------------------------------------------
        [HttpGet("{publicLink}")]
        public async Task<IActionResult> GetByLink(string publicLink)
        {
            var result = await _userService.GetByLinkAsync(publicLink);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Failure);
            }
            return Ok(result.Data);
        }


        [HttpPut("")]
        public async Task<IActionResult> UpdateUser(UserUpdateDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }
            var result = await _userService.UpdateAsync(dto,userId);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Failure);
            }
            return Ok(result.IsSuccess);
        }

        //------------------------------- user setting ---------------------------------

        [HttpGet("setting")]
        public async Task<IActionResult> GetByUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }
            var result = await _userSettingService.GetByUserId(userId);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Failure);
            }
            return Ok(result.Data);
        }

        [HttpPut("setting")]
        public async Task<IActionResult> UpdateSetting(UpdateUserSettingDto dto,CancellationToken cancellation)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }
            var result = await _userSettingService.Update(dto,userId ,cancellation);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Failure);
            }
            return Ok(result.IsSuccess);
        }
    }
}
