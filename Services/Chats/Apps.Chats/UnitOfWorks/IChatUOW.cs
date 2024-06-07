using Apps.Auth.Queries;
using Apps.Chats.Queries;

namespace Apps.Chats.UnitOfWorks;
public interface IChatUOW {
    public IChatItemQueries ChatItemQueries { get; }
    public IChatMessageQueries MessageQueries { get; }
    public IUserQueries UserQueries { get; }
    public Task CreateAsync<TEntity>(TEntity entity) where TEntity : class, new();
    public Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class, new();
    public Task SaveChangeAsync();
}
