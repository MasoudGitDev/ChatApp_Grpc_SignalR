namespace Shared.Server.Models.Results {
   public record ChatRequestItem( 
       Guid ChatRequestId,
       Guid UserId ,
       string DisplayName ,       
       DateTime RequestedAt ,
       string? ImageUrl = null
   );
}
