
using Microsoft.EntityFrameworkCore;
using Sarhne.DAL.Database;
using Sarhne.DAL.Entities;
using Sarhne.DAL.Repository.Interfaces;

namespace Sarhne.DAL.Repository.Implementation
{
    public class MessageRepo : IMessageRepo
    {
        SarhneDbContext _context;
        public MessageRepo(SarhneDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(Message message)
        {
            await _context.Messages.AddAsync(message);
        }

        public IQueryable<Message> GetAllByUserId(string userId)
        {
            return _context.Messages.Where(n => n.ReceiverId == userId).AsNoTracking();
        }

        public IQueryable<Message> GetAllUnreadByUserId(string userId)
        {
            return _context.Messages.Where(n => n.ReceiverId == userId && !n.IsRead).AsNoTracking();
        }

        public IQueryable<Message> GetAllStarredByUserId(string userId)
        {
            return _context.Messages.Where(n => n.ReceiverId == userId && n.IsStarred).AsNoTracking();
        }
        public async Task<Message?> GetByIdAsync(int id)
        {
            return await _context.Messages.FindAsync(id);
        }
    }
}