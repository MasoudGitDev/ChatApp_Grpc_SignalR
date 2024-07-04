using Client.ChatApp.Constants;
using Client.ChatApp.Protos;
using Google.Protobuf.Collections;
using Microsoft.AspNetCore.Components;

namespace Client.ChatApp.Pages.Chat;
public class ChatRequestVewHandler : ComponentBase {

    //====================== injects
    [Inject]
    private ChatRequestQueryRPCs.ChatRequestQueryRPCsClient Queries { get; set; } = null!;
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




    protected override async void OnInitialized() {
        await CreateReceiveItemsAsync();
        await CreateSendItemsAsync();
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
