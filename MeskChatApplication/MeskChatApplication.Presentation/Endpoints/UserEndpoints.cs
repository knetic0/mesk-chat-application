using MESK.MediatR;
using MESK.MiniEndpoint;
using MESK.ResponseEntity;
using MeskChatApplication.Application.Features.Queries.Users.GetAll;
using MeskChatApplication.Domain.Dtos;
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

        group.MapGet("/", async ([FromServices] ISender sender, CancellationToken cancellationToken) 
            => await sender.Send(new GetAllUsersQuery(), cancellationToken))
        .RequireAuthorization()
        .Produces<ResponseEntity<List<ApplicationUser>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);
    }
}