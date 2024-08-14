using Domains.Auth.User.Aggregate;

namespace Domains.Auth.Online.Aggregate;
public partial class OnlineUser {
    public Guid UserId { get; private set; }
    public DateTime OnlineAt { get; set; } = DateTime.UtcNow;

    //================ relations
    public AppUser User { get; private set; } = null!;
}