using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sarhne.BLL.Services.Interfaces;

namespace Sarhne.API.Controllers
{
    [Route("api/messages")]
    [Authorize]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        public MessageController(IMessageService _messageService)
        {
            this._messageService = _messageService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAllByUserId(string userId)
        {
            var result = await _messageService.GetAllByUserId(userId);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Failure);
            }
            return Ok(result.Data);
        }

        [HttpGet("all-starred/{userId}")]
        public async Task<IActionResult> GetAllStarredByUserId(string userId)
        {
            var result = await _messageService.GetAllStarredByUserId(userId);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Failure);
            }
            return Ok(result.Data);
        }

        [HttpGet("all-unread/{userId}")]
        public async Task<IActionResult> GetAllUnreadByUserId(string userId)
        {
            var result = await _messageService.GetAllUnreadByUserId(userId);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Failure);
            }
            return Ok(result.Data);
        }
    }
}
