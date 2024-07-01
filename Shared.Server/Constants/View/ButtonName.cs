namespace Shared.Server.Constants.View;

public record ButtonName(string Name) {
    public static implicit operator string(ButtonName code) => code.Name;
};


