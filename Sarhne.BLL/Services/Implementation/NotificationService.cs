using Microsoft.EntityFrameworkCore;
using Sarhne.BLL.Abstraction;
using Sarhne.BLL.DTOs.Notification;
using Sarhne.BLL.Errors;
using Sarhne.BLL.Services.Interfaces;
using Sarhne.DAL.Entities;
using Sarhne.DAL.Repository.Interfaces;

namespace Sarhne.BLL.Services.Implementation;

public class NotificationService(IUnitOfWork unitOfWork) : INotificationService
{
    public async Task<Result> Send(SendNotificationDto dto, string userId, CancellationToken cancellation = default)
    {
        var data = new Notification
        {
            Body = dto.Body!,
            Title = dto.Title,
            SenderId = userId,
            ReceiverId = dto.UserId
        };

        await unitOfWork.Notifications.SendAsync(data);
        await unitOfWork.SaveChangesAsync(cancellation);
        return Result.Success();
    }

    public async Task<Result<IEnumerable<NotificationDetailsDto>>> GetAllByUserId(string userId, CancellationToken cancellation = default)
    {
        var result = await unitOfWork.Notifications.GetAllByUserId(userId).ToListAsync(cancellation);
        if (result.Count == 0)
        {
            return NotificationErrors.NotFound;
        }
        var data = result.Select(item => new NotificationDetailsDto
        {
            Id = item.Id,
            IsRead = item.IsRead,
            Body = item.Body,
            CreatedAt = item.CreatedAt,
            Title = item.Title,
        });

        return Result<IEnumerable<NotificationDetailsDto>>.Success(data);
    }

    public async Task<Result<NotificationDetailsDto>> GetById(int id, string userId, CancellationToken cancellation = default)
    {
        var result = await unitOfWork.Notifications.GetById(id, userId, cancellation);
        if (result == null)
        {
            return NotificationErrors.NotFound;
        }

        if (!result.IsRead)
        {
            result.IsRead = true;
            await unitOfWork.SaveChangesAsync(cancellation);
        }
        var data = new NotificationDetailsDto
        {
            Id = result.Id,
            IsRead = result.IsRead,
            Body = result.Body,
            CreatedAt = result.CreatedAt,
            Title = result.Title,
        };

        return data;
    }

    public async Task<Result<int>> UnreadCountByUserId(string userId, CancellationToken cancellation = default)
    {
        return await unitOfWork.Notifications.UnreadCountByUserIdAsync(userId, cancellation);
    }
}