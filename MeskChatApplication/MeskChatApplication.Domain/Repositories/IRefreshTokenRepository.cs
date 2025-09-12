using System.Linq.Expressions;
using MeskChatApplication.Domain.Entities;

namespace MeskChatApplication.Domain.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetAsync(Expression<Func<RefreshToken, bool>> predicate, CancellationToken cancellationToken = default);
    Task CreateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
    void Update(RefreshToken refreshToken, CancellationToken cancellationToken = default);
}