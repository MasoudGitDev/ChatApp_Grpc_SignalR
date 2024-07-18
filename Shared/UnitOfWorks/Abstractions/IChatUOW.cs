namespace UnitOfWorks.Abstractions;
public interface IChatUOW {
    IAppQueries Queries { get; }
    Task CreateAsync<TEntity>(TEntity entity) where TEntity : class, new();
    Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class, new();
    Task SaveChangeAsync();
}