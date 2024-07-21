using Domains.Auth.Online.Aggregate;
using Domains.Auth.Online.Queries;
using Infra.EFCore.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.EFCore.Implementations.Auth;
internal class OnlineUserQueries(AppDbContext _dbContext) : IOnlineUserQueries {
    public async Task<List<OnlineUser>> GetAllAsync() {
        return await _dbContext.OnlineUsers.ToListAsync();
    }

    public async Task<OnlineUser?> GetByIdAsync(Guid userId) {
        return await _dbContext.OnlineUsers.Where(x=>x.UserId == userId).FirstOrDefaultAsync();
    }
}
