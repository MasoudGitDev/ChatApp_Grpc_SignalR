namespace Shared.Server.Exceptions;
public class AppException :Exception {
    public string Code { get; private set; } = "<unknown-code>";
    public string Description { get; } = "<unknown-message>";

    public static AppException Create(string code , string description) => new (code ,description);
    public static AppException Create(string description) => new (description);


    public AppException(string description) : base(description) => Description = description;
    public AppException(string code , string description) :base(description) {
        Description = description;
        Code = code;
    }
    public void Update(string code)=> Code = code;
}
