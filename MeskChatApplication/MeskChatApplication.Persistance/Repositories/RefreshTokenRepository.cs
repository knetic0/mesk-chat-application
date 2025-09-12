using System.Linq.Expressions;
using MeskChatApplication.Domain.Entities;
using MeskChatApplication.Domain.Repositories;
using MeskChatApplication.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace MeskChatApplication.Persistance.Repositories;

public sealed class RefreshTokenRepository(ApplicationDatabaseContext context) : IRefreshTokenRepository
{
    private readonly ApplicationDatabaseContext _context = context;

    public async Task<RefreshToken?> GetAsync(Expression<Func<RefreshToken, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<RefreshToken>()
            .Include(r => r.User)
            .Where(predicate)
            .FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task CreateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        await _context.Set<RefreshToken>().AddAsync(refreshToken, cancellationToken);
    }

    public void Update(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        _context.Set<RefreshToken>().Update(refreshToken);
    }
}