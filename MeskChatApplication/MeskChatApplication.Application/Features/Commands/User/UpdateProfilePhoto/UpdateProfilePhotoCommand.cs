using MESK.MediatR;
using MESK.ResponseEntity;

namespace MeskChatApplication.Application.Features.Commands.User.UpdateProfilePhoto;

public record UpdateProfilePhotoCommand(Stream Stream, Guid UserId) : IRequest<ResponseEntity<string>>;