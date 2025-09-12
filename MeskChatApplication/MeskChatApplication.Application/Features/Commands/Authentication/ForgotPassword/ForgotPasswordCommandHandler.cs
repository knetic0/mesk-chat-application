using MESK.MediatR;
using MeskChatApplication.Application.Notifications.Email;
using MeskChatApplication.Application.Services;

namespace MeskChatApplication.Application.Features.Commands.Authentication.ForgotPassword;

public sealed class ForgotPasswordCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<ForgotPasswordCommand, ForgotPasswordNotification>
{
    private readonly IAuthenticationService _authenticationService = authenticationService;
    
    public async Task<ForgotPasswordNotification> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        return await _authenticationService.ForgotPasswordAsync(request, cancellationToken);
    }
}