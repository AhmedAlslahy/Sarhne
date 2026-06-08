using Microsoft.EntityFrameworkCore;
using Sarhne.DAL.Database;
using Sarhne.DAL.Entities;
using Sarhne.DAL.Repository.Interfaces;

namespace Sarhne.DAL.Repository.Implementation;

public class MessageRepo(SarhneDbContext context) : IMessageRepo
{
    public async Task CreateAsync(Message message)
    {
        await context.Messages.AddAsync(message);
    }

    public IQueryable<Message> GetAllByUserId(string userId)
    {
        return context.Messages.Where(n => n.ReceiverId == userId).AsNoTracking();
    }

    public IQueryable<Message> GetAllUnreadByUserId(string userId)
    {
        return context.Messages.Where(n => n.ReceiverId == userId && !n.IsRead).AsNoTracking();
    }

    public IQueryable<Message> GetAllStarredByUserId(string userId)
    {
        return context.Messages.Where(n => n.ReceiverId == userId && n.IsStarred).AsNoTracking();
    }

    public IQueryable<Message> GetAllSenderByUserId(string userId)
    {
        return context.Messages.Where(n => n.SenderId == userId).AsNoTracking();
    }

    public async Task<Message?> GetByIdAsync(int id, string userId)
    {
        return await context.Messages.FirstOrDefaultAsync(m => m.Id == id && (m.ReceiverId == userId || m.SenderId == userId));
    }

    public async Task<int> UnreadCountByUserIdAsync(string userId, CancellationToken cancellation = default)
    {
        return await context.Messages.CountAsync(n => n.ReceiverId == userId && !n.IsRead, cancellation);
    }
}