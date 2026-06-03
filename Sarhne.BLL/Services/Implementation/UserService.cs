using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Sarhne.BLL.Abstraction;
using Sarhne.BLL.DTOs.User;
using Sarhne.BLL.Errors;
using Sarhne.BLL.Helper;
using Sarhne.BLL.Services.Interfaces;
using Sarhne.DAL.Entities;
using System.Security.Cryptography;
using static Sarhne.BLL.Helper.HelperMethod;

namespace Sarhne.BLL.Services.Implementation
{
    public class UserService : IUserService
    {

        private readonly UserManager<User> _userManager;
        private readonly IValidator<UserUpdateDto> _updateValidator;
        public UserService(UserManager<User> userManager, IValidator<UserUpdateDto> _updateValidator)
        {
            _userManager = userManager;
            this._updateValidator = _updateValidator;
        }

        public async Task<Response> AddAdminRole(string userId)
        {

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return Response.Fail(UserErrors.NotFound);
            }

            await _userManager.AddToRoleAsync(user, "Admin");

            return Response.Success();
        }

        public async Task<Response> DeleteAsync(string userId)
        {

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return Response.Fail(UserErrors.NotFound);
            }

            await _userManager.DeleteAsync(user);

            return Response.Success();
        }

        public async Task<Response<IEnumerable<UserDetailsDto>>> GetAllAsync()
        {
            var users = _userManager.Users.ToList();

            var data = new List<UserDetailsDto>();

            foreach (var user in users)
            {
                data.Add(new UserDetailsDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    PublicLink = user.PublicLink,
                    PhoneNumber = user.PhoneNumber,
                    ImageUrl = user.ImageUrl,
                    Gender = user.Gender,
                    ProfileDescription = user.ProfileDescription,
                    LastSeen = user.LastSeen,
                    ProfileViewsCount = user.ProfileViewsCount,
                });
            }

            return Response<IEnumerable<UserDetailsDto>>.Success(data);
        }

        public async Task<Response<UserDetailsDto>> GetByLinkAsync(string publicLink)
        {
            var user = _userManager.Users
                .FirstOrDefault(u => u.PublicLink == publicLink);
            if (user == null)
            {
                return Response<UserDetailsDto>.Fail(UserErrors.NotFound);
            }

            var data = new UserDetailsDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                PublicLink = user.PublicLink,
                PhoneNumber = user.PhoneNumber,
                ImageUrl = user.ImageUrl,
                Gender = user.Gender,
                ProfileDescription = user.ProfileDescription,
                LastSeen = user.LastSeen,
                ProfileViewsCount = user.ProfileViewsCount,
            };

            user.ProfileViewsCount++;
            await _userManager.UpdateAsync(user);
            return Response<UserDetailsDto>.Success(data);
        }

        public async Task<Response> UpdateAsync(UserUpdateDto dto, string userId)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto);
            var error = ValidationHelper.Validate(validationResult);
            if (error != null)
            {
                return Response.Fail(error);
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Response.Fail(UserErrors.NotFound);
            }

            user.FullName = dto.FullName;
            user.ProfileDescription = dto.ProfileDescription;
            user.PhoneNumber = dto.PhoneNumber;
            var uniqueNumber = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
            user.PublicLink = dto.PublicLink+ uniqueNumber;
            user.ImageUrl = dto.Image != null ? Upload.UploadFile("Photos", dto.Image) : null;

            await _userManager.UpdateAsync(user);
            return Response.Success();
        }
    }
}