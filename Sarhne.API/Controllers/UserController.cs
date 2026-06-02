using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sarhne.BLL.DTOs.User;
using Sarhne.BLL.DTOs.UserSetting;
using Sarhne.BLL.Services.Interfaces;

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


        [HttpPut("{userId}")]
        public async Task<IActionResult> GetByLink(UserUpdateDto dto)
        {
            var result = await _userService.UpdateAsync(dto);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Failure);
            }
            return Ok(result.IsSuccess);
        }

        //------------------------------- user setting ---------------------------------

        [HttpGet("setting/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var result = await _userSettingService.GetByUserId(userId);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Failure);
            }
            return Ok(result.Data);
        }

        [HttpPut("setting/{userId}")]
        public async Task<IActionResult> UpdateSetting(UpdateUserSettingDto dto,string userId ,CancellationToken cancellation)
        {
            var result = await _userSettingService.Update(dto,userId ,cancellation);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Failure);
            }
            return Ok(result.IsSuccess);
        }
    }
}
