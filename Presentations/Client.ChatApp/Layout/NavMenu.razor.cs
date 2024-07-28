using Client.ChatApp.Pages.Chat;
using Client.ChatApp.Protos;
using Client.ChatApp.Services;
using Grpc.Net.Client;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Server.Dtos.Chat;
using Shared.Server.Extensions;

namespace Client.ChatApp.Layout;

public class NavMenuViewHandler : ComponentBase , IAsyncDisposable {

    //========================================

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

    protected void OnItemClicked(ChatItemDto selectedItem) {
        if(isChatsMenuSelected) {
            UserSelectionObserver.SelectedItem(selectedItem);
            CurrentItem = selectedItem;
        }
    }

    private async Task<string> GetMyIdAsync()
        => ( await AuthState ).User.Claims.Where(x => x.Type == "UserIdentifier").FirstOrDefault()?.Value ??
        Guid.Empty.ToString();
    //=========================== basic blazor methods
    protected override async Task OnInitializedAsync() {
        try {
            var identity = (await AuthState).User.Identity;
            if(identity != null && identity.IsAuthenticated) {
                Cloud = new() {
                    DisplayName = "Cloud" ,
                    Id = Guid.NewGuid() ,
                    LogoUrl = "" ,
                    ReceiverId = ( await GetMyIdAsync() ).AsGuid()
                };
                ChatAccounts = ( await ChatItemQueries.GetAllAsync(new()) ).Items.Adapt<LinkedList<ChatItemDto>>();
                //CurrentItem = Cloud;
            }
            UserSelectionObserver.OnChangeSelection += ChangeNavbar;
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
        UserSelectionObserver.SelectedItem(CurrentItem , true, false);
    }
    protected void GoToHomePage() {
        menuName = "ChapApp";
        isChatsMenuSelected = false;
        UserSelectionObserver.SelectedItem(null , true , false);
    }

    protected string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;
    protected void ToggleNavMenu() {
        collapseNavMenu = !collapseNavMenu;
    }

    //==========================

    private void ChangeNavbar() {
        if(UserSelectionObserver.IsGoingToHome is false) {
            isChatsMenuSelected = true;
            menuName = "Home";
        }
        if(UserSelectionObserver.WasUserAtHomePage) {
            var findUser = ChatAccounts.FirstOrDefault(x=> x.ReceiverId == UserSelectionObserver.Item.ReceiverId);
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
            UserSelectionObserver.SelectedItem(CurrentItem,true, false);
            if(GrpcChannel is not null) {
                await GrpcChannel.ShutdownAsync();
            }
        }
        catch(Exception ex) {
            Console.WriteLine(ex.Message);
        }
    }
}
