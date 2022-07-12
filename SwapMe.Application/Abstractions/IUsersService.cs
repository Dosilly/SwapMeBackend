using SwapMe.Domain.Users;

namespace SwapMe.Application.Abstractions;

public interface IUsersService
{
    Task<User?> GetUserByLoginAsync(string login);
    Task<User> CreateUserAsync(User user);
}