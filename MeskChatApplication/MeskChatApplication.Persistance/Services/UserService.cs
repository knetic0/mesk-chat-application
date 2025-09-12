using System.Linq.Expressions;
using Mapster;
using MeskChatApplication.Application.Services;
using MeskChatApplication.Domain.Dtos;
using MeskChatApplication.Domain.Entities;
using MeskChatApplication.Domain.Enums;
using MeskChatApplication.Domain.Repositories;

namespace MeskChatApplication.Persistance.Services;

public sealed class UserService(IUserRepository userRepository) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    
    public async Task<User?> GetAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _userRepository.GetAsync(predicate, cancellationToken);
    }

    public async Task<List<ApplicationUser>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);
        return users.Adapt<List<ApplicationUser>>();
    }

    public async Task CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        await _userRepository.CreateAsync(user, cancellationToken);
    }

    public void Update(User user)
    {
        _userRepository.Update(user);
    }

    public void UpdatePasswordAsync(User user, byte[] passwordHash, byte[] passwordSalt)
    {
        user.ChangePassword(passwordHash, passwordSalt);
        Update(user);
    }

    public void UpdateUserStatusAsync(User user, Status status)
    {
        user.SetStatus(status);
        Update(user);
    }
}