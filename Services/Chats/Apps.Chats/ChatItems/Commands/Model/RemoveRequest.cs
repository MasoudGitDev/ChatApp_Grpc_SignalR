using MediatR;
using Shared.Server.Models.Results;

namespace Apps.Chats.ChatItems.Commands.Model;
public record RemoveRequest(Guid ChatItemId) : IRequest<ResultStatus>;
