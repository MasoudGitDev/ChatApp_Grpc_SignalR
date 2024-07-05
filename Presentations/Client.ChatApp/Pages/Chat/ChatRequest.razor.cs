using Client.ChatApp.Constants;
using Client.ChatApp.Protos;
using Google.Protobuf.Collections;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Shared.Server.Models.Results;

namespace Client.ChatApp.Pages.Chat;
public class ChatRequestVewHandler : ComponentBase {

    //====================== injects
    [Inject]
    private ChatRequestQueryRPCs.ChatRequestQueryRPCsClient Queries { get; set; } = null!;

    [Inject]
    private NavigationManager NavManager { get; set; } = null!;
    //=======================

    protected ChatRequestTab _currentTab = ChatRequestTab.Received;
    protected string IsTabSelected(ChatRequestTab tab) => _currentTab == tab ? "selected" : "";
    protected RepeatedField<ChatRequestItemMsg> GetItems() {
        if(_currentTab == ChatRequestTab.Sent) {
            return SendItems;
        }
        if(_currentTab == ChatRequestTab.Received) {
            return ReceiveItems;
        }
        return [];
    }
    protected void ChangeTab(ChatRequestTab tab) {
        _currentTab = tab;
    }

    //======================
    private RepeatedField<ChatRequestItemMsg> ReceiveItems = [];
    private RepeatedField<ChatRequestItemMsg> SendItems = [];

    private HubConnection? _hubConnection;

    //=======================



    protected override async void OnInitialized() {    
        await CreateReceiveItemsAsync();
        await CreateSendItemsAsync();
        await SetHubConfigAsync();
    }

    private async Task SetHubConfigAsync() {
        _hubConnection = new HubConnectionBuilder().WithUrl(NavManager.ToAbsoluteUri("")).Build();
        _hubConnection.On<ChatRequestItem>("GetReceiveRequests" , async item => {
            ReceiveItems.Add(item.Adapt<ChatRequestItemMsg>());
            await InvokeAsync(StateHasChanged);
        });
        _hubConnection.On<ChatRequestItem>("GetSendRequests" , async item => {
            SendItems.Add(item.Adapt<ChatRequestItemMsg>());
            await InvokeAsync(StateHasChanged);
        });
        await _hubConnection.StartAsync();
    }

    private async Task CreateReceiveItemsAsync() {
        ReceiveItems.Clear();
        ReceiveItems = ( await Queries.GetReceiveRequestsAsync(new Empty()) ).Items;
    }

    private async Task CreateSendItemsAsync() {
        SendItems.Clear();
        SendItems = ( await Queries.GetSendRequestsAsync(new Empty()) ).Items;
    }

}
