using Domains.Auth.Queries;
using Domains.Auth.User.Aggregate;
using Domains.Auth.User.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infra.EFCore.Implementations.Auth;

internal class UserQueries(UserManager<AppUser> _userManager) : IUserQueries {
    public async Task<AppUser?> FindByEmailAsync(string email) {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<AppUser?> FindByIdAsync(Guid userId) {
        return await _userManager.FindByIdAsync(userId.ToString());
    }

    public async Task<AppUser?> FindByProfileIdAsync(ProfileId profileId) {
        return await _userManager.Users.Where(x => x.ProfileId == profileId).FirstOrDefaultAsync();
    }

    public async Task<AppUser?> FindByUserNameAsync(string username) {
        return await _userManager.FindByNameAsync(username);
    }

    public async Task<List<AppUser>> GetUsersAsync(int pageNumber = 1 , int size = 20) {

        return await _userManager.Users
            .OrderBy(x => x.CreatedAt)
            .Skip(( pageNumber - 1 ) * size)
            .Take(size)
            .ToListAsync();
    }
}
