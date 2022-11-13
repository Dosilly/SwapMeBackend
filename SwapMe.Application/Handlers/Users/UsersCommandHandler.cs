using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SwapMe.Application.Abstractions;
using SwapMe.Application.Config;
using SwapMe.Application.Handlers.Abstractions;
using SwapMe.Application.Handlers.Users.Requests;
using SwapMe.Application.Handlers.Users.Results;
using SwapMe.Domain.Users;

namespace SwapMe.Application.Handlers.Users;

public class UsersCommandHandler : IUsersCommandHandler
{
    private readonly IUsersService _usersService;
    private readonly ILogger<UsersCommandHandler> _logger;

    public UsersCommandHandler(
        IUsersService usersService,
        ILogger<UsersCommandHandler> logger)
    {
        _usersService = usersService;
        _logger = logger;
    }

    public async Task<User> CreateAsync(CreateUserRequest request)
    {
        var salt = PasswordSaltedHashGenerator.CreateSalt();
        var hashedPassword = PasswordSaltedHashGenerator.GenerateSaltedHash(request.Password, salt);

        _logger.LogTrace("Generated salted password for a new user");

        var userContact = new UserContact(request.FirstName,
            request.LastName,
            request.Email,
            request.PhoneNumber,
            request.City,
            request.State);
        var newUser = new User(request.Login, hashedPassword, Convert.ToBase64String(salt))
        {
            UserContact = userContact
        };

        return await _usersService.CreateAsync(newUser);
    }
}