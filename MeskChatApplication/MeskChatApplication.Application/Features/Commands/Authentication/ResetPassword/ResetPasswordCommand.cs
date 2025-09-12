using MESK.MediatR;
using MESK.ResponseEntity;
using MeskChatApplication.Domain.Dtos;

namespace MeskChatApplication.Application.Features.Commands.Authentication.ResetPassword;

public record ResetPasswordCommand(string Token, string NewPassword) : IRequest<ResponseEntity<EmptyResponse>>;