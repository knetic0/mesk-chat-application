using System.Security.Claims;
using System.Text;
using MeskChatApplication.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MeskChatApplication.WebApi.Options.JwtSetup;

public sealed class JwtBearerOptionsSetup(IOptions<JwtOptions> jwtOptions) : IPostConfigureOptions<JwtBearerOptions>
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public void PostConfigure(string? name, JwtBearerOptions options)
    {
        options.TokenValidationParameters.ValidateIssuer = true;
        options.TokenValidationParameters.ValidateAudience = true;
        options.TokenValidationParameters.ValidateLifetime = true;
        options.TokenValidationParameters.ValidateIssuerSigningKey = true;
        options.TokenValidationParameters.ValidIssuer = _jwtOptions.Issuer;
        options.TokenValidationParameters.NameClaimType = ClaimTypes.NameIdentifier;
        options.TokenValidationParameters.ValidAudience = _jwtOptions.Audience;
        options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
    }
}