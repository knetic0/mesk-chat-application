using MeskChatApplication.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace MeskChatApplication.WebApi.Options.EmailSetup;

public sealed class EmailOptionsSetup(IConfiguration configuration) : IConfigureOptions<EmailOptions>
{
    private readonly IConfiguration _configuration = configuration;

    public void Configure(EmailOptions options)
    {
        _configuration.GetSection(nameof(EmailOptions)).Bind(options);
    }
}