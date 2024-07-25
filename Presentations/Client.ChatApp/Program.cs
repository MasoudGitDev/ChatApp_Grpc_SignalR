using Blazored.LocalStorage;
using Client.ChatApp;
using Client.ChatApp.Protos;
using Client.ChatApp.Protos.ChatMessages;
using Client.ChatApp.Protos.Users;
using Client.ChatApp.Services;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Server.ChatApp.Protos;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7001") });

//register grpc channel
var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
builder.Services.AddScoped(sp => {
    return httpClient;
});
using var gRPCChannel = GrpcChannel.ForAddress("https://localhost:7001" , new GrpcChannelOptions {
    HttpClient = httpClient
});

builder.Services.AddScoped(x => gRPCChannel);
builder.Services.AddScoped(services => new ChatMessageCommandRPCs.ChatMessageCommandRPCsClient(gRPCChannel));
builder.Services.AddScoped(services => new ChatRequestCommandsRPCs.ChatRequestCommandsRPCsClient(gRPCChannel));
builder.Services.AddScoped(services =>new ChatRequestQueryRPCs.ChatRequestQueryRPCsClient(gRPCChannel));
builder.Services.AddScoped(service => new AccountRPCs.AccountRPCsClient(gRPCChannel));
builder.Services.AddScoped(service => new ContactRPCs.ContactRPCsClient(gRPCChannel));
builder.Services.AddScoped(service => new SharedRpcs.SharedRpcsClient(gRPCChannel));
builder.Services.AddScoped(service => new OnlineUserCommandsRPCs.OnlineUserCommandsRPCsClient(gRPCChannel));

builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthenticationStateProvider , AuthStateProvider>();

builder.Services.AddSingleton(_ => new UserSelectionObserver());

await builder.Build().RunAsync();
