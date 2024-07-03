namespace Shared.Server.Models.Results {
   public record ChatRequestItem(       
       Guid UserId ,
       string DisplayName ,       
       DateTime SentAt,
       string? ImageUrl = null
   );
}
