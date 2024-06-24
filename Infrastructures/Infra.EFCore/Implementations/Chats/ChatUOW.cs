using Apps.Auth.Queries;
using Apps.Chats.Queries;
using Apps.Chats.UnitOfWorks;
using Infra.EFCore.Contexts;
using Infra.EFCore.Exceptions;
using Shared.Server.Exceptions;

namespace Infra.EFCore.Implementations.Chats;
internal class ChatUOW(AppDbContext _dbContext ,IAppQueries _queries) : IChatUOW {

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