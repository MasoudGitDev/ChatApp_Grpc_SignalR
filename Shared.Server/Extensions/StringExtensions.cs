using Shared.Server.Exceptions;
using System.Text.Json;

namespace Shared.Server.Extensions;
public static class StringExtensions {
    public static Guid AsGuid(this string? id) {
        bool isValidGuid = Guid.TryParse(id , out Guid validId);
        if(isValidGuid is false) {
            throw new AppException( "Invalid-Guid" , $"The id :<{id}> is not valid guid.");
        }
        return validId;
    }

    private static JsonSerializerOptions JsonSerializerOptions => new() {
        AllowTrailingCommas = true,
        PropertyNameCaseInsensitive =false,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
    };

    public static string AsJson<TModel>(this TModel? source) {
        if(source == null) {
            return string.Empty;
        }
        return JsonSerializer.Serialize(source , JsonSerializerOptions);
    }
    public static TModel? AsModel<TModel>(this string? jsonSource) where TModel : class , new() {
        if(jsonSource == null) {
            return default;
        }
        return JsonSerializer.Deserialize<TModel>(jsonSource , JsonSerializerOptions);
    }
}
