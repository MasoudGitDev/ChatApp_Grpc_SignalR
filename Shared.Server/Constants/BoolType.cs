namespace Shared.Server.Constants;

public record BoolType(byte Order , string Name) {
    public static BoolType None => new(0 , "None");
    public static BoolType Yes => new(1 , "Yes");
    public static BoolType No => new(2 , "No");

    public static implicit operator bool?(BoolType boolValue) =>
        boolValue == BoolType.None ? null : boolValue == BoolType.Yes;
    public static implicit operator BoolType(bool? value) {
        if(value is null) {
            return BoolType.None;
        }
        return (bool) value ? BoolType.Yes : BoolType.No;
    }
}
