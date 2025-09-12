using MESK.MediatR;
using MeskChatApplication.Application.Attributes;
using MeskChatApplication.Application.Services;
using MeskChatApplication.Domain.Entities;

namespace MeskChatApplication.Application.Features.Commands.Messages.SendMessage;

[Transactional]
public sealed class SendMessageCommandHandler(IMessageService messageService) : IRequestHandler<SendMessageCommand, Message>
{
    private readonly IMessageService _messageService = messageService;
    
    public async Task<Message> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var message = new Message(request.SenderId, request.ReceiverId, request.Message);
        await _messageService.CreateAsync(message, cancellationToken);
        return message;
    }
}