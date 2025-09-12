using MESK.MediatR;
using MeskChatApplication.Application.Services;

namespace MeskChatApplication.Application.Notifications.Email;

public sealed class ForgotPasswordEmailHandler(IEmailService emailService) : INotificationHandler<ForgotPasswordNotification>
{
    private readonly IEmailService _emailService = emailService;

    public async Task Handle(ForgotPasswordNotification notification, CancellationToken cancellationToken)
    {
        var body = ForgotPasswordTemplate.Value.Replace("{{resetLink}}",  notification.ResetLink);
        await _emailService.SendEmailAsync(notification.To, "Reset Your Password", body, cancellationToken);
    }
}