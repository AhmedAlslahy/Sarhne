using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sarhne.BLL.Services.Interfaces;

namespace Sarhne.API.Controllers
{
    [Route("api/Notifications")]
    [Authorize]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService _notificationService)
        {
            this._notificationService = _notificationService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllByUserId(string userId, CancellationToken cancellation)
        {
            var result = await _notificationService.GetAllByUserId(userId,cancellation);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Failure);
            }
            return Ok(result.Data);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id,string userId, CancellationToken cancellation)
        {
            var result = await _notificationService.GetById(Id,userId,cancellation);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Failure);
            }
            return Ok(result.Data);
        }

        [HttpGet("unread-count/{userId}")]
        public async Task<IActionResult> GetById(string userId, CancellationToken cancellation)
        {
            var result = await _notificationService.UnreadCountByUserId(userId, cancellation);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Failure);
            }
            return Ok(result.Data);
        }
    }
}
