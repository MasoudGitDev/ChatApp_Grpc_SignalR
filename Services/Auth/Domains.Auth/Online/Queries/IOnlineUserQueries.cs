using Domains.Auth.Online.Aggregate;

namespace Domains.Auth.Online.Queries;
public interface IOnlineUserQueries {
    Task<OnlineUser?> GetByIdAsync(Guid userId);
    Task<List<OnlineUser>> GetAllAsync();
}
