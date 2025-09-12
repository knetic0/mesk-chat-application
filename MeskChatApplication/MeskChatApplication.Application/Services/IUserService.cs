using System.Linq.Expressions;
using MeskChatApplication.Domain.Dtos;
using MeskChatApplication.Domain.Entities;
using MeskChatApplication.Domain.Enums;

namespace MeskChatApplication.Application.Services;

public interface IUserService
{
    Task<User?> GetAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken = default);
    Task<List<ApplicationUser>> GetAllAsync(CancellationToken cancellationToken = default);
    Task CreateAsync(User user, CancellationToken cancellationToken = default);
    void Update(User user);
    void UpdatePasswordAsync(User user, byte[] passwordHash, byte[] passwordSalt);
    void UpdateUserStatusAsync(User user, Status status);
}