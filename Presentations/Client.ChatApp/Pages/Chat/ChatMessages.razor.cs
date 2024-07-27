﻿using Client.ChatApp.Protos.ChatMessages;
using Client.ChatApp.Services;
using Grpc.Net.Client;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Server.Dtos.Chat;
using Shared.Server.Extensions;

namespace Client.ChatApp.Pages.Chat;

public class ChatMessagesViewHandler : ComponentBase , IAsyncDisposable {


    //================================ injections

    [Inject]
    private UserSelectionObserver UserSelection { get; set; } = new();
    [Inject]
    private GrpcChannel GrpcChannel { get; set; } = null!;

    [CascadingParameter]
    private Task<AuthenticationState> AuthStateAsync { get; set; } = null!;

    //===================================
    private ChatMessageCommandRPCs.ChatMessageCommandRPCsClient Commands => new(GrpcChannel);
    private ChatMessageQueryRPCs.ChatMessageQueryRPCsClient Queries => new(GrpcChannel);

    private async Task<string> GetMyIdAsync()
        => ( await AuthStateAsync ).User.Claims.Where(x => x.Type == "UserIdentifier").FirstOrDefault()?.Value ??
        Guid.Empty.ToString();
    protected ChatItemDto Cloud { get; private set; } = null!;
    //============================= Basic Blazor Methods
    protected override async Task OnInitializedAsync() {

        try {
            Cloud = new() {
                DisplayName = "Cloud" ,
                Id = Guid.NewGuid() ,
                LogoUrl = "" ,
                ReceiverId = ( await GetMyIdAsync() ).AsGuid()
            };
            UserSelection.OnChangeSelection += OnChangeSelectedItem;

            if(UserSelection.Item == null) {
                SelectedItem = Cloud;
            }
            //else {
            //    SelectedItem = UserSelection.Item;
            //}


        }
        catch(Exception ex) { 
            Console.WriteLine("chatMessages : init : " + ex.Message);
        }


       // await GetMessagesAsync();
    }


    protected string DisplayName = "Cloud1";

    public async Task RefreshAsync(ChatItemDto selectedItem) {
        SelectedItem = selectedItem;
        // await GetMessagesAsync();
     
        await Task.CompletedTask;
    }
    //=========================== 
    protected ChatItemDto SelectedItem { get; set; } = null!;
    protected SendMessageDto MessageItem = new();
    protected string MessageContent = String.Empty;
    protected LinkedList<GetMessageDto> Messages { get; private set; } = new();
    protected async Task SendMessageAsync() {
        MessageItem = new() {
            Id = Guid.NewGuid().ToString() ,
            ChatItemId = SelectedItem.Id.ToString() ,
            SenderId = await GetMyIdAsync() ,
            ReceiverId = SelectedItem.ReceiverId.ToString() ,
            Content = MessageContent
        };
        var result =  await Commands.SendAsync(new() {
            Id = MessageItem.Id,
            SenderId = MessageItem.SenderId,
            ChatItemId = MessageItem.ChatItemId,
            Content = MessageItem.Content,
            FileUrl = MessageItem.FileUrl,
            ReceiverId = MessageItem.ReceiverId
        });
        Console.WriteLine(result);
    }



 
    private async Task GetMessagesAsync() {
        Messages.Clear();
        var result = (await Queries.GetMessagesAsync(new() { Id = SelectedItem.Id.ToString() }));
        foreach(var item in result.Messages) {
            Messages.AddLast(item.Adapt<GetMessageDto>());
        }
    }
   
    private void OnChangeSelectedItem() {
        SelectedItem = UserSelection.Item ?? Cloud ;
        StateHasChanged();
    }

    public async ValueTask DisposeAsync() {
        UserSelection.OnChangeSelection -= OnChangeSelectedItem;
        await Task.CompletedTask;
    }
}