using MESK.MediatR;
using MESK.ResponseEntity;
using MeskChatApplication.Application.Services;
using MeskChatApplication.Domain.Dtos;

namespace MeskChatApplication.Application.Features.Commands.Authentication.Logout;

public sealed class LogoutCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<LogoutCommand, ResponseEntity<EmptyResponse>>
{
    private readonly IAuthenticationService _authenticationService = authenticationService;
    
    public async Task<ResponseEntity<EmptyResponse>> Handle(LogoutCommand request, CancellationToken cancellationToken = new CancellationToken())
    {
        await _authenticationService.LogoutAsync(request,  cancellationToken);
        return ResponseEntity<EmptyResponse>.Succeeded();
    }
}