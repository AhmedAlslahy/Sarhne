using Sarhne.BLL.Abstraction;
using Sarhne.BLL.DTOs.Email;

namespace Sarhne.BLL.Services.Interfaces
{
    public interface IEmailService
    {
        Task<Response> EmailBody(string to, string subject, string body, CancellationToken cancellation = default);
        Task<Response> SendConfirmEmailOTP(string email, CancellationToken cancellation = default);
        Task<Response> SendForgetPasswordOTP(string email, CancellationToken cancellation = default);
        Task<Response> ConfirmEmail(ConfirmEmailDto dto, string userId);
        Task<Response> ForgetPassword(ForgetPasswordDto dto, string email);
        Task<Response> ResetPassword(ResetPasswordDto dto, string userId);
    }
}
