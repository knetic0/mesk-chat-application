using System.Security.Claims;
using MESK.MediatR;
using MESK.MiniEndpoint;
using MESK.ResponseEntity;
using MeskChatApplication.Application.Features.Commands.Authentication.ForgotPassword;
using MeskChatApplication.Application.Features.Commands.Authentication.Login;
using MeskChatApplication.Application.Features.Commands.Authentication.Logout;
using MeskChatApplication.Application.Features.Commands.Authentication.RefreshToken;
using MeskChatApplication.Application.Features.Commands.Authentication.Register;
using MeskChatApplication.Application.Features.Commands.Authentication.ResetPassword;
using MeskChatApplication.Application.Features.Queries.Authentication.Me;
using MeskChatApplication.Domain.Dtos;
using MeskChatApplication.Presentation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MeskChatApplication.Presentation.Endpoints;

public sealed class AuthenticationEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/authentication");

        group.MapPost("/login", async ([FromBody] LoginCommand request, [FromServices] ISender sender, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(request, cancellationToken);
            return Results.Ok(response);
        })
        .Produces<ResponseEntity<LoginCommandResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status400BadRequest);

        group.MapPost("/register", async ([FromBody] RegisterCommand request, [FromServices] ISender sender, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(request, cancellationToken);
            return Results.Ok(response);
        })
        .Produces<ResponseEntity<EmptyResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status409Conflict);

        group.MapGet("/me", async (ClaimsPrincipal user, ISender sender, CancellationToken cancellationToken) =>
        {
            var userId = user.GetNameIdentifier();
            var response = await sender.Send(new GetCurrentUserQuery(userId), cancellationToken);
            return Results.Ok(response);
        })
        .RequireAuthorization()
        .Produces<ResponseEntity<ApplicationUser>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound);
        
        group.MapPost("/refresh-token", async ([FromBody] RefreshTokenCommand request, [FromServices] ISender sender, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(request, cancellationToken);
            return Results.Ok(response);
        })
        .Produces<ResponseEntity<RefreshTokenCommandResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        group.MapPost("/logout", async ([FromBody] LogoutCommand request, [FromServices] ISender sender, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(request, cancellationToken);
            return Results.Ok(response);
        })
        .RequireAuthorization()
        .Produces<ResponseEntity<EmptyResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        group.MapPost("/forgot-password", async ([FromBody] ForgotPasswordCommand request,
            [FromServices] ISender sender, CancellationToken cancellationToken) =>
        {
            var notification = await sender.Send(request, cancellationToken);
            await sender.Publish(notification, cancellationToken);
            var response = ResponseEntity<EmptyResponse>.Succeeded();
            return Results.Ok(response);
        })
        .Produces<ResponseEntity<EmptyResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
        
        group.MapPost("/reset-password", async ([FromBody] ResetPasswordCommand request, [FromServices] ISender sender, CancellationToken cancellationToken) =>
        {
            await sender.Send(request, cancellationToken);
            var response = ResponseEntity<EmptyResponse>.Succeeded();
            return Results.Ok(response);
        })
        .Produces<ResponseEntity<EmptyResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);
    }
}