using Microsoft.EntityFrameworkCore;
using Sarhne.BLL.Abstraction;
using Sarhne.BLL.DTOs.Message;
using Sarhne.BLL.Errors;
using Sarhne.BLL.Helper;
using Sarhne.BLL.Services.Interfaces;
using Sarhne.DAL.Entities;
using Sarhne.DAL.Repository.Interfaces;


namespace Sarhne.BLL.Services.Implementation
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MessageService(IUnitOfWork _unitOfWork)
        {
            this._unitOfWork = _unitOfWork;
        }

        public async Task<Response> CreateAsync(CreateMessageDto dto, CancellationToken cancellation = default)
        {
            if (string.IsNullOrWhiteSpace(dto.Content) && dto.Photo == null)
            {
                return Response.Fail(MessageErrors.InvalidData);
            }
            //create Message
            var messageData = new Message { 
            Content = dto.Content,
            CreatedAt = DateTime.UtcNow,
            IsRead = false,
            IsStarred = false,
            ReceiverId = dto.ReceiverId,
            PhotoUrl = dto.Photo != null ? Upload.UploadFile("Photos", dto.Photo) : null
            };

            //create notification
            var dataNotification = new Notification
            {
                Title = "New Message",
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                ReceiverId = dto.ReceiverId
            };

            if (!string.IsNullOrWhiteSpace(dto.Content))
            {
                dataNotification.Body = HelperMethod.GetPreview(dto.Content);
            }
            else if (dto.Photo != null)
            {
                dataNotification.Body = "📷 Sent an image";
            }
            
            await _unitOfWork.Messages.CreateAsync(messageData);
            await _unitOfWork.Notifications.CreateAsync(dataNotification);
            await _unitOfWork.SaveChangesAsync(cancellation);
            return Response.Success();
        }

        public async Task<Response<IEnumerable<MessageDetailsDto>>> GetAllByUserId(string userId, CancellationToken cancellation)
        {
            var query = _unitOfWork.Messages.GetAllByUserId(userId);

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

            if (data.Count==0)
            {
                return Response<IEnumerable<MessageDetailsDto>>.Fail(MessageErrors.NotFound);
            }

            return Response<IEnumerable<MessageDetailsDto>>.Success(data);
        }

        public async Task<Response<IEnumerable<MessageDetailsDto>>> GetAllStarredByUserId(string userId, CancellationToken cancellation)
        {
            var query = _unitOfWork.Messages.GetAllStarredByUserId(userId);

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
                return Response<IEnumerable<MessageDetailsDto>>.Fail(MessageErrors.NotFound);
            }

            return Response<IEnumerable<MessageDetailsDto>>.Success(data);
        }

        public async Task<Response<IEnumerable<MessageDetailsDto>>> GetAllUnreadByUserId(string userId, CancellationToken cancellation)
        {
            var query = _unitOfWork.Messages.GetAllUnreadByUserId(userId);

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
                return Response<IEnumerable<MessageDetailsDto>>.Fail(MessageErrors.NotFound);
            }

            return Response<IEnumerable<MessageDetailsDto>>.Success(data);
        }
    }
}
