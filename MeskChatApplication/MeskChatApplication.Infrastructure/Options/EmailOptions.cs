namespace MeskChatApplication.Infrastructure.Options;

public sealed class EmailOptions
{
    public string SmtpServer { get; init; } = string.Empty;
    public int SmtpPort { get; init; }
    public string SmtpUsername { get; init; } = string.Empty;
    public string SmtpPassword { get; init; } = string.Empty;
    public string From { get; init; } = string.Empty;
}