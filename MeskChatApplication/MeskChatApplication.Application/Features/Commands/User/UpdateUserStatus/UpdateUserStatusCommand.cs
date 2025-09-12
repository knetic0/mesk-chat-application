using MESK.MediatR;
using MeskChatApplication.Domain.Enums;

namespace MeskChatApplication.Application.Features.Commands.User.UpdateUserStatus;

public record UpdateUserStatusCommand(Guid UserId, Status Status) : IRequest;