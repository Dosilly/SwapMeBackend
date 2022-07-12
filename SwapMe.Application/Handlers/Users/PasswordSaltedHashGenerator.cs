using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace SwapMe.Application.Handlers.Users;

public static class PasswordSaltedHashGenerator
{
    public static string GenerateSaltedHash(string password, byte[] salt)
    {
        var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
        return hashed;
    }
    
    public static byte[] CreateSalt()
    {
        var salt = new byte[128 / 8];
        using var rng = RandomNumberGenerator.Create();
        rng.GetNonZeroBytes(salt);
        return salt;
    }
}