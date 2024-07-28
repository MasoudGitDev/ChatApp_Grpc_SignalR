using Client.ChatApp.Protos.ChatMessages;
using Client.ChatApp.Services;
using Grpc.Net.Client;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Shared.Server.Dtos.Chat;
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
    private HubConnection _ChatHubConnection;

    //===================================
    protected ChatItemDto? SelectedItem { get; set; }
    protected Guid MyId { get; private set; }

    private async Task<string> GetMyIdAsync()
        => ( await AuthStateAsync ).User.Claims.Where(x => x.Type == "UserIdentifier").FirstOrDefault()?.Value ??
        Guid.Empty.ToString();
    protected ChatItemDto Cloud { get; private set; } = null!;
    //============================= Basic Blazor Methods
    protected override async Task OnInitializedAsync() {

        try {
            var url = "https://localhost:7001/chatMessageHub";
            _ChatHubConnection = new HubConnectionBuilder().WithUrl(NavManager.ToAbsoluteUri(url)).Build();
            MyId = ( await GetMyIdAsync() ).AsGuid();
            Cloud = new() {
                DisplayName = "Cloud" ,
                Id = Guid.NewGuid() ,
                LogoUrl = "" ,
                ReceiverId = MyId
            };
            UserSelection.OnChangeSelection += OnChangeSelectedItem;
            SelectedItem = UserSelection.Item ?? Cloud;
            await GetMessagesAsync(SelectedItem.Id.ToString());

            _ChatHubConnection.On<GetMessageDto>("ReceiveMessage" , async (message) => {
                Messages.AddLast(message);
                await InvokeAsync(StateHasChanged);
            });
            await _ChatHubConnection.StartAsync();

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
            MessageContent = "";
        }        
        Console.WriteLine(result);
    }




    private async Task GetMessagesAsync(string chatItemId) {
        Messages.Clear();
        if(SelectedItem is null) {
            SelectedItem = UserSelection.Item ?? Cloud;
            Console.WriteLine("ChatMessages : GetMessagesAsync : " + SelectedItem);
        }
        var result = (await Queries.GetMessagesAsync(new() { Id = chatItemId}));
        foreach(var item in result.Messages) {
            Messages.AddLast(item.Adapt<GetMessageDto>());
        }
    }

    private async void OnChangeSelectedItem() {
        try {
            if(SelectedItem is null) {
                Console.WriteLine("ChatMessages : OnChangeSelectedItem : " + SelectedItem);
                return;
            }
            SelectedItem = UserSelection.Item;
            await GetMessagesAsync(SelectedItem.Id.ToString());
            await InvokeAsync(StateHasChanged);
        }
        catch(Exception ex) {
            Console.WriteLine(ex.Message);
        }
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
