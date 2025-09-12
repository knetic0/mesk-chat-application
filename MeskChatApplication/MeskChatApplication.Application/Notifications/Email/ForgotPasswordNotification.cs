using MESK.MediatR;

namespace MeskChatApplication.Application.Notifications.Email;

public record ForgotPasswordNotification(string To, string ResetLink) : INotification;