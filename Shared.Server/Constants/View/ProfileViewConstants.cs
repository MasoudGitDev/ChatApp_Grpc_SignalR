namespace Shared.Server.Constants.View;

public sealed record ProfileViewConstants(string Name) {

    public static ViewCode InvalidProfileId => new(nameof(InvalidProfileId));
    public static ViewCode InvalidContact => new(nameof(InvalidContact));
    /// <summary>
    /// Represents a profile found in the Contacts table.
    /// </summary>
    public static ViewCode Found => new(nameof(Found));

    /// <summary>
    /// Represents a profile not found in the Contacts table.
    /// </summary>
    public static ViewCode NotFound => new(nameof(NotFound));

    /// <summary>
    /// Represents a profile found in the chat-requests table and requiring confirmation.
    /// </summary>    
    public static ViewCode Confirm => new(nameof(Confirm));

    /// <summary>
    /// Represents a profile found in the chat-requests table and waiting for acceptance.
    /// </summary>   
    public static ViewCode WaitForAccept => new(nameof(WaitForAccept));

    public static ViewCode BlockedByReceiver => new(nameof(BlockedByReceiver));

    public static ButtonName InvalidBtn => new(nameof(InvalidBtn).Replace("Btn" , ""));
    public static ButtonName DialogBtn => new(nameof(DialogBtn).Replace("Btn" , ""));
    public static ButtonName RequestBtn => new(nameof(RequestBtn).Replace("Btn" , ""));
    public static ButtonName ConfirmBtn => new(nameof(ConfirmBtn).Replace("Btn" , ""));
    public static ButtonName WaitForAcceptBtn => new(nameof(WaitForAcceptBtn).Replace("Btn" , ""));

    public static (string Name, bool CanShow) ApplyCodeResult(string code) {
        var result = CodeResults.Where(x => x.Code == code).Select(x => x).FirstOrDefault();
        if(result is null) {
            return ("Invalid", false);
        }
        return (result.ButtonName, result.CanShow);
    }
    //======================= privates
    private static readonly List<CodeResult> CodeResults = [
        new CodeResult(InvalidProfileId , false , InvalidBtn) ,
        new CodeResult(InvalidContact , true , DialogBtn),
        new CodeResult(Found , true , DialogBtn),
        new CodeResult(NotFound , true ,RequestBtn),
        new CodeResult(Confirm , true , ConfirmBtn),
        new CodeResult(WaitForAccept , true , WaitForAcceptBtn)
    ];
    private record CodeResult(string Code , bool CanShow , string ButtonName);

}


