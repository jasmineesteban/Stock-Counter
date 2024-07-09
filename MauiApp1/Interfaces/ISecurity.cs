namespace MauiApp1.Interfaces
{
    public interface ISecurity
    {
        byte[] DeriveKeyFromPassword(string ConnString, byte[] salt);
        Task<string> DecryptAsync(string cipherText);
    }
}
