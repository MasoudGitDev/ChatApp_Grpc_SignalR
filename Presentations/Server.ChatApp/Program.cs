using Infra.EFCore;
using Microsoft.OpenApi.Models;
using Shared.Server.Extensions;
using Shared.Server.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddScoped((_) => new DBConnectionModel(
//    builder.Configuration["GRPCChatDB"].ThrowIfNullOrWhiteSpace("The <connection-string> can not be NullOrWhiteSpace.")));

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

});

builder.Services.AddEFCoreService();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();




var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
