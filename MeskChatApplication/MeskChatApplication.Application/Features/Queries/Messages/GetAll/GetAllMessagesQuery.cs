using MESK.MediatR;
using MESK.ResponseEntity;
using MeskChatApplication.Domain.Entities;

namespace MeskChatApplication.Application.Features.Queries.Messages.GetAll;

public record GetAllMessagesQuery(Guid SenderUserId, Guid ReceiverId) : IRequest<ResponseEntity<List<Message>>>;