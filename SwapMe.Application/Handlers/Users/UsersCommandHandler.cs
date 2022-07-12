using System.Text;
using Microsoft.Extensions.Logging;
using SwapMe.Application.Abstractions;
using SwapMe.Application.Handlers.Abstractions;
using SwapMe.Domain.Users;

namespace SwapMe.Application.Handlers.Users;

public class UsersCommandHandler : IUsersCommandHandler
{
    private readonly IUsersService _usersService;
    private readonly ILogger<UsersCommandHandler> _logger;

    public UsersCommandHandler(IUsersService usersService, 
        ILogger<UsersCommandHandler> logger)
    {
        _usersService = usersService;
        _logger = logger;
    }

    public async Task<User> HandleAsync(CreateUserCommand command)
    {
        var salt = PasswordSaltedHashGenerator.CreateSalt();
        var hashedPassword = PasswordSaltedHashGenerator.GenerateSaltedHash(command.Password, salt);
        
        _logger.LogTrace("Generated salted password for a new user");

        var userContact = new UserContact(command.FirstName,
            command.LastName,
            command.Email,
            command.PhoneNumber,
            command.City,
            command.State);
        var newUser = new User(command.Login, hashedPassword, Convert.ToBase64String(salt))
        {
            UserContact = userContact
        };

        return await _usersService.CreateUserAsync(newUser);
    }
}