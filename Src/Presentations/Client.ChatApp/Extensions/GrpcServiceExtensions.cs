using Grpc.Core;
using Mapster;

namespace Client.ChatApp.Extensions;

public static class GrpcServiceExtensions {
    public static async Task<List<TModel>> ToListAsync<TMessage, TModel>(this AsyncServerStreamingCall<TMessage> streamingCall) {
        var list = new List<TModel>();
        while(await streamingCall.ResponseStream.MoveNext(new CancellationToken())) {
            list.Add(streamingCall.ResponseStream.Current.Adapt<TModel>());
        }
        return list;
    }
    public static async Task<LinkedList<TModel>> ToLinkedListAsync<TMessage, TModel>(this AsyncServerStreamingCall<TMessage> streamingCall) {
        var list = new LinkedList<TModel>();
        while(await streamingCall.ResponseStream.MoveNext(new CancellationToken())) {
            list.AddLast(streamingCall.ResponseStream.Current.Adapt<TModel>());
        }
        return list;
    }
}
