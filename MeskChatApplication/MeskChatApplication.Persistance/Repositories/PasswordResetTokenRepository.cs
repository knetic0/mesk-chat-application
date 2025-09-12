using System.Linq.Expressions;
using MeskChatApplication.Domain.Entities;
using MeskChatApplication.Domain.Repositories;
using MeskChatApplication.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace MeskChatApplication.Persistance.Repositories;

public sealed class PasswordResetTokenRepository(ApplicationDatabaseContext context) : IPasswordResetTokenRepository
{
    private readonly ApplicationDatabaseContext _context = context;

    public async Task<PasswordResetToken?> GetAsync(Expression<Func<PasswordResetToken, bool>> predicate,
        CancellationToken cancellationToken)
    {
        return await _context.Set<PasswordResetToken>()
            .Include(p => p.User)
            .Where(predicate)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task CreateAsync(PasswordResetToken passwordResetToken,
        CancellationToken cancellationToken)
    {
        await _context.Set<PasswordResetToken>().AddAsync(passwordResetToken, cancellationToken);
    }

    public void Update(PasswordResetToken passwordResetToken)
    {
        _context.Set<PasswordResetToken>().Update(passwordResetToken);
    }
}