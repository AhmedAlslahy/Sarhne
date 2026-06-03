using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sarhne.BLL.DTOs.Message;
using Sarhne.BLL.Services.Interfaces;
using System.Security.Claims;

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

        [HttpPost("")]
        public async Task<IActionResult> Create(CreateMessageDto dto, CancellationToken cancellation)
        {
            var result = await _messageService.CreateAsync(dto,cancellation);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Failure);
            }
            return Ok(result.IsSuccess);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessageById(int id, CancellationToken cancellation)
        {
            var result = await _messageService.GetMessageById(id, cancellation);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Failure);
            }
            return Ok(result.Data);
        }

        [HttpPost("Starred/{id}")]
        public async Task<IActionResult> StarredMessageById(int id, CancellationToken cancellation)
        {
            var result = await _messageService.StarredMessageById(id, cancellation);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Failure);
            }
            return Ok(result.IsSuccess);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllByUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }
            var result = await _messageService.GetAllByUserId(userId);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Failure);
            }
            return Ok(result.Data);
        }

        [HttpGet("all-starred")]
        public async Task<IActionResult> GetAllStarredByUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }
            var result = await _messageService.GetAllStarredByUserId(userId);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Failure);
            }
            return Ok(result.Data);
        }

        [HttpGet("all-unread")]
        public async Task<IActionResult> GetAllUnreadByUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }
            var result = await _messageService.GetAllUnreadByUserId(userId);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Failure);
            }
            return Ok(result.Data);
        }
    }
}
