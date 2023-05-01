using FluentValidation;

namespace UserManager.Application.Features.Authentication.Queries.Login;

public class LoginQueryValidator : AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
    {
        RuleFor(x => x.Request.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Email is not valid.");
    }
}