using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace SwapMe.Application.Handlers.Users;

public static class PasswordSaltedHashGenerator
{
    public static string GenerateSaltedHash(string password, byte[] salt)
    {
        var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password,
            salt,
            KeyDerivationPrf.HMACSHA256,
            100000,
            256 / 8));
        return hashed;
    }

    public static string GenerateSaltedHash(string password, string salt)
    {
        var saltToBytes = StringToBytesArray(salt);
        return GenerateSaltedHash(password, saltToBytes);
    }

    public static byte[] CreateSalt()
    {
        var salt = new byte[128 / 8];
        using var rng = RandomNumberGenerator.Create();
        rng.GetNonZeroBytes(salt);
        return salt;
    }

    private static byte[] StringToBytesArray(string password) => Convert.FromBase64String(password);
}