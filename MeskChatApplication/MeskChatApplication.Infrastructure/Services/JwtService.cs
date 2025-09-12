using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MeskChatApplication.Application.Services;
using MeskChatApplication.Domain.Entities;
using MeskChatApplication.Infrastructure.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MeskChatApplication.Infrastructure.Services;

public sealed class JwtService(IOptions<JwtOptions> jwtOptions) : IJwtService
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;
    
    public string Generate(User user)
    {
        var claims = GetClaims(user);
        var expires = GetExpirationDate();
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: signingCredentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private Claim[] GetClaims(User user)
    {
        return new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
    }

    private DateTime GetExpirationDate()
    {
        return DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiresInMinutes);
    }
}