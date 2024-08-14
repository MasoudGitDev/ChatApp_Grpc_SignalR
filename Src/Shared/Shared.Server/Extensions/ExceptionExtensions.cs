using Shared.Server.Exceptions;

namespace Shared.Server.Extensions;
public static class ExceptionExtensions {

    public static string ThrowIfNullOrWhiteSpace(this string? source , string message) {
        if(String.IsNullOrWhiteSpace(source)) {
            throw AppException.Create("<null-object>" , message);
        }
        return source;
    }

    public static T ThrowIfNull<T>(this T? source , string message) {
        if(source is null) {
            throw AppException.Create("<null-object>" , message);
        }
        return source;
    }

    public static T? ThrowIfNotNull<T>(this T? source , string message) {
        if(source is not null) {
            throw AppException.Create("<not-null-object>" , message);
        }
        return source;
    }




}
