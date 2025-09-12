using FluentValidation;

namespace MeskChatApplication.Application.Features.Commands.Authentication.Logout;

public sealed class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator()
    {
        RuleFor(dto => dto.RefreshToken).NotNull().WithMessage("RefreshToken is required.");
        RuleFor(dto => dto.RefreshToken).NotEmpty().WithMessage("RefreshToken is required.");
    }
}