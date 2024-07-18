using Domains.Chats.Contacts.Aggregate;
using MediatR;
using Shared.Server.Extensions;
using Shared.Server.Models.Results;
using UnitOfWorks.Abstractions;

namespace Apps.Chats.ChatRequests.Commands;

// ChatRequests Accept Model
public sealed record Accept(Guid ChatRequestId , Guid MyId) : IRequest<ResultStatus> {
    public static Accept New(Guid ChatRequestId , Guid MyId) => new(ChatRequestId , MyId);
}


// ChatRequests Accept Handler
internal sealed class AcceptHandler(IChatUOW _unitOfWork)
    : ChatRequestHandler<Accept , ResultStatus>(_unitOfWork.ThrowIfNull("The IChatUOW can not be null!")) {
    public override async Task<ResultStatus> Handle(Accept request , CancellationToken cancellationToken)
      => await DoAsync(request.ChatRequestId , async (model) => {
          await _unitOfWork.DeleteAsync(model);
          await _unitOfWork.CreateAsync(Contact.Create(model.RequesterId , model.ReceiverId));
      } , okMessage);

    private const string okMessage = "The request has been accepted successfully.";
}
