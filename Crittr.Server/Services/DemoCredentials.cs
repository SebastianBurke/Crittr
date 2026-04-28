using System.Security.Cryptography;

namespace Crittr.Server.Services;

public sealed class DemoCredentials
{
    public string DemoEmail { get; } = "demo@crittr.ca";
    public string Password { get; }

    public DemoCredentials(IConfiguration config)
    {
        var configured = config["Demo:Password"];
        Password = string.IsNullOrWhiteSpace(configured)
            ? GenerateCompliantPassword()
            : configured;
    }

    // Identity policy requires length >= 12, lowercase, uppercase, digit, non-alphanumeric.
    // Prefix guarantees all four classes regardless of base64 output.
    private static string GenerateCompliantPassword()
    {
        var raw = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        return "Aa1!" + raw;
    }
}
