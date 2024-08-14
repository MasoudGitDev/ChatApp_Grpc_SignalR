using Domains.Auth.Online.Aggregate;
using Domains.Auth.Online.Queries;
using Domains.Auth.User.Aggregate;
using Infra.SqlServerWithEF.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.SqlServerWithEF.Implementations.Auth;
internal class OnlineUserQueries(AppDbContext _dbContext) : IOnlineUserQueries {
    public async Task<List<OnlineUser>> GetAllAsync() {
        return await _dbContext.OnlineUsers.ToListAsync();
    }

    public async Task<OnlineUser?> GetByIdAsync(Guid userId) {
        return await _dbContext.OnlineUsers.Where(x => x.UserId == userId).FirstOrDefaultAsync();
    }

    public async Task<AppUser?> GetInfoByIdAsync(Guid userId) {
        return await _dbContext.OnlineUsers
            .Where(x => x.UserId == userId)
            .Include(x => x.User)
            .Select(x => x.User)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Guid>> GetIdsAsync() {
        return await _dbContext.OnlineUsers.Select(x => x.UserId).ToListAsync();
    }
}
