using MESK.MediatR;
using MeskChatApplication.Application.Attributes;
using MeskChatApplication.Application.Services;
using MeskChatApplication.Domain.Entities;

namespace MeskChatApplication.Application.Features.Commands.Messages.MarkAsRead;

[Transactional]
public sealed class MarkAsReadCommandHandler(IMessageService messageService) : IRequestHandler<MarkAsReadCommand, Message>
{
    private readonly IMessageService _messageService = messageService;

    public async Task<Message> Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
    {
        var message = await _messageService.GetAsync(m => m.Id == request.MessageId, cancellationToken);
        if(message.ReceiverId != request.SenderId) throw new UnauthorizedAccessException();
        _messageService.MarkAsRead(message, cancellationToken);
        return message;
    }
}