using System.Linq.Expressions;
using MeskChatApplication.Domain.Entities;

namespace MeskChatApplication.Domain.Repositories;

public interface IMessageRepository
{
    Task<Message?> GetAsync(Expression<Func<Message, bool>> predicate, CancellationToken cancellationToken = default);
    Task<List<Message>> GetAllAsync(Expression<Func<Message, bool>> predicate, CancellationToken cancellationToken = default);
    Task CreateAsync(Message message, CancellationToken cancellationToken);
    void Update(Message message);
}