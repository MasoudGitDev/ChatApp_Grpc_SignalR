using Apps.Auth;
using Apps.Auth.Constants;
using Apps.Auth.Services;
using Domains.Auth.Queries;
using Domains.Auth.Role.Aggregate;
using Domains.Auth.User.Aggregate;
using Domains.Chats.Contacts.Queries;
using Domains.Chats.Item.Queries;
using Domains.Chats.Message.Queries;
using Domains.Chats.Requests.Queries;
using Infra.EFCore.Contexts;
using Infra.EFCore.Implementations;
using Infra.EFCore.Implementations.Accounts;
using Infra.EFCore.Implementations.Chats;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shared.Server.Extensions;
using Shared.Server.Models;
using System.Text;
using UnitOfWorks.Abstractions;

namespace Infra.EFCore;
public static class ServiceRegistrationExtensions {
    public static IServiceCollection AddEFCoreService(this IServiceCollection services) {
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        // ======================== Apps.Auth services
        var jwtSettings = (configuration.GetSection("JwtSettingsModel").Get<JwtSettingsModel>())
            .ThrowIfNull("JwtSettingsModel can not be null.");
        services.AddScoped(x => new JwtSettingsModel() {
            Audience = jwtSettings.Audience ,
            Issuer = jwtSettings.Issuer ,
            ExpireMinuteNumber = jwtSettings.ExpireMinuteNumber ,
            SecureKey = jwtSettings.SecureKey ,
        });
        services.AddAuthServices();
        services.AddScoped<IAccountService , AccountService>();
        services.AddScoped<IUserQueries , UserQueries>();

        //========================


        services.AddDbContext<AppDbContext>(options => {
            options.UseSqlServer(ConnectionStrings.GRPCChatDb);
            options.EnableSensitiveDataLogging();
        });

        services.AddIdentityCore<AppUser>(options => {
            options.User.RequireUniqueEmail = true;
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredUniqueChars = 1;
            options.SignIn.RequireConfirmedEmail = true;
        })
            .AddUserManager<UserManager<AppUser>>()
            .AddSignInManager<SignInManager<AppUser>>()
            .AddRoles<AppRole>()
            .AddEntityFrameworkStores<AppDbContext>();


        var jwtSettingModel = configuration.GetSection("JwtSettingsModel").Get<JwtSettingsModel>()
            .ThrowIfNull("The <JwtSettingsModel> can not be null.");

        services.AddAuthentication(options => {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme , options => {
            options.TokenValidationParameters = new TokenValidationParameters {
                ValidateIssuer = true ,
                ValidateAudience = true ,
                ValidateLifetime = true ,
                ValidateIssuerSigningKey = true ,
                ValidIssuer = jwtSettingModel.Issuer ,
                ValidAudience = jwtSettingModel.Audience ,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettingModel.SecureKey))
            };
            options.Events = new JwtBearerEvents() {
                OnTokenValidated = async (ctx) => {
                    var userId = (ctx.Principal?.Claims.Where(x=>x.Type == TokenKeys.UserId).FirstOrDefault()?.Value)
                        .ThrowIfNull("The <user-id> can not be NullOrWhiteSpace.");
                    var signInManager = ctx.HttpContext.RequestServices.GetRequiredService<SignInManager<AppUser>>();
                    var user = (await signInManager.UserManager.FindByIdAsync(userId))
                        .ThrowIfNull($"The User with id :<{userId}> not exist.");
                    ctx.Principal = await signInManager.CreateUserPrincipalAsync(user);
                }
            };
        });

        //================ Chat Services
        services.AddScoped<IChatItemQueries , ChatItemQueries>();
        services.AddScoped<IChatMessageQueries , MessageQueries>();
        services.AddScoped<IChatRequestQueries , ChatRequestQueries>();
        services.AddScoped<IContactQueries , ContactQueries>();
        services.AddScoped<IAppQueries , AppQueries>();
        services.AddScoped<IChatUOW , ChatUOW>();
        return services;
    }
}
