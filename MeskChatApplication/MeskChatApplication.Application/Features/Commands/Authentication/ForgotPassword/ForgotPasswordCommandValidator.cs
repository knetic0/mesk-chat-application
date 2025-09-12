using FluentValidation;

namespace MeskChatApplication.Application.Features.Commands.Authentication.ForgotPassword;

public sealed class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(dto => dto.Email).NotNull().WithMessage("Email is required.");
        RuleFor(dto => dto.Email).NotEmpty().WithMessage("Email is required.");
        RuleFor(dto => dto.Email).EmailAddress().WithMessage("Email is not valid.");
    }
}