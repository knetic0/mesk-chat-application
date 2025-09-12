using MESK.MediatR;
using MESK.ResponseEntity;
using MeskChatApplication.Application.Attributes;
using MeskChatApplication.Application.Services;

namespace MeskChatApplication.Application.Features.Commands.Authentication.RefreshToken;

[Transactional]
public sealed class RefreshTokenCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<RefreshTokenCommand, ResponseEntity<RefreshTokenCommandResponse>>
{
    private readonly IAuthenticationService _authenticationService = authenticationService;
    
    public async Task<ResponseEntity<RefreshTokenCommandResponse>> Handle(RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _authenticationService.RefreshTokenAsync(request, cancellationToken);
        var responseEntity = ResponseEntity<RefreshTokenCommandResponse>.Succeeded();
        return responseEntity.WithData(result);
    }
}