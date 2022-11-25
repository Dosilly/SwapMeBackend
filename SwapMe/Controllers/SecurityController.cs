using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwapMe.Application.Handlers.Authentication;

namespace SwapMe.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SecurityController : ControllerBase
{
    private readonly ILogger<SecurityController> _logger;
    private readonly IAuthenticationRequestHandler _authHandler;

    public SecurityController(ILogger<SecurityController> logger, IAuthenticationRequestHandler authHandler)
    {
        _logger = logger;
        _authHandler = authHandler;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateToken([FromBody] AuthenticationRequest request)
    {
        _logger.LogInformation("Received request for token generation");
        var result = await new AuthenticationRequestValidator().ValidateAsync(request);

        if (result.IsValid)
        {
            var token = await _authHandler.AuthorizeAsync(request);

            if (token is null)
            {
                return Unauthorized("User does not exists or password is incorrect");
            }
            
            return Ok(token);
        }

        return BadRequest("Invalid request");
    }
}