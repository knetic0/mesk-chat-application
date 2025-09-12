using MailKit.Net.Smtp;
using MailKit.Security;
using MeskChatApplication.Application.Services;
using MeskChatApplication.Infrastructure.Options;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MeskChatApplication.Infrastructure.Services;

public sealed class EmailService(IOptions<EmailOptions> options) : IEmailService
{
    private readonly EmailOptions _options = options.Value;

    public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_options.From));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_options.SmtpServer, _options.SmtpPort, SecureSocketOptions.StartTls, cancellationToken);
        await smtp.AuthenticateAsync(_options.SmtpUsername, _options.SmtpPassword, cancellationToken);
        await smtp.SendAsync(email, cancellationToken);
        await smtp.DisconnectAsync(true, cancellationToken);
    }
}