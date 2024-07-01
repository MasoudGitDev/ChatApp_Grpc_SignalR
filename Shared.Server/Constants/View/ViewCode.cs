namespace Shared.Server.Constants.View;

public record ViewCode(string Name)
{
    public static implicit operator string(ViewCode code) => code.Name;
};


