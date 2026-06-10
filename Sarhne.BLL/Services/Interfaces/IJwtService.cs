
namespace Sarhne.BLL.Services.Interfaces;

public interface IJwtService
{
    Task<Result<GenerateTokenResDto>> GenerateToken(User user, IList<string> roles);
}