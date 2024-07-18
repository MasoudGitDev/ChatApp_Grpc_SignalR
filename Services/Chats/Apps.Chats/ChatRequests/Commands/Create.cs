using Domains.Chats.Requests.Aggregate;
using MediatR;
using Shared.Server.Models.Results;
using UnitOfWorks.Abstractions;
using UnitOfWorks.Extensions;

namespace Apps.Chats.ChatRequests.Commands;

// ChatRequest Create(Request) Model
public sealed record Create(Guid MyId , Guid PersonId) : IRequest<ResultStatus> {
    public static Create New(Guid MyId , Guid PersonId) => new(MyId , PersonId);
}

// ChatRequest Create(Request) Handler
internal sealed class CreateHandler(IChatUOW _unitOfWork)
    : ChatRequestHandler<Create , ResultStatus>(_unitOfWork.HasValue()) {
    public override async Task<ResultStatus> Handle(Create request , CancellationToken cancellationToken) {
        // Destructure request for better readability
        (Guid myId, Guid personId) = request;

        // Check if receiver is already in contacts
        var isInContact = await _unitOfWork.Queries.Contacts.IsInContactAsync(myId, personId);
        if(isInContact is not null) {
            return ErrorResults.Founded("The recipient is already in your contacts.");
        }

        // Check for existing chat request
        var existingRequest = await _unitOfWork.Queries.ChatRequests.FindSameRequestAsync(myId, personId);
        if(existingRequest is not null) {
            return ErrorResults.Founded("A chat request already exists for this recipient.");
        }

        // Create a new chat request if everything is ok
        await _unitOfWork.CreateAsync(ChatRequest.Create(myId , personId));
        await _unitOfWork.SaveChangeAsync();
        return SuccessResults.Created;
    }
}
