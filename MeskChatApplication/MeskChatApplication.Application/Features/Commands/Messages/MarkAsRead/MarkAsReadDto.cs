using MESK.MediatR;

namespace MeskChatApplication.Application.Features.Commands.Messages.MarkAsRead;

public record MarkAsReadDto(List<Guid> MessageId) : IRequest;