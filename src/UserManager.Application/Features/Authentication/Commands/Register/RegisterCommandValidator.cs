using FluentValidation;

namespace UserManager.Application.Features.Authentication.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Request.FirstName)
            .NotEmpty()
            .WithMessage("First Name is required");

        RuleFor(x => x.Request.LastName)
            .NotEmpty()
            .WithMessage("Last Name is required");

        RuleFor(x => x.Request.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Email is not valid");

        RuleFor(x => x.Request.UserName)
            .NotEmpty()
            .WithMessage("Username is required");

        RuleFor(x => x.Request.Password)
            .NotEmpty()
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters");
    }
}