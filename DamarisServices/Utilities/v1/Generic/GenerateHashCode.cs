using System.Security.Cryptography;
using System.Text;

namespace DamarisServices.Utilities.v1.Generic
{
    public static class GenerateHashCode
    {

        internal static string GetHashCode(string message)
        {
            // Use a cryptographic hash function to generate a unique hash value.
            SHA256? sha256 = SHA256.Create();
            byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(message));

            // Convert the hash to a string.
            string key = Convert.ToBase64String(hash);

            // Return the hash as a string.
            return key;
        }
    }
}
