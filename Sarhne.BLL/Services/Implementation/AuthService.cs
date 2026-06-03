
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Sarhne.BLL.Abstraction;
using Sarhne.BLL.DTOs.Auth;
using Sarhne.BLL.Errors;
using Sarhne.BLL.Services.Interfaces;
using Sarhne.DAL.Entities;
using Sarhne.DAL.Enums;
using static Sarhne.BLL.Abstraction.Errors;
using static Sarhne.BLL.Helper.HelperMethod;

namespace Sarhne.BLL.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IValidator<LoginDto> _Loginvalidator;
        private readonly IValidator<RegisterDto> _Regvalidator;
        public AuthService(UserManager<User> userManager, IJwtService _jwtService
            , IValidator<LoginDto> _Loginvalidator, IValidator<RegisterDto> _Regvalidator)
        {
            _userManager = userManager;
            this._jwtService = _jwtService;
            this._Loginvalidator = _Loginvalidator;
            this._Regvalidator = _Regvalidator;
        }

        public async Task<Response> Register(RegisterDto dto, CancellationToken cancellation)
        {
            var validationResult = await _Regvalidator.ValidateAsync(dto);
            var error = ValidationHelper.Validate(validationResult);
            if (error != null)
            {
                return Response.Fail(error);
            }

            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                return Response.Fail(UserErrors.AlreadyExists);
            }

            var user = new User
            {
                Email = dto.Email,
                FullName = dto.FullName,
                UserName = dto.UserName,
                EmailConfirmed = false,
                UserSetting = new UserSetting
                {
                    AllowAnonymousMessages = true,
                    ShowLastSeen = true,
                    ShowProfileViews = true
                }
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Response.Fail(new Error("Create Failed", errors, ErrorType.BadRequest));
            }
            await _userManager.AddToRoleAsync(user, "User");
            return Response.Success();
        }

        public async Task<Response<LoginRes>> Login(LoginDto dto)
        {
            var validationResult = await _Loginvalidator.ValidateAsync(dto);
            var error = ValidationHelper.Validate(validationResult);
            if (error != null)
            {
                return Response<LoginRes>.Fail(error);
            }

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return Response<LoginRes>.Fail(UserErrors.NotFound);
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!passwordValid)
            {
                return Response<LoginRes>.Fail(new Error("Invalid Password", "Cannot Reset Password", ErrorType.BadRequest));
            }

            var roles =await _userManager.GetRolesAsync(user);
            var tokenResult = await _jwtService.GenerateToken(user, roles);

            if (!tokenResult.IsSuccess)
            {
                return Response<LoginRes>.Fail(tokenResult.Failure);
            }

            var data = new LoginRes { 
            Token= tokenResult.Data.Token,
            ExpireIn = tokenResult.Data.ExpireIn
            };
            return Response<LoginRes>.Success(data);
        }
    }
}
