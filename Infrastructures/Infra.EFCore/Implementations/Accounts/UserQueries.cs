using Domain.Auth.UserAggregate;
using Microsoft.AspNetCore.Identity;

namespace Infra.EFCore.Implementations.Accounts;

internal class UserQueries(UserManager<AppUser> _userManager) : IUserQueries {
    public async Task<AppUser?> FindByUserNameAsync(string username) {
      return  await _userManager.FindByNameAsync(username);
    }
}
