using System.Linq.Expressions;
using MeskChatApplication.Domain.Dtos;
using MeskChatApplication.Domain.Entities;
using MeskChatApplication.Domain.Enums;

namespace MeskChatApplication.Application.Services;

public interface IUserService
{
    Task<User?> GetAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken = default);
    Task<List<ApplicationUser>> GetAllAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken = default);
    Task CreateAsync(User user, CancellationToken cancellationToken = default);
    void Update(User user);
    void UpdatePassword(User user, byte[] passwordHash, byte[] passwordSalt);
    void UpdateUserStatus(User user, Status status);
}