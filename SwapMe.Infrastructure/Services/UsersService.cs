using Microsoft.EntityFrameworkCore;
using SwapMe.Application.Abstractions;
using SwapMe.Domain.Users;
using SwapMe.Infrastructure.Sql.Contexts;

namespace SwapMe.Infrastructure.Services;

public class UsersService : IUsersService
{
    private readonly UsersContext _context;

    public UsersService(UsersContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByLoginAsync(string login)
    {
        return await _context.Users.FirstOrDefaultAsync(u =>
            u.Login.Equals(login));
    }

    public async Task<User?> GetByIdAsync(long id)
    {
        return await _context.Users.FirstOrDefaultAsync(u =>
            u.UserId == id);
    }

    public async Task<User> CreateAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return user;
    }
}