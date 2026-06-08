namespace Sarhne.DAL.Repository.Interfaces;

public interface IUnitOfWork
{
    INotificationRepo Notifications { get; }
    IUserSettingRepo UserSettings { get; }
    IMessageRepo Messages { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellation = default);
}