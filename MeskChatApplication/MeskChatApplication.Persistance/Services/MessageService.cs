using System.Linq.Expressions;
using MeskChatApplication.Application.Exceptions;
using MeskChatApplication.Application.Services;
using MeskChatApplication.Domain.Entities;
using MeskChatApplication.Domain.Repositories;

namespace MeskChatApplication.Persistance.Services;

public sealed class MessageService(IMessageRepository messageRepository) : IMessageService
{
    private readonly IMessageRepository _messageRepository = messageRepository;

    public async Task<Message> GetAsync(Expression<Func<Message, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        var message = await _messageRepository.GetAsync(predicate, cancellationToken);
        if (message is null) throw new NotFoundException(nameof(Message), "");
        return message;
    }

    public async Task<List<Message>> GetAllAsync(Expression<Func<Message, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _messageRepository.GetAllAsync(predicate, cancellationToken);
    }
    
    public async Task CreateAsync(Message message, CancellationToken cancellationToken)
    {
        await _messageRepository.CreateAsync(message, cancellationToken);
    }

    public void MarkAsRead(Message message, CancellationToken cancellationToken)
    {
        message.MarkAsRead();
        _messageRepository.Update(message);
    }
}