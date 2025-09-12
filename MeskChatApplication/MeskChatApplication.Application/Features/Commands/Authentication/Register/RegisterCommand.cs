using MESK.MediatR;
using MESK.ResponseEntity;
using MeskChatApplication.Domain.Dtos;

namespace MeskChatApplication.Application.Features.Commands.Authentication.Register;

public record RegisterCommand(string FirstName, string LastName, string Username, string Email, string Password) : IRequest<ResponseEntity<EmptyResponse>>;