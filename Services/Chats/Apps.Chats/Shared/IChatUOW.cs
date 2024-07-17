using Apps.Auth.Queries;
using Apps.Chats.Queries;

namespace Apps.Chats.Shared;
public interface IChatUOW
{
    IAppQueries Queries { get; }
    Task CreateAsync<TEntity>(TEntity entity) where TEntity : class, new();
    Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class, new();
    Task SaveChangeAsync();
}
