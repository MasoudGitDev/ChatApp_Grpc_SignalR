using Domains.Auth.User.Aggregate;

namespace Apps.Auth.Queries;
public interface IUserQueries {
    Task<AppUser?> FindByUserNameAsync(string username);
    Task<AppUser?> FindByIdAsync(Guid userId);
}

