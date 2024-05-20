namespace Shared.Server.Exceptions;
public class JwtException : AppException {
    private JwtException(string description) : base(description) => Update("<Jwt-Error>");
}
