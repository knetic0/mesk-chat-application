using MESK.MediatR;
using MESK.ResponseEntity;
using MeskChatApplication.Domain.Dtos;

namespace MeskChatApplication.Application.Features.Queries.Users.GetAll;

public record GetAllUsersQuery(Guid CurrentUserId) : IRequest<ResponseEntity<List<ApplicationUser>>>;