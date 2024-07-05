using Google.Protobuf.Collections;
using Mapster;

namespace Server.ChatApp.Extensions;

public static class GrpcExtensions {

    public static RepeatedField<T> AsRepeatedFields<T, U>(this LinkedList<U> items)
        where T : class, new()
        where U : class, new() {
        RepeatedField<T> result = [];
        foreach(var item in items) {
            result.Add(item.Adapt<T>());
        }
        return result;
    }

}
