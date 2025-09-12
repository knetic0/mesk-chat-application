using System.Linq.Expressions;
using MeskChatApplication.Domain.Entities;

namespace MeskChatApplication.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken = default);
    Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default);
    Task CreateAsync(User user, CancellationToken cancellationToken = default);
    void Update(User user);
}