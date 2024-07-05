using Domains.Auth.User.Aggregate;
using Domains.Chats.Contacts.Aggregate;
using Domains.Chats.Requests.Aggregate;
using Microsoft.EntityFrameworkCore;
using Shared.Server.Dtos.Dashboard;
using Shared.Server.Extensions;
using Shared.Server.Models.Results;

namespace Infra.EFCore.Extensions;
internal static class QueriesExtensions {
    public static async Task<List<ChatRequestItem>> ToChatRequestItemsAsync(
        this IQueryable<ChatRequest> querySource ,
        Func<Guid , Task<AppUser>> findUserAction ,
        bool getRequesters = true) {
        try {
            var chatRequests = await querySource.ToListAsync();
            var list = new List<ChatRequestItem>();

            foreach(var item in chatRequests) {
                var userId = getRequesters ? item.RequesterId : item.ReceiverId;
                var currentUser = await findUserAction.Invoke(userId);

                list.Add(new ChatRequestItem(
                    item.Id ,
                    currentUser.Id ,
                    currentUser.DisplayName ,
                    item.RequestedAt ,
                    currentUser.ImageUrl));
            }

            return list;
        }
        catch(Exception ex) {
            Console.WriteLine(ex.ToString());
            return [];
        }
    }


    public static async Task<LinkedList<ContactItemDto>> ToContactItemsAsync(this IQueryable<Contact> querySource ,
      Guid myId ,
      Func<Guid , Task<AppUser>> findUserAction) {
        try {
            var contacts = await querySource.ToLinkedListAsync();
            var list = new LinkedList<ContactItemDto>();

            foreach(var item in contacts) {
                var contactId = myId == item.RequesterId ?  item.ReceiverId : item.RequesterId;
                var currentUser = await findUserAction.Invoke(contactId);

                list.AddLast(new ContactItemDto(
                    item.Id ,
                    currentUser.Id ,
                    currentUser.DisplayName ,
                    item.ContactedAt ,
                    currentUser.ImageUrl));
            }
            return list;
        }
        catch(Exception ex) {
            Console.WriteLine(ex.ToString());
            return [];
        }
    }

}
