using Client.ChatApp.Pages.Chat;
using Client.ChatApp.Protos;
using Client.ChatApp.Services;
using Domains.Chats.Item.Aggregate;
using Grpc.Net.Client;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Shared.Server.Constants;
using Shared.Server.Dtos.Chat;
using Shared.Server.Dtos.User;
using Shared.Server.Extensions;

namespace Client.ChatApp.Layout;

public class NavMenuViewHandler : ComponentBase, IAsyncDisposable {

    //========================================
    [Inject]
    private NavigationManager NavManager { get; set; } = null!;

    [Inject]
    private GrpcChannel? GrpcChannel { get; set; }

    [CascadingParameter]
    public Task<AuthenticationState>? AuthState { get; set; }

    [Inject]
    private UserSelectionObserver UserSelectionObserver { get; set; } = new();

    //=========================================
    private ChatItemQueryRPCs.ChatItemQueryRPCsClient ChatItemQueries => new(GrpcChannel);

    //=========================================

    private readonly ChatMessages ChatMessagePage = new();
    protected ChatItemDto? CurrentItem { get; private set; }
    protected ChatItemDto Cloud { get; private set; } = null!;
    protected LinkedList<ChatItemDto> ChatAccounts = new();

    protected bool collapseNavMenu = true;
    protected string WhenOver100(int number) => number >= 100 ? "99+" : number.ToString();

    protected bool isChatsMenuSelected = false;
    protected string menuName = "ChatApp";
    protected string UserNameClaim = string.Empty;
    private HubConnection _ChatHubConnection;

    protected void OnItemClicked(ChatItemDto selectedItem) {
        if(isChatsMenuSelected) {
            UserSelectionObserver.SelectedItem(selectedItem);
            CurrentItem = selectedItem;
        }
    }
    private async Task<string> GetUserNameClaimAsync()
       => ( await AuthState ).User.Claims.Where(x => x.Type == TokenKeys.UserName).FirstOrDefault()?.Value ??
       Guid.Empty.ToString();

    private async Task<string> GetMyIdAsync()
        => ( await AuthState ).User.Claims.Where(x => x.Type == TokenKeys.UserId).FirstOrDefault()?.Value ??
        Guid.Empty.ToString();
    //=========================== basic blazor methods
    protected override async Task OnInitializedAsync() {
        try {
            var identity = (await AuthState).User.Identity;
            if(identity != null && identity.IsAuthenticated) {
                Cloud = ( await ChatItemQueries.GetCloudItemAsync(new Empty()) ).Items.FirstOrDefault().Adapt<ChatItemDto>();
                UserNameClaim = $"({await GetUserNameClaimAsync()})";
                ChatAccounts = ( await ChatItemQueries.GetAllAsync(new()) ).Items.Adapt<LinkedList<ChatItemDto>>();
                //CurrentItem = Cloud;
            }
            UserSelectionObserver.OnChangeSelection += ChangeNavbar;
            await SetHubConnectionAsync();
        }
        catch(Exception ex) {
            Console.WriteLine("NavMenu:OnInitializedAsync : " + ex.Message);
        }
    }


    //========================== visible 
    protected void GoToChatsPage() {
        isChatsMenuSelected = true;
        menuName = "Home";
        CurrentItem = Cloud;
        UserSelectionObserver.SelectedItem(CurrentItem , true , false);
    }
    protected void GoToHomePage() {
        menuName = "ChatApp";
        isChatsMenuSelected = false;
        UserSelectionObserver.SelectedItem(null , true , false);
    }

    protected string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

   

    protected void ToggleNavMenu() {
        collapseNavMenu = !collapseNavMenu;
    }

    //==========================

    private async Task SetHubConnectionAsync() {
        var url = "https://localhost:7001/chatMessageHub";
        _ChatHubConnection = new HubConnectionBuilder().WithUrl(NavManager.ToAbsoluteUri(url)).Build();
        _ChatHubConnection.On<UserBasicInfoDto,UserBasicInfoDto,Guid>("ReceiveChatItem" ,
            async (senderInfo , receiverInfo , chatItemId) => {
            await CreateChatItemAsync(senderInfo,receiverInfo,chatItemId);
            await InvokeAsync(StateHasChanged);
        });
        await _ChatHubConnection.StartAsync();
    }  

    private async Task CreateChatItemAsync(UserBasicInfoDto senderInfo , UserBasicInfoDto receiverInfo , Guid chatItemId) {
        if(senderInfo.Id == receiverInfo.Id) { 
            return; // means uses cloud item!
        }
        string MyId =(await GetMyIdAsync());
        bool amISender = senderInfo.Id == MyId;
        var findChatItem = ChatAccounts.FirstOrDefault(x=> x.Id == chatItemId);
        if(findChatItem is not null) {
            ChatAccounts.Remove(findChatItem);
            findChatItem.UnReadMessages += 0;
            ChatAccounts.AddFirst(findChatItem);
            if(amISender) {
                CurrentItem = findChatItem;
            }
        }
        else {
           
            var chatItem = new ChatItemDto() {
                DisplayName = amISender ? receiverInfo.DisplayName : senderInfo.DisplayName ,
                Id = chatItemId ,
                LogoUrl = amISender ? receiverInfo.ImageUrl : senderInfo.ImageUrl ,
                ReceiverId = amISender ? receiverInfo.Id.AsGuid() : senderInfo.Id.AsGuid() ,
                UnReadMessages = amISender ? 0 : 0
            };
            ChatAccounts.AddFirst(chatItem);
            if(amISender) {
                CurrentItem = chatItem;
            }
           
        }
        await Task.CompletedTask;
    }



    private void ChangeNavbar() {
        if(UserSelectionObserver.IsGoingToHome is false) {
            isChatsMenuSelected = true;
            menuName = "Home";
        }
        if(UserSelectionObserver.WasUserAtHomePage) {
            var findUser = ChatAccounts.FirstOrDefault(x=> x.ReceiverId == UserSelectionObserver.Item?.ReceiverId);
            CurrentItem = findUser;
        }
        StateHasChanged();
    }

    private LinkedList<ChatItemDto> FakeData() {
        var list = new LinkedList<ChatItemDto>();
        for(int i = 1 ; i <= 50 ; i++) {
            list.AddLast(new ChatItemDto() {
                DisplayName = "مسعود " + i ,
                Id = Guid.NewGuid() ,
                LogoUrl = "M" + i ,
                ReceiverId = Guid.NewGuid() ,
                UnReadMessages = i
            });
        }
        return list;
    }

    public async ValueTask DisposeAsync() {
        try {
            UserSelectionObserver.OnChangeSelection -= ChangeNavbar;
            CurrentItem = Cloud;
            UserSelectionObserver.SelectedItem(CurrentItem , true , false);
            if(GrpcChannel is not null) {
                await GrpcChannel.ShutdownAsync();
            }
        }
        catch(Exception ex) {
            Console.WriteLine(ex.Message);
        }
    }
}
