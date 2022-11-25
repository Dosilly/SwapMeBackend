using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SwapMe.Application.Abstractions;
using SwapMe.Application.Config;
using SwapMe.Application.Handlers.Users;

namespace SwapMe.Application.Handlers.Authentication;

public class AuthenticationRequestHandler : IAuthenticationRequestHandler
{
    private readonly ILogger<AuthenticationRequestHandler> _logger;
    private readonly IUsersService _usersService;
    private readonly JwtSettings _jwtSettings;

    public AuthenticationRequestHandler(ILogger<AuthenticationRequestHandler> logger, IUsersService usersService, IOptions<JwtSettings> jwtSettings)
    {
        _logger = logger;
        _usersService = usersService;
        _jwtSettings = jwtSettings.Value;
    }
    
    public async Task<string?> AuthorizeAsync(AuthenticationRequest request)
    {
        var user = await _usersService.GetByLoginAsync(request.Login);

        if (user is not null)
        {
            var saltedPassword = PasswordSaltedHashGenerator.GenerateSaltedHash(request.Password, user.Salt);
            if (saltedPassword == user.Password)
            {
                var key = Encoding.ASCII.GetBytes(_jwtSettings.JwtSecret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Login),
                        new Claim(JwtRegisteredClaimNames.Email, user.UserContact?.Email ?? string.Empty),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var stringToken = tokenHandler.WriteToken(token);
                
                _logger.LogInformation("Successfully generated token for user {User}", user.Login);
                
                return stringToken;
            }
        }
        
        _logger.LogWarning("Request for token generation failed: {User} user does not exists", request.Login);

        return null;
    }
}