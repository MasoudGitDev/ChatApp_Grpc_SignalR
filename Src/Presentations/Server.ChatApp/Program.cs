using Apps.Auth.Users.Queries;
using Apps.Chats;
using Infra.SqlServerWithEF;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;
using Server.ChatApp.GRPCHandlers;
using Server.ChatApp.Hubs;
using Server.ChatApp.Hubs.Chats;
using ChatMessages = Server.ChatApp.ServiceHandlers.ChatMessages;
using ChatItems = Server.ChatApp.ServiceHandlers.ChatItems;
using Users = Server.ChatApp.ServiceHandlers.Users;
using Server.ChatApp.Services.Abstractions;
using Server.ChatApp.Services.Upload;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddScoped((_) => new DBConnectionModel(
//    builder.Configuration["GRPCChatDB"].ThrowIfNullOrWhiteSpace("The <connection-string> can not be NullOrWhiteSpace.")));

builder.Services.AddGrpc().AddJsonTranscoding();
builder.Services.AddGrpcReflection();
builder.Services.AddGrpcSwagger();
builder.Services.AddSwaggerGen(opt => {
    opt.SwaggerDoc("v1" , new OpenApiInfo() { Title = "Jwt Authentication" });
    var openApiSecurityScheme = new OpenApiSecurityScheme(){
        Scheme = "Bearer" ,
        BearerFormat = "JWT" ,
        Description = "Put your jwt token in this textbox below !" ,
        In = ParameterLocation.Header ,
        Name = "Jwt Authentication" ,
        Type = SecuritySchemeType.Http ,
        Reference = new OpenApiReference{
            Id = "Bearer" ,
            Type = ReferenceType.SecurityScheme
        }
    };
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement() {
        {openApiSecurityScheme , Array.Empty<string>() }
    });
    opt.AddSecurityDefinition("Bearer" , openApiSecurityScheme);

    //var filePath = Path.Combine(System.AppContext.BaseDirectory, "Server.xml");
    //opt.IncludeXmlComments(filePath);
    //opt.IncludeGrpcXmlComments(filePath , includeControllerXmlComments: true);
});

builder.Services.AddTransient<IFileUploadService , UploadUserLogo>();

builder.Services.AddResponseCompression(opts => {
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
          new[] { "application/octet-stream" });
});

builder.Services.AddEFCoreService();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSignalR();

builder.Services.AddMediatR((config) => {
    config.RegisterServicesFromAssemblies(
        AppsChatsAssembly.Assembly , typeof(GetUsersBasicInfo).Assembly
    );
});

//builder.Services.AddGrpcClient<AdService.AdServiceClient>(o => o.Address = new Uri("https://localhost:7223"));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
if(app.Environment.IsDevelopment()) {
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json" , "My API V1");
    });
}

app.UseResponseCompression();
app.UseCors(opt => {
    opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
        .WithExposedHeaders("Content-Disposition" , "grpc-status" , "grpc-message");
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseGrpcWeb();

app.UseAuthentication();
app.UseAuthorization();

//============================================================ grpc handlers

app.MapGrpcService<GrpcAccountHandler>().EnableGrpcWeb();


//============= ChatItems
app.MapGrpcService<ChatItems.CommandsHandler>().EnableGrpcWeb();
app.MapGrpcService<ChatItems.QueriesHandler>().EnableGrpcWeb();

//==============ChatMessages
app.MapGrpcService<ChatMessages.CommandsHandler>().EnableGrpcWeb();
app.MapGrpcService<ChatMessages.QueriesHandler>().EnableGrpcWeb();



//==============must be remove for simplicity
app.MapGrpcService<SharedModelsHandler>().EnableGrpcWeb();

//========users
app.MapGrpcService<Users.QueriesHandler>().EnableGrpcWeb();
app.MapGrpcService<Users.OnlineUserCommandsHandler>().EnableGrpcWeb();
app.MapGrpcService<Users.UserCommandsHandler>().EnableGrpcWeb();



// signalR hubs
app.MapHub<ChatMessageHub>("/chatMessageHub");
app.MapHub<OnlineStatusHub>("/OnlineStatusHub");


app.MapControllers();

app.Run();
