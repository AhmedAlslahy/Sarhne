using Sarhne.BLL.Abstraction;
using Sarhne.BLL.DTOs.Token;
using Sarhne.DAL.Entities;

namespace Sarhne.BLL.Services.Interfaces;

public interface IJwtService
{
    Task<Result<GenerateTokenResDto>> GenerateToken(User user, IList<string> roles);
}