using Client.ChatApp.Protos.Users;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;

namespace Client.ChatApp.Layout;

public class MainLayoutViewHandler : LayoutComponentBase, IAsyncDisposable {


    //================= injections
    [Inject]
    private GrpcChannel GrpcChannel { get; set; } = null!;

    [Inject]
    private NavigationManager NavManager { get; set; } = null!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = null!;

    [CascadingParameter]
    private Task<AuthenticationState> AuthState { get; set; } = null!;
    //=====================================================
    
    private OnlineUserCommandsRPCs.OnlineUserCommandsRPCsClient OnlineUserCommands => new(GrpcChannel);

    //====================
    protected bool IsOnline = true;
    private HubConnection _hubConnection = null! ;
    //=============================== basic blazor methods
    protected override async Task OnInitializedAsync() {
        try {
            await TabVisibilityListener();
            await HubConnectionSetupAsync();          
        }
        catch(Exception ex) {
            Console.WriteLine("MainLayoutViewHandler : " + ex.Message);
        }

    }
    //=======================privates

    private async Task HubConnectionSetupAsync() {
        var absUri = "https://localhost:7001/OnlineStatusHub";
        _hubConnection = new HubConnectionBuilder().WithUrl(NavManager.ToAbsoluteUri(absUri)).Build();
        _hubConnection.On<string,bool>("GetOnlineStatus" , async (_ , isActive) => {
            IsOnline = isActive;
            await InvokeAsync(StateHasChanged);
        });       
        await _hubConnection.StartAsync();
        //====== make user online at first loading
        var userId = await GetUserIdAsync();
        await _hubConnection!.InvokeAsync("SetOnlineStatus" , userId , true);
        await OnlineUserCommands.CreateOrUpdateAsync(new Protos.Empty());
    }

    private async Task<string> GetUserIdAsync() {
        return (await AuthState).User.Claims.Where(x=>x.Type == "UserIdentifier").FirstOrDefault()?.Value ?? String.Empty;
    }

    //============================ js controller
    [JSInvokable]
    public async Task OnTabVisibilityChanged(bool isVisible) {
        if(isVisible) {
            await OnlineUserCommands.CreateOrUpdateAsync(new Protos.Empty());
            IsOnline = true;
        }
        else {
            await OnlineUserCommands.RemoveAsync(new Protos.Empty());
            IsOnline = false;
        }
        await _hubConnection!.InvokeAsync("SetOnlineStatus" , await GetUserIdAsync() , isVisible);
    }
    private async Task TabVisibilityListener() {
        await JSRuntime.InvokeVoidAsync("AddTabVisibilityListener" , DotNetObjectReference.Create(this));
    }
    //=============================== dispose
    public async ValueTask DisposeAsync() {
        if(_hubConnection is not null) {
            await _hubConnection.StopAsync();
            await _hubConnection.DisposeAsync();
        }
        GrpcChannel.Dispose();
        await GrpcChannel.ShutdownAsync();
        await OnlineUserCommands.RemoveAsync(new Protos.Empty());
        IsOnline = false;
    }
}
