using Client.ChatApp.Extensions;
using Client.ChatApp.Protos;
using Client.ChatApp.Protos.Users;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Shared.Server.Dtos.User;

namespace Client.ChatApp.Pages;

public class HomeViewHandler : ComponentBase , IAsyncDisposable {

    [Inject]
    private NavigationManager NavManager { get; set; } = null!;

    [Inject]
    private GrpcChannel GrpcChannel { get; set; } = null!;

    private UserQeriesRPCs.UserQeriesRPCsClient Queries;

    private HubConnection _hubConnection;

    protected LinkedList<UserBasicInfoDto> Users = new();
    protected void GoUserProfile(string profileId) => NavManager.NavigateTo("/Profile/" +  profileId);

    protected override async Task OnInitializedAsync() {
        Users.Clear();
        Queries = new(GrpcChannel);
        Users = await GetUsers();
        _hubConnection = new HubConnectionBuilder().WithUrl(NavManager.ToAbsoluteUri("https://localhost:7001/SignUpHub")).Build();        
        _hubConnection.On<UserBasicInfoDto>("GetNewUser" , async (user) => {
            Users.AddFirst(user);
            await InvokeAsync(StateHasChanged);
        });
        await _hubConnection.StartAsync();
    }


    private async Task<LinkedList<UserBasicInfoDto>> GetUsers()
        => await (Queries.GetUsers(new Protos.Empty()))
        .ToLinkedListAsync<UserBasicInfoMsg,UserBasicInfoDto>();

    public async ValueTask DisposeAsync() {
       await GrpcChannel.ShutdownAsync();
    }
}
