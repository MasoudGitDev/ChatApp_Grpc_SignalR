using Client.ChatApp.Pages.Features;
using Client.ChatApp.Protos;
using Google.Protobuf.Collections;
using Microsoft.AspNetCore.Components;

namespace Client.ChatApp.Pages.Dashboard;

public class ContactsViewHandler : ComponentBase {

    //===================== Injects
    [Inject]
    private NavigationManager NavManager { get; set; } = null!;

    [Inject]
    private ContactRPCs.ContactRPCsClient ContactService { get; set; } = null!;

    //private
    private uint _currentPage = 1;


    //==============protected 
    protected uint PageSize = 5;
    protected uint TotalItems = 1;

    protected RepeatedField<ContactItem> _contacts = [];

    protected async Task OnRemoveItem(ContactItem contact) {
        _contacts.Remove(contact);
        await ContactService.RemoveAsync(new RowMsg() { RowId = contact.ContactId.ToString() });
    }
    protected void GoToChat(ContactItem item) {
        NavManager.NavigateTo("/Chats");
    }

    protected void GoToDialog(ContactItem contact) {
        NavManager.NavigateTo("/Chats");
    }

   
    protected override async Task OnInitializedAsync() {
        _contacts = ( await ContactService.GetContactsAsync(new Empty() { }) ).Items;
        TotalItems = (uint) _contacts.Count;
    }

    //====================

    protected Pagination pagination  = new();

    protected async Task NotifyOnChangePage((uint pageSize , uint currentPage) info) {
        _currentPage = info.currentPage;
        PageSize = info.pageSize;
        await InvokeAsync(StateHasChanged);
    }

}
