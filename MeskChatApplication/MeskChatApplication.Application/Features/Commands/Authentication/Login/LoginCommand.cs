using MESK.MediatR;
using MESK.ResponseEntity;

namespace MeskChatApplication.Application.Features.Commands.Authentication.Login;

public record LoginCommand(string Email, string Password) : IRequest<ResponseEntity<LoginCommandResponse>>;