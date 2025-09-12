using FluentValidation;

namespace MeskChatApplication.Application.Features.Commands.Authentication.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(dto => dto.Email).NotEmpty().WithMessage("Email is required.");
        RuleFor(dto => dto.Email).NotNull().WithMessage("Email is required.");
        RuleFor(dto => dto.Email).EmailAddress().WithMessage("Invalid email address.");
        
        RuleFor(dto => dto.Password).NotEmpty().WithMessage("Password is required.");
        RuleFor(dto => dto.Password).NotNull().WithMessage("Password is required.");
        RuleFor(dto => dto.Password).MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        RuleFor(dto => dto.Password).Matches("[A-Z]").WithMessage("The password must be include minimum 1 upper letter!");
        RuleFor(dto => dto.Password).Matches("[a-z]").WithMessage("The password must be include minimum 1 lower letter!");
        RuleFor(dto => dto.Password).Matches("[0-9]").WithMessage("The password must be include minimum 1 number!");
        RuleFor(dto => dto.Password).Matches("[^a-zA-Z0-9]").WithMessage("The password must be include minimum 1 special character!");
    }
}