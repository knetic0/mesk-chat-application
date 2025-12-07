using MESK.MediatR;
using MESK.ResponseEntity;
using MeskChatApplication.Application.Attributes;
using MeskChatApplication.Application.Exceptions;
using MeskChatApplication.Application.Services;

namespace MeskChatApplication.Application.Features.Commands.User.UpdateProfilePhoto;

[Transactional]
public class UpdateProfilePhotoCommandHandler(IUserService userService, IFileStorageService fileStorageService) : IRequestHandler<UpdateProfilePhotoCommand, ResponseEntity<string>>
{
    private readonly IFileStorageService _fileStorageService = fileStorageService;
    private readonly IUserService _userService = userService;
    
    public async Task<ResponseEntity<string>> Handle(UpdateProfilePhotoCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userService.GetAsync(user => user.Id == request.UserId, cancellationToken);
        if (user is null) throw new NotFoundException(nameof(User), request.UserId);
        
        var fileName = $"{request.UserId.ToString()}-profile";
        var photoUrl = await _fileStorageService.UploadAsync(request.Stream, fileName, cancellationToken);
        
        _userService.UpdateProfilePhoto(user, photoUrl.Data!);
        
        return photoUrl;
    }
}