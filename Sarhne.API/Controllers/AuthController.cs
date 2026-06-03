using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sarhne.BLL.DTOs.Auth;
using Sarhne.BLL.DTOs.Email;
using Sarhne.BLL.Services.Interfaces;
using System.Security.Claims;

namespace Sarhne.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;

        public AuthController(IAuthService _authService, IEmailService _emailService)
        {
            this._authService = _authService;
            this._emailService = _emailService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {            
            var result = await _authService.Login(dto);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Failure);
            }
            Response.Cookies.Append("jwt", result.Data.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            });

            return Ok(result.Data);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto, CancellationToken cancellation)
        {
            var result = await _authService.Register(dto ,cancellation);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Failure);
            }           
            return Ok(result.IsSuccess);
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");

            return Ok(new { message = "Logged out successfully" });
        }



        //----------------------------------------------------
        [HttpPost("send-confirm-email-OTP")]
        public async Task<IActionResult> SendConfirmEmailOTP(string email, CancellationToken cancellation)
        {            
            var result = await _emailService.SendConfirmEmailOTP(email, cancellation);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Failure);
            }
            return Ok(result.IsSuccess);
        }

        [HttpPost("Send-Forget-Password-OTP")]
        public async Task<IActionResult> SendForgetPasswordOTP(string email,CancellationToken cancellation)
        {
            var result = await _emailService.SendForgetPasswordOTP(email, cancellation);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Failure);
            }
            return Ok(result.IsSuccess);
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }
            var result = await _emailService.ConfirmEmail(dto, userId);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Failure);
            }
            return Ok(result.IsSuccess);
        }


        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordDto dto,string email)
        {
            var result = await _emailService.ForgetPassword(dto, email);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Failure);
            }
            return Ok(result.IsSuccess);
        }

        [Authorize]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }
            var result = await _emailService.ResetPassword(dto, userId);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Failure);
            }
            return Ok(result.IsSuccess);
        }
    }
}