using Client.ChatApp.Extensions;
using Client.ChatApp.Protos;
using Client.ChatApp.Protos.Users;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Shared.Server.Dtos.User;

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

    //==================================== private fields and props
    private UserQeriesRPCs.UserQeriesRPCsClient Queries => new(GrpcChannel);

    private HubConnection _hubConnection = null!;

    //======================================= Visible fields or props in razor
    protected LinkedList<OnlineUserDto> Users = new();
    protected void GoUserProfile(string profileId) => NavManager.NavigateTo("/Profile/" +  profileId);

    //============================ basic blazor methods
    protected override async Task OnInitializedAsync() {
        Users.Clear();
        Users = await GetUsers();
        await SignUpHubConfigAsync();
    }


    //==================================== private methods
    private async Task SignUpHubConfigAsync() {
        var absoluteUri = "https://localhost:7001/SignUpHub";
        _hubConnection = new HubConnectionBuilder().WithUrl(NavManager.ToAbsoluteUri(absoluteUri)).Build();
        _hubConnection.On<OnlineUserDto>("GetNewUser" , async (user) => {
            Users.AddFirst(user);
            await InvokeAsync(StateHasChanged);
        });
        await _hubConnection.StartAsync();
    }

    private async Task<LinkedList<OnlineUserDto>> GetUsers()
        => await (Queries.GetUsersWithOnlineStatus(new Protos.Empty())).ToLinkedListAsync<OnlineUserMsg , OnlineUserDto>();


    //============================== Disposable
    public async ValueTask DisposeAsync() {
       await  _hubConnection.StopAsync();
       await _hubConnection.DisposeAsync();
       await GrpcChannel.ShutdownAsync();
    }
}
