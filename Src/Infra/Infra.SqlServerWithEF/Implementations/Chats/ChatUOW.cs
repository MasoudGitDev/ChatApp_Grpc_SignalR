using Infra.SqlServerWithEF.Contexts;
using Infra.SqlServerWithEF.Exceptions;
using Shared.Server.Exceptions;
using UnitOfWorks.Abstractions;

namespace Infra.SqlServerWithEF.Implementations.Chats;
internal class ChatUOW(AppDbContext _dbContext , IAppQueries _queries) : IChatUOW {

    public IAppQueries Queries => _queries;

    //=====================

    public async Task CreateAsync<TEntity>(TEntity entity) where TEntity : class, new() {
        await _dbContext.AddAsync(entity , new CancellationToken());
    }
    public async Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class, new() {
        _dbContext.Remove(entity);
        await Task.CompletedTask;
    }

    [DbChangeException]
    public async Task SaveChangeAsync() {
        try {
            await _dbContext.SaveChangesAsync();
        }
        catch(Exception ex) {
            throw AppException.Create("OnDbSaveChange" , ex.InnerException?.Message ?? ex.Message);
        }
    }
}