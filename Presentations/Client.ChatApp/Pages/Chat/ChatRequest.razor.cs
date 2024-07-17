using Client.ChatApp.Constants;
using Client.ChatApp.Protos;
using Google.Protobuf.Collections;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Server.ChatApp.Protos;
using Shared.Server.Models.Results;

namespace Client.ChatApp.Pages.Chat;
public class ChatRequestVewHandler : ComponentBase {

    //====================== injects
    [Inject]
    private ChatRequestQueryRPCs.ChatRequestQueryRPCsClient Queries { get; set; } = null!;

    [Inject]
    private ChatRequestCommandsRPCs.ChatRequestCommandsRPCsClient Commands { get; set; } =null!;

    [Inject]
    private NavigationManager NavManager { get; set; } = null!;
    //=======================
    protected LinkedList<MessageInfo> Errors = new();
    protected ChatRequestTab CurrentTab = ChatRequestTab.Received;
    protected string IsTabSelected(ChatRequestTab tab) => CurrentTab == tab ? "selected" : "";
    protected RepeatedField<ChatRequestItemMsg> GetItems() {
        if(CurrentTab == ChatRequestTab.Sent) {
            return SendItems;
        }
        else {
            return ReceiveItems;
        }
    }
    protected void ChangeTab(ChatRequestTab tab) {
        CurrentTab = tab;
    }

    //======================
    private RepeatedField<ChatRequestItemMsg> ReceiveItems = [];
    private RepeatedField<ChatRequestItemMsg> SendItems = [];

    

    private HubConnection? _hubConnection;

    //=======================

    protected async Task OnDeleteAsync(ChatRequestItemMsg item , ChatRequestTab tab) {
        if(tab == ChatRequestTab.Received) {
            ReceiveItems.Remove(item);
        }
        else {
            SendItems.Remove(item);
        }      
        await Commands.DeleteAsync(new ChatRequestMsg() { ChatRequestId = item.ChatRequestId });
    }

    /// <summary>
    ///  Only receivers can block the requests.
    /// </summary>
    protected async Task OnBlockAsync(ChatRequestItemMsg item) {
        ReceiveItems.Remove(item);
        await Commands.BlockAsync(new ChatRequestMsg(){ ChatRequestId = item.ChatRequestId });        
    }

    /// <summary>
    ///  Only receivers can accept the requests.
    /// </summary>
    protected async void OnAcceptAsync(ChatRequestItemMsg item) {
        ReceiveItems.Remove(item);
        await Commands.AcceptAsync(new ChatRequestMsg() { ChatRequestId = item.ChatRequestId });
    }


    protected override async Task OnInitializedAsync() {
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
