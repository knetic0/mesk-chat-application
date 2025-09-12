using System.Linq.Expressions;
using MeskChatApplication.Domain.Entities;

namespace MeskChatApplication.Domain.Repositories;

public interface IPasswordResetTokenRepository
{
    Task CreateAsync(PasswordResetToken token, CancellationToken cancellationToken);
    void Update(PasswordResetToken token);

    Task<PasswordResetToken?> GetAsync(Expression<Func<PasswordResetToken, bool>> predicate,
        CancellationToken cancellationToken);
}