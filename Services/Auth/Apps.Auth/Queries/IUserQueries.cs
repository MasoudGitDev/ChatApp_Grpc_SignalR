using Domains.Auth.User.Aggregate;
using Domains.Auth.User.ValueObjects;

namespace Apps.Auth.Queries;
public interface IUserQueries {
    Task<AppUser?> FindByUserNameAsync(string username);
    Task<AppUser?> FindByIdAsync(Guid userId);
    Task<AppUser?> FindByProfileIdAsync(ProfileId profileId);
}

