using Client.ChatApp.Protos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
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
    private ChatRequestRPCs.ChatRequestRPCsClient RequestService { get; set; } = null!;

    [CascadingParameter]
    private Task<AuthenticationState> AuthState { get; set; } = null!;

    //========================== Use In View
    protected List<MessageInfo> Messages = [];
    protected bool IsAnyError = false;   
    protected BoolType IsMe = BoolType.None;
    protected bool CanShowButton = false;
    protected string ButtonName = string.Empty;
    //================================ private fields
    private string ReceiverId =  String.Empty;
    //====================== protected Methods
    protected override async Task OnInitializedAsync() {
        try {
            Messages.Clear();
            var result = await ContactService.IsInContactsAsync(new ContactMsg() { ProfileId = ProfileId });      
            CheckContactResult(result);            
        }
        catch(Exception ex) {
            Console.WriteLine("OnInitializedAsync : " + ex.Message);
            Messages.Add(new MessageInfo() { Code = "Loading-Error" , Description = "The page not load Completely." , Type = MessageType.Error });
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
            await SendChatRequestAsync();
            return;
        }
        if(ButtonName == ProfileViewConstants.ConfirmBtn) {
            
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
    private async Task<string> GetIdByClaimAsync() 
        => ( await AuthState ).User.Claims.Where(x => x.Type == "UserIdentifier").FirstOrDefault()?.Value ?? string.Empty;
    private void CheckContactResult(ContactResult result) {
        foreach(var message in result.Messages) {
            Messages.Add(message);
            (ButtonName,CanShowButton) = ProfileViewConstants.ApplyCodeResult(message.Code);
        }
        IsAnyError = Messages.Count > 0;
        ReceiverId = result.ContactInfo.UserId;
    }
    private async Task SendChatRequestAsync() {
        if(Errors.Count > 0) {
            Messages.Add(new MessageInfo() { Code = "NotPossible" , Description = "Because of other errors" , Type = MessageType.Error });
            return;
        }
        var result = await RequestService.RequestAsync(new UserMsg() { UserId = ReceiverId });
        Messages.AddRange(result.Messages);
    }
    private List<MessageInfo> Errors => Messages.Where(x=> x.Type == MessageType.Error).Select(x=>x).ToList(); 
}
