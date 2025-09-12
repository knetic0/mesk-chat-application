using MESK.MediatR;
using MESK.ResponseEntity;
using MeskChatApplication.Domain.Dtos;

namespace MeskChatApplication.Application.Features.Commands.Authentication.Logout;

public record LogoutCommand(string RefreshToken) : IRequest<ResponseEntity<EmptyResponse>>;