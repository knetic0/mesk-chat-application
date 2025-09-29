using MESK.MediatR;
using MeskChatApplication.Application.Attributes;
using MeskChatApplication.Application.Services;
using MeskChatApplication.Domain.Entities;

namespace MeskChatApplication.Application.Features.Commands.Messages.MarkAsRead;

[Transactional]
public sealed class MarkAsReadCommandHandler(IMessageService messageService) : IRequestHandler<MarkAsReadCommand, List<Message>>
{
    private readonly IMessageService _messageService = messageService;

    public async Task<List<Message>> Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
    {
        var messages = await _messageService.GetAllAsync(m => request.Messages.Contains(m.Id), cancellationToken);
        foreach (var message in messages)
        {
            if(message.ReceiverId != request.SenderId) throw new UnauthorizedAccessException();
            _messageService.MarkAsRead(message);
        }
        return messages;
    }
}