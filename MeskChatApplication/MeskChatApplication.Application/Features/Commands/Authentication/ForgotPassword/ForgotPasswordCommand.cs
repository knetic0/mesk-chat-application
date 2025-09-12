using MESK.MediatR;
using MeskChatApplication.Application.Notifications.Email;

namespace MeskChatApplication.Application.Features.Commands.Authentication.ForgotPassword;

public record ForgotPasswordCommand(string Email) : IRequest<ForgotPasswordNotification>;