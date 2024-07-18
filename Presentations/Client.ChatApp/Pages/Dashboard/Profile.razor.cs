using Client.ChatApp.Protos;
using Microsoft.AspNetCore.Components;
using Shared.Server.Constants;
using Shared.Server.Constants.View;

namespace Client.ChatApp.Pages.Dashboard;

public class ProfileViewHandler : ComponentBase {

    //=============== public props
    [Parameter]
    public string ProfileId { get; set; } = null!;

    //================== injections
    [Inject]
    private NavigationManager NavManager { get; set; } = null!;

    [Inject]
    private ContactRPCs.ContactRPCsClient ContactService { get; set; } = null!;

    [Inject]
    private ChatRequestCommandsRPCs.ChatRequestCommandsRPCsClient RequestService { get; set; } = null!;

    //========================== Use In View
    protected List<MessageInfo> Messages = [];
    protected bool IsAnyError = false;
    protected BoolType IsMe = BoolType.None;
    protected bool CanShowButton = false;
    protected string ButtonName = string.Empty;
    //================================ private fields
    private string ReceiverId =  String.Empty;
    private string ChatRequestId = String.Empty;
    //====================== protected Methods
    protected override async Task OnInitializedAsync() {
        try {
            Messages.Clear();
            var result = await ContactService.IsInContactsAsync(new ContactMsg() { ProfileId = ProfileId });
            CheckContactResult(result);
        }
        catch(Exception ex) {
            Console.WriteLine("OnInitializedAsync : " + ex.Message);
            Messages.Add(SharedViewCodes.NotCompletedLoading);
        }
    }
    protected async Task OnButtonClick() {
        if(ButtonName == ProfileViewConstants.DialogBtn && IsMe.IsYes()) {
            NavManager.NavigateTo("/Chats");
            return;
        }
        if(ButtonName == ProfileViewConstants.DialogBtn) {
            NavManager.NavigateTo("/Chats");
            return;
        }
        if(ButtonName == ProfileViewConstants.RequestBtn) {
            Messages.AddRange(await DoAsync(Messages ,
                async () => await RequestService.RequestAsync(new PersonMsg() { Id = ReceiverId })));
            return;
        }
        if(ButtonName == ProfileViewConstants.ConfirmBtn) {
            Messages.AddRange(await DoAsync(Messages ,
                async () => await RequestService.AcceptAsync(new() { ChatRequestId = ChatRequestId })));
            return;
        }
    }
    protected void CloseNotification(MessageInfo model) {
        Messages.Remove(model);
        if(Messages.Count <= 0) {
            IsAnyError = false;
        }
    }

    //============== private Methods
    private void CheckContactResult(ContactResult result) {
        foreach(var message in result.Messages) {
            Messages.Add(message);
            (ButtonName, CanShowButton) = ProfileViewConstants.ApplyCodeResult(message.Code);
        }
        IsAnyError = SharedViewCodes.GetErrors(Messages).Count > 0;
        ReceiverId = result.ContactInfo.UserId;
        ChatRequestId = result.ChatRequestId;
    }
    private static async Task<List<MessageInfo>> DoAsync(List<MessageInfo> messages ,
        Func<Task<ResultMsg>> action) {
        if(SharedViewCodes.GetErrors(messages).Count > 0) {
            messages.Add(SharedViewCodes.NotPossible);
            return messages;
        }
        var result = await action.Invoke();
        messages.AddRange(result.Messages);
        return messages;
    }

}
