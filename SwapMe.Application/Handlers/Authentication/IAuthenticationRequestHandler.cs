namespace SwapMe.Application.Handlers.Authentication;

public interface IAuthenticationRequestHandler
{
    Task<string?> AuthorizeAsync(AuthenticationRequest request);
}