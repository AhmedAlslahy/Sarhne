namespace Sarhne.DAL.Repository.Interfaces;

public interface INotificationRepo
{
    Task SendAsync(Notification notification);

    Task<Notification?> GetById(int id, string userId, CancellationToken cancellation = default);

    IQueryable<Notification> GetAllByUserId(string userId);

    Task<int> UnreadCountByUserIdAsync(string userId, CancellationToken cancellation = default);
}