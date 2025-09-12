using System.Linq.Expressions;
using System.Security.Cryptography;
using MeskChatApplication.Application.Services;
using MeskChatApplication.Domain.Entities;
using MeskChatApplication.Domain.Repositories;
using MeskChatApplication.Persistance.Options;
using Microsoft.Extensions.Options;

namespace MeskChatApplication.Persistance.Services;

public sealed class PasswordResetTokenService(IPasswordResetTokenRepository passwordResetTokenRepository, 
    IOptions<ApplicationOptions> applicationOptions) : IPasswordResetTokenService
{
    private readonly IPasswordResetTokenRepository _passwordResetTokenRepository = passwordResetTokenRepository;
    private readonly ApplicationOptions _applicationOptions = applicationOptions.Value;

    public async Task<string> CreateAsync(User user, CancellationToken cancellationToken)
    {
        var token = GenerateResetPasswordToken();
        var passwordResetToken = new PasswordResetToken(user.Id, token); 
        await _passwordResetTokenRepository.CreateAsync(passwordResetToken, cancellationToken);
        return $"{_applicationOptions.FrontendBaseUrl}/${_applicationOptions.ResetPasswordPathName}{token}";
    }

    public void Update(PasswordResetToken token)
    {
        _passwordResetTokenRepository.Update(token);
    }

    public async Task<PasswordResetToken?> GetAsync(Expression<Func<PasswordResetToken, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _passwordResetTokenRepository.GetAsync(predicate, cancellationToken);
    }
    
    public void MarkAsUsed(PasswordResetToken passwordResetToken)
    {
        passwordResetToken.MarkAsUsed();
        Update(passwordResetToken);
    }
    
    private static string GenerateResetPasswordToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}