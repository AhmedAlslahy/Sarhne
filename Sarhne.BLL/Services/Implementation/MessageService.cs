using Microsoft.EntityFrameworkCore;
using Sarhne.BLL.Abstraction;
using Sarhne.BLL.DTOs.Message;
using Sarhne.BLL.Errors;
using Sarhne.BLL.Helper;
using Sarhne.BLL.Services.Interfaces;
using Sarhne.DAL.Entities;
using Sarhne.DAL.Repository.Interfaces;

namespace Sarhne.BLL.Services.Implementation;

public class MessageService(IUnitOfWork unitOfWork) : IMessageService
{
    public async Task<Result> CreateAsync(CreateMessageDto dto, string userId, CancellationToken cancellation)
    {
        //create Message
        var messageData = new Message
        {
            Content = dto.Content,
            ReceiverId = dto.ReceiverId,
            SenderId = userId,
            PhotoUrl = dto.Photo != null ? Upload.UploadFile("Photos", dto.Photo) : null
        };

        //create notification
        var dataNotification = new Notification
        {
            Title = "New Message",
            IsRead = false,
            SenderId = userId,
            ReceiverId = dto.ReceiverId
        };

        if (!string.IsNullOrWhiteSpace(dto.Content))
        {
            dataNotification.Body = dto.Content;
        }
        else if (dto.Photo != null)
        {
            dataNotification.Body = "Sent an image";
        }

        await unitOfWork.Messages.CreateAsync(messageData);
        await unitOfWork.Notifications.SendAsync(dataNotification);
        await unitOfWork.SaveChangesAsync(cancellation);
        return Result.Success();
    }

    public async Task<Result> StarredMessageById(int id, string userId, CancellationToken cancellation)
    {
        var result = await unitOfWork.Messages.GetByIdAsync(id, userId);
        if (result == null)
        {
            return MessageErrors.NotFound;
        }
        result.IsStarred = !result.IsStarred;
        await unitOfWork.SaveChangesAsync(cancellation);
        return Result.Success();
    }

    public async Task<Result<MessageDetailsDto>> GetMessageById(int id, string userId, CancellationToken cancellation)
    {
        var result = await unitOfWork.Messages.GetByIdAsync(id, userId);
        if (result == null)
        {
            return MessageErrors.NotFound;
        }
        var data = new MessageDetailsDto
        {
            Id = result.Id,
            IsRead = result.IsRead,
            Content = result.Content,
            CreatedAt = result.CreatedAt,
            IsStarred = result.IsStarred,
            PhotoUrl = result.PhotoUrl,
        };
        if (!result.IsRead)
        {
            result.IsRead = true;
            await unitOfWork.SaveChangesAsync(cancellation);
        }

        return data;
    }

    public async Task<Result<IEnumerable<MessageDetailsDto>>> GetAllByUserId(string userId, CancellationToken cancellation)
    {
        var query = unitOfWork.Messages.GetAllByUserId(userId);

        var data = await query
            .Select(item => new MessageDetailsDto
            {
                Id = item.Id,
                IsRead = item.IsRead,
                Content = item.Content,
                CreatedAt = item.CreatedAt,
                IsStarred = item.IsStarred,
                PhotoUrl = item.PhotoUrl,
            })
            .ToListAsync(cancellation);

        if (data.Count == 0)
        {
            return MessageErrors.NotFound;
        }

        return data;
    }

    public async Task<Result<IEnumerable<MessageDetailsDto>>> GetAllStarredByUserId(string userId, CancellationToken cancellation)
    {
        var query = unitOfWork.Messages.GetAllStarredByUserId(userId);

        var data = await query
            .Select(item => new MessageDetailsDto
            {
                Id = item.Id,
                IsRead = item.IsRead,
                Content = item.Content,
                CreatedAt = item.CreatedAt,
                IsStarred = item.IsStarred,
                PhotoUrl = item.PhotoUrl,
            })
            .ToListAsync(cancellation);

        if (data.Count == 0)
        {
            return MessageErrors.NotFound;
        }

        return data;
    }

    public async Task<Result<IEnumerable<MessageDetailsDto>>> GetAllUnreadByUserId(string userId, CancellationToken cancellation)
    {
        var query = unitOfWork.Messages.GetAllUnreadByUserId(userId);

        var data = await query
            .Select(item => new MessageDetailsDto
            {
                Id = item.Id,
                IsRead = item.IsRead,
                Content = item.Content,
                CreatedAt = item.CreatedAt,
                IsStarred = item.IsStarred,
                PhotoUrl = item.PhotoUrl,
            })
            .ToListAsync(cancellation);

        if (data.Count == 0)
        {
            return MessageErrors.NotFound;
        }

        return data;
    }

    public async Task<Result<int>> UnreadCountByUserId(string userId, CancellationToken cancellation = default)
    {
        return await unitOfWork.Messages.UnreadCountByUserIdAsync(userId, cancellation);
    }

    public async Task<Result<IEnumerable<MessageDetailsDto>>> GetAllSenderByUserId(string userId, CancellationToken cancellation)
    {
        var query = unitOfWork.Messages.GetAllSenderByUserId(userId);

        var data = await query
            .Select(item => new MessageDetailsDto
            {
                Id = item.Id,
                IsRead = item.IsRead,
                Content = item.Content,
                CreatedAt = item.CreatedAt,
                IsStarred = item.IsStarred,
                PhotoUrl = item.PhotoUrl,
            })
            .ToListAsync(cancellation);

        if (data.Count == 0)
        {
            return MessageErrors.NotFound;
        }

        return data;
    }
}