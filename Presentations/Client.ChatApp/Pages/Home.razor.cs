using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Shared.Server.Dtos.User;

namespace Client.ChatApp.Pages;

public class HomeViewHandler : ComponentBase {

    [Inject]
    private NavigationManager NavManager { get; set; } = null!;


    private HubConnection _hubConnection;

    protected LinkedList<UserHomeDto> Users = new();
    protected void GoUserProfile(string profileId) => NavManager.NavigateTo("/Profile/" +  profileId);

    protected override async Task OnInitializedAsync() {
        _hubConnection = new HubConnectionBuilder().WithUrl(NavManager.ToAbsoluteUri("https://localhost:7001/SignUpHub")).Build();
        _hubConnection.On<UserHomeDto>("GetNewUser" , async (user) => {
            Users.AddFirst(user);
            await InvokeAsync(StateHasChanged);
        });
        await _hubConnection.StartAsync();
    }
}
