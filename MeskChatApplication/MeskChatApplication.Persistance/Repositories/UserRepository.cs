using System.Linq.Expressions;
using Mapster;
using MeskChatApplication.Domain.Entities;
using MeskChatApplication.Domain.Repositories;
using MeskChatApplication.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace MeskChatApplication.Persistance.Repositories;

public sealed class UserRepository(ApplicationDatabaseContext context) : IUserRepository
{
    private readonly ApplicationDatabaseContext _context = context;

    public async Task<User?> GetAsync(
        Expression<Func<User, bool>> predicate, 
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<User>()
            .AsNoTracking()
            .Where(predicate)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<User>()
            .AsNoTracking()
            .OrderBy(u => u.FirstName + " " + u.LastName)
            .ToListAsync(cancellationToken);
    }

    public async Task CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Set<User>().AddAsync(user, cancellationToken);
    }

    public void Update(User user)
    {
        _context.Set<User>().Update(user);
    }
}