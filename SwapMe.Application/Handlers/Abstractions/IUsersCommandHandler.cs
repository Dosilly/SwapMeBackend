using SwapMe.Application.Handlers.Users;
using SwapMe.Domain.Users;

namespace SwapMe.Application.Handlers.Abstractions;

public interface IUsersCommandHandler
{
    Task<User> HandleAsync(CreateUserCommand command);
}