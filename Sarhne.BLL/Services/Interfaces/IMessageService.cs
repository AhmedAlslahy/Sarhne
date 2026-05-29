

using Sarhne.BLL.Abstraction;
using Sarhne.BLL.DTOs.Message;

namespace Sarhne.BLL.Services.Interfaces
{
    public interface IMessageService
    {
        Task<Response> CreateAsync(CreateMessageDto dto, CancellationToken cancellation = default);
        Task<Response<IEnumerable<MessageDetailsDto>>> GetAllByUserId(string userId, CancellationToken cancellation = default);
        Task<Response<IEnumerable<MessageDetailsDto>>> GetAllStarredByUserId(string userId, CancellationToken cancellation = default);
        Task<Response<IEnumerable<MessageDetailsDto>>> GetAllUnreadByUserId(string userId, CancellationToken cancellation = default);
    }
}
