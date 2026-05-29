
using Microsoft.EntityFrameworkCore;
using Sarhne.DAL.Database;
using Sarhne.DAL.Entities;
using Sarhne.DAL.Repository.Interfaces;

namespace Sarhne.DAL.Repository.Implementation
{
    public class NotificationRepo : INotificationRepo
    {
        SarhneDbContext _context;
        public NotificationRepo(SarhneDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
        }

        public IQueryable<Notification> GetAllByUserId(string userId)
        {
            return _context.Notifications.Where(n => n.ReceiverId == userId).AsNoTracking();
        }

        public Task<Notification?> GetById(int id , string userId , CancellationToken cancellation = default)
        {
            return _context.Notifications.FirstOrDefaultAsync(n => n.Id == id && n.ReceiverId ==userId, cancellation);
        }
        public async Task<int> UnreadCountByUserIdAsync(string userId, CancellationToken cancellation = default)
        {
            return await _context.Notifications.CountAsync(n => n.ReceiverId == userId && !n.IsRead,cancellation);
        }
    }
}