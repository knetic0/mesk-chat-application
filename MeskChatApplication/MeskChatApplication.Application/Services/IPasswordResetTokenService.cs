using System.Linq.Expressions;
using MeskChatApplication.Domain.Entities;

namespace MeskChatApplication.Application.Services;

public interface IPasswordResetTokenService
{
    Task<string> CreateAsync(User user, CancellationToken cancellationToken);
    Task<PasswordResetToken?> GetAsync(Expression<Func<PasswordResetToken, bool>> expression, CancellationToken cancellationToken = default);
    void Update(PasswordResetToken passwordResetToken);
    
    void MarkAsUsed(PasswordResetToken passwordResetToken);
}