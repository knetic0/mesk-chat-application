using MeskChatApplication.Application.Features.Commands.Authentication.ForgotPassword;
using MeskChatApplication.Application.Features.Commands.Authentication.Login;
using MeskChatApplication.Application.Features.Commands.Authentication.Logout;
using MeskChatApplication.Application.Features.Commands.Authentication.RefreshToken;
using MeskChatApplication.Application.Features.Commands.Authentication.Register;
using MeskChatApplication.Application.Features.Commands.Authentication.ResetPassword;
using MeskChatApplication.Application.Features.Queries.Authentication.Me;
using MeskChatApplication.Application.Notifications.Email;
using MeskChatApplication.Domain.Dtos;

namespace MeskChatApplication.Application.Services;

public interface IAuthenticationService
{
    Task<LoginCommandResponse> LoginAsync(LoginCommand command, CancellationToken cancellationToken = default);
    Task RegisterAsync(RegisterCommand command, CancellationToken cancellationToken = default);
    Task<ApplicationUser> GetCurrentUserAsync(GetCurrentUserQuery query, CancellationToken cancellationToken = default);
    Task<RefreshTokenCommandResponse>  RefreshTokenAsync(RefreshTokenCommand command, CancellationToken cancellationToken = default);
    Task LogoutAsync(LogoutCommand command, CancellationToken cancellationToken = default);
    Task<ForgotPasswordNotification> ForgotPasswordAsync(ForgotPasswordCommand command, CancellationToken cancellationToken = default);
    Task ResetPasswordAsync(ResetPasswordCommand command, CancellationToken cancellationToken = default);
}