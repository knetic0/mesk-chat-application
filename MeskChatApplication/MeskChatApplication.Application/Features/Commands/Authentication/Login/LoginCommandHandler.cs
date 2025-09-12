using MESK.MediatR;
using MESK.ResponseEntity;
using MeskChatApplication.Application.Services;

namespace MeskChatApplication.Application.Features.Commands.Authentication.Login;

public sealed class LoginCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<LoginCommand, ResponseEntity<LoginCommandResponse>>
{
    private readonly IAuthenticationService _authenticationService = authenticationService;
    
    public async Task<ResponseEntity<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _authenticationService.LoginAsync(request, cancellationToken);
        var responseEntity = ResponseEntity<LoginCommandResponse>.Succeeded();
        return responseEntity.WithData(result);
    }
}