using FluentValidation;

namespace MeskChatApplication.Application.Features.Commands.User.UpdateProfilePhoto;

public class UpdateProfilePhotoCommandValidator : AbstractValidator<UpdateProfilePhotoCommand>
{
    public UpdateProfilePhotoCommandValidator()
    {
        RuleFor(p => p.Stream)
            .NotNull().WithMessage("Stream cannot be null")
            .Must(stream => stream.Length <= 5 * 1024 * 1024)
            .WithMessage("Stream length must be between 5 MB");
    }    
}