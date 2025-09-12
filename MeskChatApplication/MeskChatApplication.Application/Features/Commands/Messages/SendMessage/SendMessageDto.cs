using MESK.MediatR;

namespace MeskChatApplication.Application.Features.Commands.Messages.SendMessage;

public record SendMessageDto(Guid ReceiverId, string Message);