using System.Security.Claims;
using MESK.MediatR;
using MESK.MiniEndpoint;
using MESK.ResponseEntity;
using MeskChatApplication.Application.Features.Queries.Messages.GetAll;
using MeskChatApplication.Domain.Entities;
using MeskChatApplication.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MeskChatApplication.Presentation.Endpoints;

public sealed class MessageEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/messages");

        group.MapGet("/",
                async ([FromQuery] Guid receiverId, ClaimsPrincipal user, [FromServices] ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var senderId = user.GetNameIdentifier();
                    var messages = await sender.Send(new GetAllMessagesQuery(senderId, receiverId), cancellationToken);
                    return messages;
                })
            .RequireAuthorization()
            .Produces<ResponseEntity<List<Message>>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
    }
}