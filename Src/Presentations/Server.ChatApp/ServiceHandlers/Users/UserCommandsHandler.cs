using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Server.ChatApp.Extensions;
using Server.ChatApp.Protos;
using Server.ChatApp.Protos.Users;
using Server.ChatApp.Services.Abstractions;
using Command = Apps.Auth.Users.Commands;

namespace Server.ChatApp.ServiceHandlers.Users;

[Authorize]
public class UserCommandsHandler(IMediator _mediator , IFileUploadService _logoUploader) : UserCommandRPCs.UserCommandRPCsBase {
    public override async Task<ResultMsg> UpdateImageUrl(Protos.File request , ServerCallContext context) {
        var userId = await SharedMethods.GetMyIdAsync(context,_mediator);
        using var memoryStream = new MemoryStream(request.Data.ToByteArray());
        var formFile = new FormFile(memoryStream,0,memoryStream.Length , "",request.Name);
        var uploadResult = await _logoUploader.UploadAsync(formFile,userId.ToString());
        if(!uploadResult.IsSuccessful) {
            return uploadResult.AsCommonResult();
        }
        var saveResult =await _mediator.Send(Command.UpdateProfileLogo.New(userId , uploadResult.Model!));
        return saveResult.AsCommonResult();
    }
}
