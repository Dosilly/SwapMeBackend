using SwapMe.Application.Handlers.Users.Requests;
using SwapMe.Domain.Users;

namespace SwapMe.Application.Handlers.Abstractions;

public interface IUsersCommandHandler
{
    Task<User> CreateAsync(CreateUserRequest request);
}