namespace SwapMe.Application.Handlers.Users.Results;

public record AuthenticateResult
{
    private AuthenticateResult()
    {
    }
    
    public static AuthenticateResult Fail(Exception ex) => 
        new AuthenticateResult { Exception = ex };
    
    public static AuthenticateResult Success(string token) => 
        new AuthenticateResult { Result = true, Token = token};

    public Exception? Exception { get; init; }
    public bool Result { get; init; }
    public string? Token { get; init; }
}

