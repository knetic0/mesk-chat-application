using MeskChatApplication.Infrastructure.Authentication;
using Microsoft.Extensions.Options;

namespace MeskChatApplication.WebApi.Options.JwtSetup;

public sealed class JwtOptionsSetup(IConfiguration configuration) : IConfigureOptions<JwtOptions>
{
    private readonly IConfiguration _configuration = configuration;

    public void Configure(JwtOptions options)
    {
        _configuration.GetSection(nameof(JwtOptions)).Bind(options);
    }
}