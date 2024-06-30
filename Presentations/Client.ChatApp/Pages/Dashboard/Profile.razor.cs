using Client.ChatApp.Protos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Server.Constants;
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
    protected List<ErrorInfo> Errors = [];
    protected string IsAnyError = "";   
    protected BoolType IsMe = BoolType.None;
    protected bool IsButtonHidden = true;
    protected string ButtonName => ExistInContacts.IsYes() ? "Dialog" : "Request";
    //================================ private fields
    private BoolType ExistInContacts = BoolType.None;
    private string UserId = String.Empty;
    private string ReceiverId =  String.Empty;
    private BoolType IsValidUserName = BoolType.None;
    //====================== protected Methods
    protected override async Task OnInitializedAsync() {
        try {
            var result = await ContactService.IsInContactsAsync(new ContactMsg() { ProfileId = ProfileId });      
            UserId = await GetIdByClaimAsync();
            CheckContactResult(result);            
        }
        catch(Exception ex) {
            Console.WriteLine("OnInitializedAsync : " + ex.Message);
            Errors.Add(new ErrorInfo() { Code = "Loading-Error" , Description = "The page not load Completely." });
        }
    }
    protected async Task OnButtonClick() {
        if(ButtonName == "Dialog" && IsMe.IsYes()) {
            NavManager.NavigateTo("/Chats");
            return;
        }
        if(ButtonName == "Dialog") {
            NavManager.NavigateTo("/Chats");
            return;
        }
        if(ButtonName == "Request") {
            await SendChatRequestAsync();
            return;
        }
    }
    protected void CloseNotification(ErrorInfo model) {
        Errors.Remove(model);
        if(Errors.Count <= 0) {
            IsAnyError = "";
        }
    }

    //============== private Methods
    private async Task<string> GetIdByClaimAsync() 
        => ( await AuthState ).User.Claims.Where(x => x.Type == "UserIdentifier").FirstOrDefault()?.Value ?? string.Empty;
    private void CheckContactResult(ContactResult result) {
        foreach(var message in result.Messages) {
            Errors.Add(message);
            if(message.Code == "Invalid-ProfileId") {
                IsValidUserName = BoolType.No;
            }
            if(message.Code == "NotExist") {
                ExistInContacts = BoolType.No;
            }
            if(message.Code == "NotPossible") {
                IsMe = BoolType.Yes;
            }
        }
        if(IsValidUserName.IsNotNo()) {
            IsValidUserName = BoolType.Yes;
        }
        if(ExistInContacts.IsNotNo()) {
            ExistInContacts = BoolType.Yes;
        }
        IsButtonHidden = ( IsValidUserName.IsYes() && IsMe.IsNotYes() );
        IsAnyError = Errors.Any() ? "hidden" : "";
        ReceiverId = result.ContactInfo.UserId;
    }  
    private async Task SendChatRequestAsync() {
        Errors.Clear();
        var result = await RequestService.RequestAsync(new UsersConnection()
        {
            RequesterId = UserId ,
            ReceiverId = ReceiverId
        });
        if(!result.IsSuccessful) {
            Errors.AddRange(result.Messages);
        }
    }

    //=================================
  
}
