using FluentValidation;
using SwapMe.Application.Handlers.Users;

namespace SwapMe.Application.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(c => c.Login).NotEmpty();
        RuleFor(c => c.Password).NotEmpty();
        RuleFor(c => c.FirstName).NotEmpty();
        RuleFor(c => c.LastName).NotEmpty();
    }
}