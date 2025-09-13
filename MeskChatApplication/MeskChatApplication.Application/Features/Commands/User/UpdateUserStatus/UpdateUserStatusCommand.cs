using MESK.MediatR;
using MESK.ResponseEntity;
using MeskChatApplication.Domain.Dtos;
using MeskChatApplication.Domain.Enums;

namespace MeskChatApplication.Application.Features.Commands.User.UpdateUserStatus;

public record UpdateUserStatusCommand(Guid UserId, Status Status) : IRequest<ResponseEntity<ApplicationUser>>;