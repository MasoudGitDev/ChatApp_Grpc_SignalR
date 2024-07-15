using Domains.Auth.User.Aggregate;
using MediatR;

namespace Apps.Chats.Shared.Models;
public record FindMeByUserNameRequest(string UserName) : IRequest<AppUser>;