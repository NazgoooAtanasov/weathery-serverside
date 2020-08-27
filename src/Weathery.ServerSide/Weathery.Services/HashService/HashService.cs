namespace Weathery.Services.HashService
{
    using System.Security.Cryptography;
    using System.Text;

    public class HashService : IHashService
    {
        public string Hash(string rawData, string username)
        {
            var hashedPassword = "";
            using var shaHash = SHA256.Create();
            var bytes = shaHash.ComputeHash(Encoding.UTF8.GetBytes(rawData + username));

            // Convert byte array to a string   
            for (var i = 0; i < bytes.Length; i++) hashedPassword += bytes[i].ToString("x2");

            return hashedPassword;
        }
    }
}