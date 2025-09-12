using MESK.MediatR;
using MESK.ResponseEntity;
using MeskChatApplication.Application.Services;
using MeskChatApplication.Domain.Dtos;

namespace MeskChatApplication.Application.Features.Commands.Authentication.Register;

public sealed class RegisterCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<RegisterCommand, ResponseEntity<EmptyResponse>>
{
    private readonly IAuthenticationService _authenticationService = authenticationService;
    
    public async Task<ResponseEntity<EmptyResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        await _authenticationService.RegisterAsync(request, cancellationToken);
        var responseEntity = ResponseEntity<EmptyResponse>.Succeeded();
        return responseEntity;
    }
}