using System.Linq.Expressions;
using MeskChatApplication.Domain.Entities;

namespace MeskChatApplication.Application.Services;

public interface IMessageService
{
    Task<Message> GetAsync(Expression<Func<Message, bool>> predicate, CancellationToken cancellationToken = default);
    Task<List<Message>> GetAllAsync(Expression<Func<Message, bool>> predicate, CancellationToken cancellationToken = default);
    Task CreateAsync(Message message, CancellationToken cancellationToken);
    void MarkAsRead(Message message);
}