using Microsoft.AspNetCore.Mvc;
using SwapMe.Application.Handlers.Abstractions;
using SwapMe.Application.Handlers.Users.Requests;


namespace SwapMe.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUsersCommandHandler _commandHandler;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUsersCommandHandler commandHandler, ILogger<UsersController> logger)
    {
        _commandHandler = commandHandler;
        _logger = logger;
    }

    [HttpPost("create")]
    public async Task<IActionResult> PostAsync([FromBody] CreateUserRequest request)
    {
        using (_logger.BeginScope(request.Login))
        {
            _logger.LogInformation("Received request for new user creation");
            var result = await new CreateUserValidator().ValidateAsync(request);

            if (result.IsValid)
            {
                 var user = await _commandHandler.CreateAsync(request);
                 return Ok(user);
            }

            _logger.LogWarning("Validation not passed due to: \r\n {Errors}", result.Errors.ToString());
            return BadRequest(result.Errors.ToArray());
        }
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate(AuthenticationRequest request)
    {
        using (_logger.BeginScope(request.Login))
        {
            _logger.LogInformation("Received authentication request");
            var result = await new AuthenticationRequestValidator().ValidateAsync(request);

            if (result.IsValid)
            {
                var token = await _commandHandler.AuthenticateAsync(request);
                return token?.Result is null || token.Result == false
                    ? BadRequest(token?.Exception)
                    : Ok(token);
            }

            return BadRequest(result.Errors.ToArray());

        }
    }
}