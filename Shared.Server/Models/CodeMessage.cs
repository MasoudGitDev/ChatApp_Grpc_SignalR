namespace Shared.Server.Models;
public record class CodeMessage {
    public CodeMessage()
    {
        
    }
    public string Code { get; private set; } = "<empty-code>";
    public string Message { get; private set; } = "<empty-message>";
    public static CodeMessage Create(string code , string message) => new() { Code = code , Message = message };
}
