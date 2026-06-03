using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Sarhne.BLL.Abstraction;
using Sarhne.BLL.DTOs.Notification;
using Sarhne.BLL.Errors;
using Sarhne.BLL.Services.Interfaces;
using Sarhne.DAL.Entities;
using Sarhne.DAL.Repository.Interfaces;
using static Sarhne.BLL.Helper.HelperMethod;

namespace Sarhne.BLL.Services.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateNotificationDto> _createValidator;
        public NotificationService(IUnitOfWork _unitOfWork, IValidator<CreateNotificationDto> _createValidator)
        {
            this._unitOfWork = _unitOfWork;
            this._createValidator = _createValidator;
        }

        public async Task<Response> Create(CreateNotificationDto dto, CancellationToken cancellation = default)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);
            var error = ValidationHelper.Validate(validationResult);
            if (error != null)
            {
                return Response.Fail(error);
            }

            var data = new Notification { 
            Body = dto.Body,
            Title = dto.Title,
            CreatedAt = DateTime.UtcNow,
            IsRead = false,
            ReceiverId = dto.UserId
            };

            await _unitOfWork.Notifications.CreateAsync(data);
            await _unitOfWork.SaveChangesAsync(cancellation);
            return Response.Success();
        }

        public async Task<Response<IEnumerable<NotificationDetailsDto>>> GetAllByUserId(string userId, CancellationToken cancellation = default)
        {
            var result= await _unitOfWork.Notifications.GetAllByUserId(userId).ToListAsync(cancellation);
            if(result.Count==0)
            {
                return Response<IEnumerable<NotificationDetailsDto>>.Fail(NotificationErrors.NotFound);
            }
            var data = result.Select(item => new NotificationDetailsDto
            {
                Id = item.Id,
                IsRead = item.IsRead,
                Body = item.Body,
                CreatedAt = item.CreatedAt,
                Title = item.Title,
            });

            return Response<IEnumerable<NotificationDetailsDto>>.Success(data);
        }

        public async Task<Response<NotificationDetailsDto>> GetById(int id, string userId, CancellationToken cancellation = default)
        {
            var result= await _unitOfWork.Notifications.GetById(id, userId, cancellation);
            if (result == null)
            {
                return Response<NotificationDetailsDto>.Fail(NotificationErrors.NotFound);
            }

            if (!result.IsRead)
            {
                result.IsRead = true;
                await _unitOfWork.SaveChangesAsync(cancellation);
            }
            var data = new NotificationDetailsDto
            {
                Id = result.Id,
                IsRead = result.IsRead,
                Body = result.Body,
                CreatedAt = result.CreatedAt,
                Title = result.Title,
            };
          
            return Response<NotificationDetailsDto>.Success(data);
        }

        public async Task<Response<int>> UnreadCountByUserId(string userId, CancellationToken cancellation = default)
        {
            var data = await _unitOfWork.Notifications.UnreadCountByUserIdAsync(userId ,cancellation);
            return Response<int>.Success(data);
        }
    }
}
