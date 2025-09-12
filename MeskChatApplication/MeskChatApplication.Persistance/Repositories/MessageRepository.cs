using System.Linq.Expressions;
using MeskChatApplication.Domain.Entities;
using MeskChatApplication.Domain.Repositories;
using MeskChatApplication.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace MeskChatApplication.Persistance.Repositories;

public sealed class MessageRepository(ApplicationDatabaseContext context) : IMessageRepository
{
    private readonly ApplicationDatabaseContext _context = context;

    public async Task<Message?> GetAsync(Expression<Func<Message, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<Message>()
            .Where(predicate)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Message>> GetAllAsync(Expression<Func<Message, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<Message>()
            .AsNoTracking()
            .Where(predicate)
            .OrderBy(x => x.SendAt)
            .ToListAsync(cancellationToken);
    }
    
    public async Task CreateAsync(Message message, CancellationToken cancellationToken)
    {
        await _context.Set<Message>().AddAsync(message, cancellationToken);
    }

    public void Update(Message message)
    {
        _context.Set<Message>().Update(message);
    }
}