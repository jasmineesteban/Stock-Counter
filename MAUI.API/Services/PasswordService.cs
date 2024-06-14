using System.Security.Cryptography;
using System.Text;

namespace MAUI.API.Services;

public class PasswordService
{
    private const int SaltSize = 10;
    public (string salt, string hash) GenerateSaltAndHash(string plainPassword)
    {
        if(string.IsNullOrWhiteSpace(plainPassword))
            throw new ArgumentNullException(nameof(plainPassword));

        var buffer = RandomNumberGenerator.GetBytes(10);
        var salt = Convert.ToBase64String(buffer);

        byte[] bytes  = Encoding.UTF8.GetBytes(plainPassword + salt);

        var hash = SHA256.HashData(bytes);

        var hashedPassword = Convert.ToBase64String(hash);

        return (salt, hashedPassword);

    }

    public bool Compare(string plainPassword, string hashs string salt)
    {

    }
}
