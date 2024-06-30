namespace Shared.Server.Constants;

public static class BoolExtensions {
    public static bool IsYes(this BoolType source) => source == BoolType.Yes;
    /// <summary>
    /// if true ,the answer can be No or None!
    /// </summary>
    public static bool IsNotYes(this BoolType source) => source != BoolType.Yes;

    public static bool IsNo(this BoolType source) => source == BoolType.No;
    /// <summary>
    /// if true ,the answer can be Yes or None!
    /// </summary>
    public static bool IsNotNo(this BoolType source) => source != BoolType.No;
    public static bool IsNone(this BoolType source) => source == BoolType.None;

    /// <summary>
    /// if true ,the answer can be No or Yes!
    /// </summary>
    public static bool IsNotNone(this BoolType source) => source != BoolType.None;
}
