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
    private readonly IConfiguration _iconfig;
    private readonly AppSettingsConfig _config;
    private readonly ILogger<UsersCommandHandler> _logger;

    public UsersCommandHandler(IUsersService usersService,
        IOptions<AppSettingsConfig> config, 
        IConfiguration iconfig,
        ILogger<UsersCommandHandler> logger)
    {
        _usersService = usersService;
        _iconfig = iconfig;
        _config = config.Value;
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

    public async Task<AuthenticateResult?> AuthenticateAsync(AuthenticationRequest request)
    {
        var matchingUser = await _usersService.GetByLoginAsync(request.Login);
        
        if (matchingUser is null)
        {
            return AuthenticateResult.Fail(new AuthenticationException("Not existing user"));
        }

        var saltedPassword = PasswordSaltedHashGenerator.GenerateSaltedHash(
            request.Password, Convert.FromBase64String(matchingUser.Salt));

        return saltedPassword != matchingUser.Password 
            ? AuthenticateResult.Fail(new AuthenticationException("Incorrect password"))
            : AuthenticateResult.Success(GenerateJwtToken(matchingUser));
    }
    
    private string GenerateJwtToken(User user)
    {
        const int TokenExpirationInDays = 1;
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config.JwtSecret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.UserId.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(TokenExpirationInDays),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}