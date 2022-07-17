using SwapMe.Domain.Users;

namespace SwapMe.Application.Abstractions;

public interface IUsersService
{
    Task<User?> GetByLoginAsync(string login);
    Task<User> CreateAsync(User user);
    Task<User?> GetByIdAsync(long id);
}