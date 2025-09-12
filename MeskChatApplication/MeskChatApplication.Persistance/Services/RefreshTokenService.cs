using System.Linq.Expressions;
using System.Security.Cryptography;
using MeskChatApplication.Application.Services;
using MeskChatApplication.Domain.Entities;
using MeskChatApplication.Domain.Repositories;
using UnauthorizedAccessException = MeskChatApplication.Application.Exceptions.UnauthorizedAccessException;

namespace MeskChatApplication.Persistance.Services;

public sealed class RefreshTokenService(IRefreshTokenRepository refreshTokenRepository) : IRefreshTokenService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;

    public async Task<RefreshToken> GetAsync(Expression<Func<RefreshToken, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        var refreshToken = await _refreshTokenRepository.GetAsync(predicate, cancellationToken);
        if (refreshToken is null) throw new UnauthorizedAccessException();
        return refreshToken;
    }
    
    public async Task<string> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        var token = GenerateRefreshToken();
        var refreshToken = new RefreshToken(token, user.Id);
        await _refreshTokenRepository.CreateAsync(refreshToken, cancellationToken);
        return token;
    }

    public void MarkAsUsed(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        refreshToken.MarkUsed();
        _refreshTokenRepository.Update(refreshToken, cancellationToken);
    }

    public void MarkAsRevoked(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        refreshToken.MarkRevoked();
        _refreshTokenRepository.Update(refreshToken, cancellationToken);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}