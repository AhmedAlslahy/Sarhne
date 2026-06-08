using Microsoft.AspNetCore.Identity;
using Sarhne.BLL.Abstraction;
using Sarhne.BLL.DTOs.User;
using Sarhne.BLL.Errors;
using Sarhne.BLL.Helper;
using Sarhne.BLL.Services.Interfaces;
using Sarhne.DAL.Entities;
using System.Security.Cryptography;

namespace Sarhne.BLL.Services.Implementation;

public class UserService(UserManager<User> userManager) : IUserService
{
    public async Task<Result> AddAdminRole(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return UserErrors.NotFound;
        }
        bool isAdmin = await userManager.IsInRoleAsync(user, "Admin");
        if (isAdmin)
        {
            return RoleErrors.AlreadyExists;
        }

        await userManager.AddToRoleAsync(user, "Admin");

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return UserErrors.NotFound;
        }

        await userManager.DeleteAsync(user);

        return Result.Success();
    }

    public async Task<Result<IEnumerable<UserDetailsDto>>> GetAllAsync()
    {
        var users = userManager.Users.ToList();

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

        return data;
    }

    public async Task<Result<UserDetailsDto>> GetByLinkAsync(string publicLink)
    {
        var user = userManager.Users
            .FirstOrDefault(u => u.PublicLink == publicLink);
        if (user == null)
        {
            return Result<UserDetailsDto>.Fail(UserErrors.NotFound);
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
        await userManager.UpdateAsync(user);
        return Result<UserDetailsDto>.Success(data);
    }

    public async Task<Result> UpdateAsync(UserUpdateDto dto, string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return UserErrors.NotFound;
        }

        user.FullName = dto.FullName!;
        user.ProfileDescription = dto.ProfileDescription;
        user.PhoneNumber = dto.PhoneNumber;
        var uniqueNumber = RandomNumberGenerator.GetInt32(1000, 9999).ToString();
        user.PublicLink = dto.PublicLink + uniqueNumber;
        user.ImageUrl = dto.Image != null ? Upload.UploadFile("Photos", dto.Image) : null;

        await userManager.UpdateAsync(user);
        return Result.Success();
    }
}