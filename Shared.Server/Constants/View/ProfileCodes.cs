namespace Shared.Server.Constants.View;

public sealed record ProfileViewConstants(string Name)
{
    public static ViewCode Invalid => new(nameof(Invalid));
    public static ViewCode MyId => new(nameof(MyId));
    public static ViewCode Founded => new(nameof(Founded));
    public static ViewCode NotFounded => new(nameof(NotFounded));
    public static ViewCode Confirm => new(nameof(Confirm));

    public static ButtonName DialogBtn => new(nameof(DialogBtn).Replace("Btn" , ""));
    public static ButtonName RequestBtn => new(nameof(RequestBtn).Replace("Btn" , ""));
    public static ButtonName ConfirmBtn => new(nameof(ConfirmBtn).Replace("Btn" , ""));

    public static (string Name, bool CanShow) ApplyCodeResult(string code)
    {
        var result = CodeResults.Where(x => x.Code == code).Select(x => x).FirstOrDefault();
        if (result is null)
        {
            return ("Invalid", false);
        }
        return (result.ButtonName, result.CanShow);
    }
    private static List<CodeResult> CodeResults = [
        new CodeResult(Invalid , false , Invalid) ,
        new CodeResult(MyId , true , DialogBtn),
        new CodeResult(Founded , true , DialogBtn),
        new CodeResult(NotFounded , true ,RequestBtn),
        new CodeResult(Confirm , true , ConfirmBtn)
    ];
    private record CodeResult(string Code, bool CanShow, string ButtonName);
    
}


