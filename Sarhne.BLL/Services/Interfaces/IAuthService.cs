using Sarhne.BLL.Abstraction;
using Sarhne.BLL.DTOs.Auth;

namespace Sarhne.BLL.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Response> Register(RegisterDto dto, CancellationToken cancellation = default);
        Task<Response<LoginRes>> Login(LoginDto dto);
    }
}
