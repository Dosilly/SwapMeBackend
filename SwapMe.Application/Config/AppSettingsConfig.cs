namespace SwapMe.Application.Config;

public record JwtSettings
{
    public string JwtSecret { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
}