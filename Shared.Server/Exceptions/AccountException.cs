namespace Shared.Server.Exceptions;
public class AccountException : AppException {
    private AccountException(string description) : base(description) => Update("<Account-Error>");
}
