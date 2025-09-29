using System.Security.Claims;
using MESK.MediatR;
using MESK.MiniEndpoint;
using MESK.ResponseEntity;
using MeskChatApplication.Application.Features.Queries.Users.GetAll;
using MeskChatApplication.Domain.Dtos;
using MeskChatApplication.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MeskChatApplication.Presentation.Endpoints;

public sealed class UserEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/users");

        group.MapGet("/", async (ClaimsPrincipal user, [FromServices] ISender sender, CancellationToken cancellationToken) =>
        {
            var userId = user.GetNameIdentifier();
            return await sender.Send(new GetAllUsersQuery(userId), cancellationToken);
        })
        .RequireAuthorization()
        .Produces<ResponseEntity<List<ApplicationUser>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);
    }
}