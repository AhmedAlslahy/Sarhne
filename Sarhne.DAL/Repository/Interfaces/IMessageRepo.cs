using Sarhne.DAL.Entities;

namespace Sarhne.DAL.Repository.Interfaces;

public interface IMessageRepo
{
    Task CreateAsync(Message message);

    IQueryable<Message> GetAllByUserId(string userId);

    IQueryable<Message> GetAllStarredByUserId(string userId);

    IQueryable<Message> GetAllUnreadByUserId(string userId);

    IQueryable<Message> GetAllSenderByUserId(string userId);

    Task<Message?> GetByIdAsync(int id, string userId);

    Task<int> UnreadCountByUserIdAsync(string userId, CancellationToken cancellation = default);
}