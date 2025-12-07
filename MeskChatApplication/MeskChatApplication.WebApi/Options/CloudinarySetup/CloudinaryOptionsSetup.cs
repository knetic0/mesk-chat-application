using MeskChatApplication.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace MeskChatApplication.WebApi.Options.CloudinarySetup;

public sealed class CloudinaryOptionsSetup(IConfiguration configuration) : IConfigureOptions<CloudinaryOptions>
{
    private readonly IConfiguration _configuration = configuration;

    public void Configure(CloudinaryOptions options)
    {
        _configuration.GetSection(nameof(CloudinaryOptions)).Bind(options);
    }
}