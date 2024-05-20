namespace Domain.Auth.UserAggregate;
public interface IUserQueries {
    Task<AppUser?> FindByUserNameAsync(string username);
}
