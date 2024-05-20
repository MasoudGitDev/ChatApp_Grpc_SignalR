using Apps.Auth.Accounts;
using Domain.Auth.UserAggregate;

namespace Infra.EFCore.Implementations.Accounts;
internal class AccountQueries(IUserQueries _userQueries) : IAccountQueries {
    public IUserQueries User => _userQueries;
}