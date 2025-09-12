using FluentValidation;

namespace MeskChatApplication.Application.Features.Commands.Authentication.RefreshToken;

public sealed class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(r => r.RefreshToken).NotEmpty().WithMessage("RefreshToken is required.");
        RuleFor(r => r.RefreshToken).NotNull().WithMessage("User is required.");
    }
}