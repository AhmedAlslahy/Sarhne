

using Sarhne.DAL.Entities;

namespace Sarhne.DAL.Repository.Interfaces
{
    public interface INotificationRepo
    {
        Task CreateAsync(Notification notification);
        IQueryable<Notification> GetAllByUserId(string userId);
        Task<int> UnreadCountByUserIdAsync(string userId);
    }
}
