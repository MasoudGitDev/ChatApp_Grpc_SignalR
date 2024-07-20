using Apps.Auth.Users.Queries;
using Grpc.Core;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Server.ChatApp.Protos;
using Server.ChatApp.Protos.Users;
using Shared.Server.Dtos.User;
using Shared.Server.Models.Results;

namespace Server.ChatApp.ServiceHandlers.Users;

[Authorize]
public class QueriesHandler(IMediator _mediator) : UserQeriesRPCs.UserQeriesRPCsBase {


    /// <summary>
    ///  usage in home page
    /// </summary>
    public override async Task GetUsers(Empty request , IServerStreamWriter<UserBasicInfoMsg> responseStream , ServerCallContext context) {
        ResultStatus<List<UserHomeDto>> result = await _mediator.Send(GetHomeUsers.New());
        if(!result.IsSuccessful) {
            throw new RpcException(Status.DefaultCancelled);
        }
        foreach(var user in result.Model!) {
            await responseStream.WriteAsync(user.Adapt<UserBasicInfoMsg>());
        }        
    }
}
