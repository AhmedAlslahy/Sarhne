

using Sarhne.BLL.Abstraction;
using Sarhne.BLL.DTOs.Notification;

namespace Sarhne.BLL.Services.Interfaces
{
    public interface INotificationService
    {
        Task<Response> Create(CreateNotificationDto dto, CancellationToken cancellation = default);
        Task<Response<IEnumerable<NotificationDetailsDto>>> GetAllByUserId(string userId, CancellationToken cancellation = default);
        Task<Response<NotificationDetailsDto>> GetById(int id, string userId, CancellationToken cancellation = default);
        Task<Response<int>> UnreadCountByUserId(string userId, CancellationToken cancellation = default);
    }
}
