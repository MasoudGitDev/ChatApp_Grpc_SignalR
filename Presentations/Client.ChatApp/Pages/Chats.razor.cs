using Client.ChatApp.Layout;
using Client.ChatApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Server.Dtos;
using Shared.Server.Dtos.Chat;
using Shared.Server.Dtos.User;

namespace Client.ChatApp.Pages;
public class ChatsViewHandler : ComponentBase, IDisposable {

    //======================== injections
    //[Inject]
    //private GrpcChannel grpcChannel { get; set; } = null!;

    [Inject]
    private NavigationManager NavManager { get; set; } = null!;

    [Inject]
    private UserSelectionObserver SelectedOnlineUserObserver { get; set; }=null!;

    [CascadingParameter]
    public Task<AuthenticationState> AuthStateAsync { get; set; } = null!;

    //====================== route param


    //======================== visible props
    protected ChatAccountItems Sidebar { get; set; } = null!;
    protected LinkedList<ChatMessageDto> Messages { get; set; } = new();
    protected ChatAccountDto SelectedItem = null!;

    //======================= private props

    private async Task<string> GetMyIdAsync()
        => ( await AuthStateAsync ).User.Claims.Where(x => x.Type == "UserIdentifier").FirstOrDefault()?.Value ?? "";

    //private HubConnection _hubConnection
    //    => new HubConnectionBuilder().WithUrl(navManager.ToAbsoluteUri("https://localhost:7001/chatMessageHub")).Build();

    //======================= basic blazor methods
    protected override async Task OnInitializedAsync() {
        await CheckSelectItemAsync(SelectedOnlineUserObserver.Item);
        SelectedOnlineUserObserver.OnChangeSelection += OnGetOnlineUserItem;
    }


    //==================== visible methods
    protected void OnChatItemSelected(ChatAccountDto item) {
        SelectedItem = item;
    }

    public void Dispose() {
        SelectedOnlineUserObserver.OnChangeSelection -= OnGetOnlineUserItem;
    }

    protected void OnGetOnlineUserItem() {

        StateHasChanged();
    }
    //=================
    private async Task CheckSelectItemAsync(UserBasicInfoDto? item) {

        if(item is null) {
            SelectedItem = new() {
                FullName = "فضای ابری" ,
                UserId = Guid.Parse(await GetMyIdAsync())
            };
        }
        else {
            SelectedItem = new() {
                FullName = item.DisplayName ,
                UserId = Guid.Parse(item.Id) ,
                LogoUrl = item.ImageUrl ,
            };
        }

       
    }
}
