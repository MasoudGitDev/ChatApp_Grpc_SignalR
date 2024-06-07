using Apps.Chats.Abstractions;
using Domain.Auth.UserAggregate;

namespace Apps.Chats;
public interface IChatUOW {
    public IChatItemQueries ChatItemQueries { get; }
    public IMessageQueries MessageQueries { get;}
    public IUserQueries UserQueries { get; }
    public Task CreateAsync<TEntity>(TEntity entity) where TEntity : class, new();
    public Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class, new();
    public Task SaveChangeAsync();
}
