using FluentValidation;

namespace SwapMe.Application.Handlers.Users.Requests;

public record CreateUserRequest(
    string Login,
    string Password,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string City,
    string State);

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        RuleFor(c => c.Login).NotEmpty();
        RuleFor(c => c.Password).NotEmpty();
        RuleFor(c => c.FirstName).NotEmpty();
        RuleFor(c => c.LastName).NotEmpty();
    }
}