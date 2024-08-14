namespace Domains.Chats.Message.ValueObjects;
public class FileUrl {
    public string Value { get; set; } = string.Empty;

    public FileUrl(string url) {
        //if(string.IsNullOrWhiteSpace(url))
        //    throw new ArgumentNullException(nameof(url));
        Value = url;
    }

    public static FileUrl Create(string url) => new(url);
    public static FileUrl Empty => new("<empty>");

    public static implicit operator string(FileUrl value) => value.Value;
    public static implicit operator FileUrl(string value) => new(value);

}
