using Domain.Auth.UserAggregate;

namespace Apps.Auth.Accounts;
public interface IAccountQueries {
    public IUserQueries User { get;}
}
