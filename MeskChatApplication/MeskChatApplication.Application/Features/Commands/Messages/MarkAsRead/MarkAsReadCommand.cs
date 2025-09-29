using MESK.MediatR;
using MeskChatApplication.Domain.Entities;

namespace MeskChatApplication.Application.Features.Commands.Messages.MarkAsRead;

public record MarkAsReadCommand(List<Guid> Messages, Guid SenderId) : IRequest<List<Message>>;