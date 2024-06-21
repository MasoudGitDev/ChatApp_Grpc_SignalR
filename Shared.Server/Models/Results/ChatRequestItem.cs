namespace Shared.Server.Models.Results {
   public record ChatRequestItem(
       Guid RequesterId ,
       string DisplayName ,
       DateTime SentAt
   );
}
