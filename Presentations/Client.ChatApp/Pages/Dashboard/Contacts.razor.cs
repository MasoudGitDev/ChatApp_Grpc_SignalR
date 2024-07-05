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

    //==============protected 
    protected RepeatedField<ContactItem> _contacts = new();

    protected async Task OnRemoveItem(ContactItem contact) {
        _contacts.Remove(contact);
        await ContactService.RemoveAsync(new RowMsg() { RowId = contact.ContactId.ToString() });
    }

    protected void GoToDialog(ContactItem contact) {
        NavManager.NavigateTo("/Chats");
    }

    protected override async Task OnInitializedAsync() {
        _contacts = ( await ContactService.GetContactsAsync(new Empty() { }) ).Items;
    }

    //====================

}
