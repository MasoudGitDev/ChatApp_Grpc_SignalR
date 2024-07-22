using Domains.Auth.Online.Aggregate;
using Domains.Auth.User.Aggregate;

namespace Domains.Auth.Online.Queries;
public interface IOnlineUserQueries {
    Task<OnlineUser?> GetByIdAsync(Guid userId);
    Task<List<OnlineUser>> GetAllAsync();
    Task<List<Guid>> GetIdsAsync();

    /// <summary>
    /// This Methods can be a GetUserWithOnlineStatus() method.
    /// </summary>
    Task<AppUser?> GetInfoByIdAsync(Guid userId);
}
