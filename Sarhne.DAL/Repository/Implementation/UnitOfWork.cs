using Sarhne.DAL.Database;
using Sarhne.DAL.Repository.Interfaces;

namespace Sarhne.DAL.Repository.Implementation;

public class UnitOfWork : IUnitOfWork
{
    private readonly SarhneDbContext _context;
    public INotificationRepo Notifications { get; }
    public IUserSettingRepo UserSettings { get; }
    public IMessageRepo Messages { get; }

    public UnitOfWork(
        SarhneDbContext context,
        INotificationRepo notificationRepo,
        IUserSettingRepo userSettingRepo,
        IMessageRepo messageRepo)
    {
        _context = context;
        Notifications = notificationRepo;
        UserSettings = userSettingRepo;
        Messages = messageRepo;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellation = default)
    {
        return await _context.SaveChangesAsync(cancellation);
    }
}