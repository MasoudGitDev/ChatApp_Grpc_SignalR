using Grpc.Core;
using Server.ChatApp.Protos;

namespace Server.ChatApp.GRPCHandlers;
public class SharedModelsHandler : SharedRpcs.SharedRpcsBase {
    public override Task<Empty> shared(Empty request , ServerCallContext context) {
        Console.WriteLine("SharedModelsHandler is running...");
        return base.shared(request , context);
    }
}
