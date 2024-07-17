namespace Shared.Server.Models.Results {
   public record ChatRequestItem( 
       Guid ChatRequestId,
       Guid UserId ,
       string DisplayName ,       
       DateTime RequestedAt ,
       string ImageUrl
   ) {
        public ChatRequestItem():this(Guid.Empty,Guid.Empty,"<invalid-name>" , DateTime.UtcNow,"<invalid-image>")    
        {
            
        }
    }
}
