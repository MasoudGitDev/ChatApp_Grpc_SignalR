using Domains.Auth.User.Aggregate;
using Domains.Auth.User.ValueObjects;

namespace Domains.Auth.User.Queries;
public interface IUserQueries {
    Task<AppUser?> FindByUserNameAsync(string username);
    Task<AppUser?> FindByIdAsync(Guid userId);
    Task<AppUser?> FindByProfileIdAsync(ProfileId profileId);
    Task<AppUser?> FindByEmailAsync(string email);
    Task<List<AppUser>> GetUsersAsync(int pageNumber = 1 , int size = 20);
}

