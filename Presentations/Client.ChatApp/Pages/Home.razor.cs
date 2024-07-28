using Client.ChatApp.Extensions;
using Client.ChatApp.Protos;
using Client.ChatApp.Protos.Users;
using Client.ChatApp.Services;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Shared.Server.Dtos.Chat;
using Shared.Server.Dtos.User;
using Shared.Server.Extensions;

namespace Client.ChatApp.Pages;
/// <summary>
/// The purpose of this class is to handle those user are new and get them online status
/// </summary>
public class HomeViewHandler : ComponentBase , IAsyncDisposable {

    // =============================== init by DI
    [Inject]
    private NavigationManager NavManager { get; set; } = null!;

    [Inject]
    private GrpcChannel GrpcChannel { get; set; } = null!;

    [Inject]
    private UserSelectionObserver? UserSelectionObserver { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationState { get; set; } = null!;
    private async Task<string> GetUserIdAsync() {
        return ( await AuthenticationState ).User.Claims.Where(x => x.Type == "UserIdentifier").FirstOrDefault()?.Value ?? String.Empty;
    }
    //==================================== private fields and props
    private UserQeriesRPCs.UserQeriesRPCsClient Queries => new(GrpcChannel);

    //private HubConnection _hubConnection = null!;
    private HubConnection _onlineStatusHub = null!;
    //======================================= Visible fields or props in razor
    protected LinkedList<OnlineUserDto> Users = new();
    protected void GoToChatPage(string userId) => NavManager.NavigateTo("/Chats/" +  userId);

    //============================ basic blazor methods
    protected override async Task OnInitializedAsync() {
        try {
            Users.Clear();
            Users = await GetUsers();
            ChangeOnlineUserStatus(await GetUserIdAsync() , true);
            await OnlineStatusHubConfigAsync();
        }
        catch(Exception ex) { 
            Console.WriteLine("Home : init : " + ex.Message);
        }

    }

    protected void OnChatItemSelected(UserBasicInfoDto item) {
        if(UserSelectionObserver != null) {
            UserSelectionObserver.SelectedItem(new ChatItemDto() {
                DisplayName = item.DisplayName,
                LogoUrl = item.ImageUrl,
                ReceiverId = item.Id.AsGuid(),
                Id = Guid.NewGuid(),
                UnReadMessages = 0
            } , false , true);
            NavManager.NavigateTo("/Chats");
        }
    }

    //==================================== private methods
    private async Task OnlineStatusHubConfigAsync() {
        var absUri = "https://localhost:7001/OnlineStatusHub";
        _onlineStatusHub = new HubConnectionBuilder().WithUrl(NavManager.ToAbsoluteUri(absUri)).Build();
        _onlineStatusHub.On<string , bool>("GetOnlineStatus" , async (userId , isActive) => {
            ChangeOnlineUserStatus(userId, isActive);
            await InvokeAsync(StateHasChanged);
        });
        _onlineStatusHub.On<OnlineUserDto>("GetOnlineUserInfo" , async (user) => {
            Users.AddFirst(user);
            await InvokeAsync(StateHasChanged);
        });
        await _onlineStatusHub.StartAsync();
    }



    private async Task<LinkedList<OnlineUserDto>> GetUsers()
        => await (Queries.GetUsersWithOnlineStatus(new Empty())).ToLinkedListAsync<OnlineUserMsg , OnlineUserDto>();

    private void ChangeOnlineUserStatus(string userId , bool isActive) {
        var onlineUser =  (Users.Where(x => x.BasicInfo.Id == userId).FirstOrDefault());
        if(onlineUser is not null) {
            Users.Find(onlineUser)!.Value = onlineUser with { IsOnline = isActive };
        }
    }

    //============================== Disposable
    public async ValueTask DisposeAsync() {
        if(_onlineStatusHub != null) {
            await _onlineStatusHub.StopAsync();
        }      
       await GrpcChannel.ShutdownAsync();
    }
}
