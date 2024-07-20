namespace Domains.Auth.Online.Aggregate;
public partial class OnlineUser {
    public static OnlineUser Create(Guid userId)
        => new() {
            UserId = userId
        };

    public Task UpdateAsync(DateTime onlineAt) {
        OnlineAt = onlineAt;
        return Task.CompletedTask;
    }
}
