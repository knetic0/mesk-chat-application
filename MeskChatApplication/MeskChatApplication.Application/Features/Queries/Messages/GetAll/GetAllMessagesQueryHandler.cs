using MESK.MediatR;
using MESK.ResponseEntity;
using MeskChatApplication.Application.Services;
using MeskChatApplication.Domain.Entities;

namespace MeskChatApplication.Application.Features.Queries.Messages.GetAll;

public sealed class GetAllMessagesQueryHandler(IMessageService messageService) : IRequestHandler<GetAllMessagesQuery, ResponseEntity<List<Message>>>
{
    private readonly IMessageService _messageService = messageService;

    public async Task<ResponseEntity<List<Message>>> Handle(GetAllMessagesQuery request,
        CancellationToken cancellationToken)
    {
        var messages = await _messageService.GetAllAsync(p 
            => p.SenderId == request.SenderUserId && p.ReceiverId == request.ReceiverId ||
            p.SenderId == request.ReceiverId && p.ReceiverId == request.SenderUserId, cancellationToken);
        var responseEntity = ResponseEntity<List<Message>>.Succeeded();
        return responseEntity.WithData(messages);
    }
}