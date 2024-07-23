using Client.ChatApp.Protos.Users;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Components;
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
    //=====================================================

    private UserCommandsRPCs.UserCommandsRPCsClient OnlineCommands => new(GrpcChannel);

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
        _hubConnection.On<bool>("GetOnlineStatus" , async (isActive) => {
            IsOnline = isActive;
            await InvokeAsync(StateHasChanged);
        });       
        await _hubConnection.StartAsync();
        //====== create online at first load
        await _hubConnection!.InvokeAsync("SetOnlineStatus" , true);
        await OnlineCommands.CreateOrUpdateAsync(new Protos.Empty());
    }

    //============================ js controller
    [JSInvokable]
    public async Task OnTabVisibilityChanged(bool isVisible) {
        if(isVisible) {
            await OnlineCommands.CreateOrUpdateAsync(new Protos.Empty());
            IsOnline = true;
        }
        else {
            await OnlineCommands.RemoveAsync(new Protos.Empty());
            IsOnline = false;
        }
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
        await OnlineCommands.RemoveAsync(new Protos.Empty());
        IsOnline = false;
    }
}
