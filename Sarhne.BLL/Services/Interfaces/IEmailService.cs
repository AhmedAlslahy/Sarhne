using Sarhne.BLL.Abstraction;
using Sarhne.BLL.DTOs.Email;

namespace Sarhne.BLL.Services.Interfaces
{
    public interface IEmailService
    {
        Task<Response> EmailBody(string to, string subject, string body, CancellationToken cancellation = default);
        Task<Response> SendConfirmEmailOTP(string userId, CancellationToken cancellation = default);
        Task<Response> SendForgetPasswordOTP(string userId, CancellationToken cancellation = default);
        Task<Response> ConfirmEmail(ConfirmEmailDto dto);
        Task<Response> ForgetPassword(ForgetPasswordDto dto);
        Task<Response> ResetPassword(ResetPasswordDto dto);
    }
}
