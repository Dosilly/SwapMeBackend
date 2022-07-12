using Microsoft.AspNetCore.Mvc;
using SwapMe.Application.Handlers.Abstractions;
using SwapMe.Application.Handlers.Users;
using SwapMe.Application.Validators;

namespace SwapMe.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUsersCommandHandler _commandHandler;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUsersCommandHandler commandHandler, ILogger<UsersController> logger)
    {
        _commandHandler = commandHandler;
        _logger = logger;
    }

    [HttpPost(Name = "Create")]
    public async Task<IActionResult> PostAsync([FromBody] CreateUserCommand command)
    {
        using (_logger.BeginScope(command.Login))
        {
            _logger.LogInformation("Received request for new user creation");
            var result = await new CreateUserValidator().ValidateAsync(command);

            if (result.IsValid)
            {
                 var user = await _commandHandler.HandleAsync(command);
                 return Ok(user);
            }

            _logger.LogWarning("Validation not passed due to: \r\n {Errors}", result.Errors.ToString());
            return BadRequest();
        }
    }
}