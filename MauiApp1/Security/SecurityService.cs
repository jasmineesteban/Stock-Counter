using System.Security.Cryptography;
using System.Text;
using MauiApp1.Interfaces;

namespace MauiApp1.Security
{
    public class SecurityService : ISecurity
    {
        private readonly byte[] _key;

        public SecurityService(string ConnString, byte[] salt)
        {
            _key = DeriveKeyFromPassword(ConnString, salt);
        }

        public byte[] DeriveKeyFromPassword(string ConnString, byte[] salt)
        {
            var iterations = 1000;
            var desiredKeyLength = 32;
            var hashMethod = HashAlgorithmName.SHA256;

            using (var deriveBytes = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(ConnString), salt, iterations, hashMethod))
            {
                return deriveBytes.GetBytes(desiredKeyLength);
            }
        }

        public async Task<string> DecryptAsync(string cipherText)
        {
            try
            {
                var fullCipher = Convert.FromBase64String(cipherText);
                using (var aes = Aes.Create())
                {
                    aes.Key = _key;

                    byte[] iv = new byte[aes.BlockSize / 8];
                    byte[] cipher = new byte[fullCipher.Length - iv.Length];

                    Array.Copy(fullCipher, iv, iv.Length);
                    Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);

                    aes.IV = iv;
                    aes.Padding = PaddingMode.PKCS7;

                    using (var descriptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    using (var ms = new MemoryStream(cipher))
                    using (var cs = new CryptoStream(ms, descriptor, CryptoStreamMode.Read))
                    using (var sr = new StreamReader(cs))
                    {
                        return await sr.ReadToEndAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Decryption failed: " + ex.Message);
                throw;
            }
        }
    }
}