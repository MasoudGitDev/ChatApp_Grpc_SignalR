namespace Domains.Auth.User.ValueObjects;  
public class ImageUrl {
    public string Url { get; private set; }
    public ImageUrl(string url) => Url = url;
    public static implicit operator ImageUrl(string url) => new (url);
    public static implicit operator string(ImageUrl imageUrl) => imageUrl.Url;
}
