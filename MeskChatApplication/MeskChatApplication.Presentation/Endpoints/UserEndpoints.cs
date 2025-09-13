using System.Security.Claims;
using MESK.MediatR;
using MESK.MiniEndpoint;
using MESK.ResponseEntity;
using MeskChatApplication.Application.Features.Queries.Users.GetAll;
using MeskChatApplication.Domain.Dtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using UnauthorizedAccessException = MeskChatApplication.Application.Exceptions.UnauthorizedAccessException;

namespace MeskChatApplication.Presentation.Endpoints;

public sealed class UserEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/users");

        group.MapGet("/", async (ClaimsPrincipal user, [FromServices] ISender sender, CancellationToken cancellationToken) =>
        {
            var currentUserId = user.FindFirst(claims => claims.Type == ClaimTypes.NameIdentifier)?.Value;
            if(!Guid.TryParse(currentUserId, out var userId)) throw new UnauthorizedAccessException();
            return await sender.Send(new GetAllUsersQuery(userId), cancellationToken);
        })
        .RequireAuthorization()
        .Produces<ResponseEntity<List<ApplicationUser>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);
    }
}