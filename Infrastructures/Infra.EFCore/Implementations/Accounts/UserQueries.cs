using Apps.Auth.Queries;
using Domains.Auth.User.Aggregate;
using Microsoft.AspNetCore.Identity;

namespace Infra.EFCore.Implementations.Accounts;

internal class UserQueries(UserManager<AppUser> _userManager) : IUserQueries {
    public async Task<AppUser?> FindByIdAsync(Guid userId) {
        return await _userManager.FindByIdAsync(userId.ToString());
    }

    public async Task<AppUser?> FindByUserNameAsync(string username) {
        return await _userManager.FindByNameAsync(username);
    }
}
