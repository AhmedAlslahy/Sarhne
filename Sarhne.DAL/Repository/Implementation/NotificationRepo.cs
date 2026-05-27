
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
            await _context.SaveChangesAsync();
        }

        public IQueryable<Notification> GetAllByUserId(string userId)
        {
            return _context.Notifications.Where(n => n.ReceiverId == userId).AsNoTracking();
        }

        public Task<int> UnreadCountByUserIdAsync(string userId)
        {
            return _context.Notifications.CountAsync(n => n.ReceiverId == userId && !n.IsRead);
        }
    }
}
