using MESK.MediatR;
using MESK.ResponseEntity;

namespace MeskChatApplication.Application.Features.Commands.Authentication.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : IRequest<ResponseEntity<RefreshTokenCommandResponse>>;