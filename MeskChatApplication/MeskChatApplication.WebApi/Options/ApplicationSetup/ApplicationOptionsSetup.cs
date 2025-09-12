using MeskChatApplication.Persistance.Options;
using Microsoft.Extensions.Options;

namespace MeskChatApplication.WebApi.Options.ApplicationSetup;

public sealed class ApplicationOptionsSetup(IConfiguration configuration) : IConfigureOptions<ApplicationOptions>
{
    private readonly IConfiguration _configuration = configuration;

    public void Configure(ApplicationOptions options)
    {
        _configuration.GetSection(nameof(ApplicationOptions)).Bind(options);
    }
}