using MESK.MediatR;
using MESK.ResponseEntity;
using MeskChatApplication.Application.Services;
using MeskChatApplication.Domain.Dtos;

namespace MeskChatApplication.Application.Features.Queries.Users.GetAll;

public sealed class GetAllUsersQueryHandler(IUserService userService) : IRequestHandler<GetAllUsersQuery, ResponseEntity<List<ApplicationUser>>>
{
    private readonly IUserService _userService = userService;

    public async Task<ResponseEntity<List<ApplicationUser>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userService.GetAllAsync(u => u.Id != request.CurrentUserId, cancellationToken);
        var responseEntity = ResponseEntity<List<ApplicationUser>>.Succeeded();
        return responseEntity.WithData(users);
    }
}