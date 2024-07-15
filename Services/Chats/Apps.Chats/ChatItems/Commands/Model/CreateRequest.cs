using MediatR;
using Shared.Server.Models.Results;

namespace Apps.Chats.ChatItems.Commands.Model;
public sealed record CreateRequest(Guid RequesterId , Guid ReceiverId) : IRequest<ResultStatus> {
    
}
