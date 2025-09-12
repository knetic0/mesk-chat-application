using MESK.MediatR;
using MESK.ResponseEntity;
using MeskChatApplication.Domain.Dtos;

namespace MeskChatApplication.Application.Features.Queries.Authentication.Me;

public record GetCurrentUserQuery(Guid UserId) : IRequest<ResponseEntity<ApplicationUser>>;