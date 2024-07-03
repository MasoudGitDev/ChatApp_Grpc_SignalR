using Client.ChatApp.Constants;
using Microsoft.AspNetCore.Components;
using Shared.Server.Models.Results;

namespace Client.ChatApp.Pages.Chat;  
public class ChatRequestVewHandler :ComponentBase {

    protected ChatRequestTab _currentTab = ChatRequestTab.Received;
    protected string IsTabSelected(ChatRequestTab tab) => _currentTab == tab ? "selected" : "";
    protected LinkedList<ChatRequestItem> GetItems() {
        if(_currentTab == ChatRequestTab.Sent) {
            return SendItems;
        }
        if(_currentTab == ChatRequestTab.Received) {
            return ReceiveItems;
        }
        return new();
    }
    protected void ChangeTab(ChatRequestTab tab) {
        _currentTab = tab;
    }

    //======================
    private readonly LinkedList<ChatRequestItem> ReceiveItems = new();
    private readonly LinkedList<ChatRequestItem> SendItems = new();
  

 

    protected override void OnInitialized() {
        CreateReceiveItems();
        CreateSendItems();
    }

    private void CreateReceiveItems() {
        ReceiveItems.AddLast(new ChatRequestItem(Guid.NewGuid() , "Ali1" , DateTime.UtcNow));
        ReceiveItems.AddLast(new ChatRequestItem(Guid.NewGuid() , "Ali2" , DateTime.UtcNow));
        ReceiveItems.AddLast(new ChatRequestItem(Guid.NewGuid() , "Ali2" , DateTime.UtcNow));
        ReceiveItems.AddLast(new ChatRequestItem(Guid.NewGuid() , "Ali3" , DateTime.UtcNow));
    }

    private void CreateSendItems() {
        SendItems.AddLast(new ChatRequestItem(Guid.NewGuid() , "Masoud1" , DateTime.UtcNow));
        SendItems.AddLast(new ChatRequestItem(Guid.NewGuid() , "Masoud2" , DateTime.UtcNow));
        SendItems.AddLast(new ChatRequestItem(Guid.NewGuid() , "Masoud3" , DateTime.UtcNow));
        SendItems.AddLast(new ChatRequestItem(Guid.NewGuid() , "Masoud4" , DateTime.UtcNow));
    }

}
