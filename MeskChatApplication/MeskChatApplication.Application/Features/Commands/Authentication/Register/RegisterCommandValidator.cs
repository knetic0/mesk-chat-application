using FluentValidation;

namespace MeskChatApplication.Application.Features.Commands.Authentication.Register;

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(dto => dto.FirstName).NotEmpty().WithMessage("First name is required.");
        RuleFor(dto => dto.FirstName).NotNull().WithMessage("First name is required.");
        RuleFor(dto => dto.FirstName).MinimumLength(3).WithMessage("First name must be at least 3 characters long.");
        RuleFor(dto => dto.FirstName).MaximumLength(30).WithMessage("First name must be at max 30 characters long.");
        
        RuleFor(dto => dto.LastName).NotEmpty().WithMessage("Last name is required.");
        RuleFor(dto => dto.LastName).NotNull().WithMessage("Last name is required.");
        RuleFor(dto => dto.LastName).MinimumLength(3).WithMessage("Last name must be at least 3 characters long.");
        RuleFor(dto => dto.LastName).MaximumLength(30).WithMessage("Last name must be at max 30 characters long.");
        
        RuleFor(dto => dto.Username).NotEmpty().WithMessage("User name is required.");
        RuleFor(dto => dto.Username).NotNull().WithMessage("User name is required.");
        RuleFor(dto => dto.Username).MinimumLength(3).WithMessage("User name must be at least 3 characters long.");
        RuleFor(dto => dto.Username).MaximumLength(30).WithMessage("User name must be at max 30 characters long.");
        
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