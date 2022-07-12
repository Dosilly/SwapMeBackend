namespace SwapMe.Application.Handlers.Users;

public record CreateUserCommand(
    string Login,
    string Password,
    string FirstName,
    string LastName,
    string Email,
    long PhoneNumber,
    string City,
    string State);