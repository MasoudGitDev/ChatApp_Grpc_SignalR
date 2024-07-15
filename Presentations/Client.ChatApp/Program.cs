using Blazored.LocalStorage;
using Client.ChatApp;
using Client.ChatApp.Protos;
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

builder.Services.AddScoped<ChatMessageRPCs.ChatMessageRPCsClient>(services => {
    return new ChatMessageRPCs.ChatMessageRPCsClient(gRPCChannel);
});

builder.Services.AddScoped<ChatRequestCommandsRPCs.ChatRequestCommandsRPCsClient>(services => {
    return new ChatRequestCommandsRPCs.ChatRequestCommandsRPCsClient(gRPCChannel);
});

builder.Services.AddScoped<ChatRequestQueryRPCs.ChatRequestQueryRPCsClient>(services => {
    return new ChatRequestQueryRPCs.ChatRequestQueryRPCsClient(gRPCChannel);
});



builder.Services.AddScoped<AccountRPCs.AccountRPCsClient>(service => {
    return new AccountRPCs.AccountRPCsClient(gRPCChannel);
});

builder.Services.AddScoped<ContactRPCs.ContactRPCsClient>(service => {
    return new ContactRPCs.ContactRPCsClient(gRPCChannel);
});

builder.Services.AddScoped(service => {
    return new SharedRpcs.SharedRpcsClient(gRPCChannel);
});
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthenticationStateProvider , AuthStateProvider>();



await builder.Build().RunAsync();
