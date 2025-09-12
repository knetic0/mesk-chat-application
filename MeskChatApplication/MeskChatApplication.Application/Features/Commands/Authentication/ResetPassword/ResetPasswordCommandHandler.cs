using MESK.MediatR;
using MESK.ResponseEntity;
using MeskChatApplication.Application.Services;
using MeskChatApplication.Domain.Dtos;

namespace MeskChatApplication.Application.Features.Commands.Authentication.ResetPassword;

public sealed class ResetPasswordCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<ResetPasswordCommand, ResponseEntity<EmptyResponse>>
{
    private readonly IAuthenticationService _authenticationService = authenticationService;
    
    public async Task<ResponseEntity<EmptyResponse>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        await _authenticationService.ResetPasswordAsync(request,  cancellationToken);
        var responseEntity = ResponseEntity<EmptyResponse>.Succeeded();
        return responseEntity;
    }
}