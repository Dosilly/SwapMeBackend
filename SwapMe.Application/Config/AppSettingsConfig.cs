namespace SwapMe.Application.Config;

public record AppSettingsConfig
{
    public string JwtSecret { get; set; } = string.Empty;
}