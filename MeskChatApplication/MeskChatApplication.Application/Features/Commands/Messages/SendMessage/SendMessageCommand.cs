using MESK.MediatR;
using MeskChatApplication.Domain.Entities;

namespace MeskChatApplication.Application.Features.Commands.Messages.SendMessage;

public record SendMessageCommand(Guid SenderId, Guid ReceiverId, string Message) : IRequest<Message>;