using MESK.MediatR;
using MeskChatApplication.Domain.Entities;

namespace MeskChatApplication.Application.Features.Commands.Messages.MarkAsRead;

public record MarkAsReadCommand(Guid MessageId, Guid SenderId) : IRequest<Message>;