using MESK.MediatR;
using MESK.ResponseEntity;
using MeskChatApplication.Application.Attributes;
using MeskChatApplication.Application.Exceptions;
using MeskChatApplication.Application.Services;
using MeskChatApplication.Domain.Dtos;

namespace MeskChatApplication.Application.Features.Commands.User.UpdateUserStatus;

[Transactional]
public sealed class UpdateUserStatusCommandHandler(IUserService userService) : IRequestHandler<UpdateUserStatusCommand, ResponseEntity<EmptyResponse>>
{
    private readonly IUserService _userService = userService;

    public async Task<ResponseEntity<EmptyResponse>> Handle(UpdateUserStatusCommand request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetAsync(user => user.Id == request.UserId, cancellationToken);
        if (user is null) throw new NotFoundException(nameof(Domain.Entities.User), "");
        _userService.UpdateUserStatus(user, request.Status);
        return ResponseEntity<EmptyResponse>.Succeeded();
    }
}