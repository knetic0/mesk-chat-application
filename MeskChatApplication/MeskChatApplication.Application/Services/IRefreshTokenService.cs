using System.Linq.Expressions;
using MeskChatApplication.Domain.Entities;

namespace MeskChatApplication.Application.Services;

public interface IRefreshTokenService
{
    Task<RefreshToken> GetAsync(Expression<Func<RefreshToken, bool>> expression, CancellationToken cancellationToken = default);
    Task<string> CreateAsync(User user, CancellationToken cancellationToken = default);
    void MarkAsUsed(RefreshToken refreshToken, CancellationToken cancellationToken = default);
    void MarkAsRevoked(RefreshToken refreshToken, CancellationToken cancellationToken = default);
}