using Client.ChatApp.Protos;
using Client.ChatApp.Protos.ChatMessages;
using Client.ChatApp.Services;
using Grpc.Net.Client;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Shared.Server.Constants;
using Shared.Server.Dtos.Chat;
using Shared.Server.Dtos.User;
using Shared.Server.Extensions;

namespace Client.ChatApp.Pages.Chat;

public class ChatMessagesViewHandler : ComponentBase, IAsyncDisposable {


    //================================ injections

    [Inject]
    private UserSelectionObserver UserSelection { get; set; } = new();

    [Inject]
    private GrpcChannel GrpcChannel { get; set; } = null!;

    [Inject]
    private NavigationManager NavManager { get; set; } = null!;

    [CascadingParameter]
    private Task<AuthenticationState> AuthStateAsync { get; set; } = null!;

    //===================================
    private ChatMessageCommandRPCs.ChatMessageCommandRPCsClient Commands => new(GrpcChannel);
    private ChatMessageQueryRPCs.ChatMessageQueryRPCsClient Queries => new(GrpcChannel);
    private ChatItemQueryRPCs.ChatItemQueryRPCsClient ChatItemQueries => new(GrpcChannel);

    private HubConnection _ChatHubConnection;

    //===================================
    protected ChatItemDto? SelectedItem { get; set; }
    protected Guid MyId { get; private set; }
    protected string TypeStatus = String.Empty;

    private async Task<string> GetMyIdAsync()
        => ( await AuthStateAsync ).User.Claims.Where(x => x.Type == "UserIdentifier").FirstOrDefault()?.Value ??
        Guid.Empty.ToString();

    private async Task<string> GetMyDisplayNameAsync()
      => ( await AuthStateAsync ).User.Claims.Where(x => x.Type == TokenKeys.DisplayName).FirstOrDefault()?.Value ??
      Guid.Empty.ToString();

    protected ChatItemDto Cloud { get; private set; } = null!;
    //============================= Basic Blazor Methods
    protected override async Task OnInitializedAsync() {

        try {          
            MyId = ( await GetMyIdAsync() ).AsGuid();
            UserSelection.OnChangeSelection += OnChangeSelectedItem;
            await GetMessagesAsync();

            await SetHubConnectionAsync();

        }
        catch(Exception ex) {
            Console.WriteLine("chatMessages : init : " + ex.Message);
        }
    }

    //=========================== 

    protected SendMessageDto MessageItem = new();
    protected string MessageContent = String.Empty;
    protected LinkedList<GetMessageDto> Messages { get; private set; } = new();
    protected async Task SendMessageAsync() {
        if(SelectedItem is null) {
            Console.WriteLine("ChatMessages : SendMessageAsync : " + SelectedItem);
            return;
        }
        MessageItem = new() {
            Id = Guid.NewGuid().ToString() ,
            ChatItemId = SelectedItem.Id.ToString() ,
            SenderId = await GetMyIdAsync() ,
            ReceiverId = SelectedItem.ReceiverId.ToString() ,
            Content = MessageContent
        };
        var getMessage = new GetMessageDto() {
            Id = MessageItem.Id.AsGuid(),
            ChatItemId = MessageItem.ChatItemId.AsGuid() ,
            SenderId = MessageItem.SenderId.AsGuid() ,
            IsSeen = false ,
            IsSent = true ,
            Content = MessageContent ,
            FileUrl = ""
        };
        var result =  await Commands.SendAsync(MessageItem.Adapt<SendMessageMsg>());
        if(result.IsSuccessful) {
            await _ChatHubConnection.InvokeAsync("SendMessage" , getMessage);
            var chatItemId = (await GetChatItemAsync(SelectedItem.ReceiverId)).Id;
            var senderInfo = new UserBasicInfoDto(MyId.ToString() , "" , await GetMyDisplayNameAsync());
            var receiverInfo = new UserBasicInfoDto(SelectedItem.ReceiverId.ToString() , "" , SelectedItem.DisplayName );
            await _ChatHubConnection.InvokeAsync("SendChatItem" ,senderInfo, receiverInfo , chatItemId);
            MessageContent = "";
        }     
        
        Console.WriteLine(result);
    }

    //======================================================
    private async Task SetHubConnectionAsync() {
        var url = "https://localhost:7001/chatMessageHub";
        _ChatHubConnection = new HubConnectionBuilder().WithUrl(NavManager.ToAbsoluteUri(url)).Build();
        _ChatHubConnection.On<GetMessageDto>("ReceiveMessage" , async (message) => {
            if(message.ChatItemId == SelectedItem.Id) {
                Messages.AddLast(message);
            }          
            await InvokeAsync(StateHasChanged);
        });
        _ChatHubConnection.On<bool>("GetTypingStatus" , async (isTyping) => {
            TypeStatus = isTyping ? "درحال نوشتن..." : "";
            await InvokeAsync(StateHasChanged);
        });
        await _ChatHubConnection.StartAsync();
    }



    private async Task GetMessagesAsync() {
        Messages.Clear();
        if(SelectedItem is null) {
            SelectedItem = UserSelection.Item ?? new();
            Console.WriteLine("ChatMessages : GetMessagesAsync : " + SelectedItem);
        }
        var chatItem = await GetChatItemAsync(SelectedItem.ReceiverId);
        var result = (await Queries.GetMessagesAsync(new() { Id = chatItem.Id.ToString()}));
        foreach(var item in result.Messages) {
            Messages.AddLast(item.Adapt<GetMessageDto>());
        }
    }

    private async Task<ChatItemDto> GetChatItemAsync(Guid receiverId)
        => ( await ChatItemQueries.GetItemAsync(new() { Id = receiverId.ToString() }) )
            .Items.FirstOrDefault().Adapt<ChatItemDto>();

    private async void OnChangeSelectedItem() {
        try {
            if(SelectedItem is null) {
                Console.WriteLine("ChatMessages : OnChangeSelectedItem : " + SelectedItem);
                return;
            }
            SelectedItem = UserSelection.Item;
            await GetMessagesAsync();
            await InvokeAsync(StateHasChanged);
        }
        catch(Exception ex) {
            Console.WriteLine(ex.Message);
        }
    }

    protected async Task OnChangeMessageContent(ChangeEventArgs  changeEventArgs) {
        // Update MessageContent with the latest value from the event args
        MessageContent = changeEventArgs.Value?.ToString() ?? String.Empty;

        // Check if the user is typing
        bool isTyping = !string.IsNullOrWhiteSpace(MessageContent);

        // Invoke the method on the hub
        await _ChatHubConnection.InvokeAsync("SetTypingStatus" , isTyping);
    }

    public async ValueTask DisposeAsync() {
        try {
            UserSelection.OnChangeSelection -= OnChangeSelectedItem;
            if(GrpcChannel is not null) {
                await GrpcChannel.ShutdownAsync();
            }
        }
        catch(Exception ex) {
            Console.WriteLine(ex.Message);
        }
    }
}
