using MESK.MediatR;
using MeskChatApplication.Application.Exceptions;
using MeskChatApplication.Application.Services;

namespace MeskChatApplication.Application.Features.Commands.User.UpdateUserStatus;

public sealed class UpdateUserStatusCommandHandler(IUserService userService) : IRequestHandler<UpdateUserStatusCommand>
{
    private readonly IUserService _userService = userService;

    public async Task Handle(UpdateUserStatusCommand request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetAsync(user => user.Id == request.UserId, cancellationToken);
        if (user is null) throw new NotFoundException(nameof(Domain.Entities.User), "");
        _userService.UpdateUserStatusAsync(user, request.Status);
    }
}