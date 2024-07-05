namespace Shared.Server.Dtos.Dashboard;
public record ContactItemDto(
    Guid ContactId ,
    Guid UserId ,
    string DisplayName ,
    DateTime ContactedAt ,
    string ImageUrl
) {
    public ContactItemDto():this(Guid.Empty,Guid.Empty,"<empty>" , DateTime.UtcNow , "<empty>")
    {
        
    }
};