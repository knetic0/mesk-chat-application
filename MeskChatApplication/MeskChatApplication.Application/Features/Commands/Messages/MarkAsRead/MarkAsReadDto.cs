using MESK.MediatR;

namespace MeskChatApplication.Application.Features.Commands.Messages.MarkAsRead;

public record MarkAsReadDto(Guid MessageId) : IRequest;