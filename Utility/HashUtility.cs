using System.Security.Cryptography;
using System.Text;

namespace Utility
{
    public class HashUtility
    {
        /// <summary>
        /// Computes the SHA256 hash of a string value.
        /// </summary>
        /// <param name="plainText">The string to hash.</param>
        /// <param name="salt">An optional salt value to append to the plain text before hashing. Defaults to an empty string.</param>
        /// <returns>The SHA256 hash of the combined plain text and salt (if provided) in lowercase hexadecimal format.</returns>
        public static string ToSHA256(string plainText, string salt = "")
        {
            // ref https://docs.microsoft.com/zh-tw/dotnet/api/system.security.cryptography.sha256?view=net-6.0
                string combinedString = salt + plainText;

                byte[] combinedBytes = Encoding.UTF8.GetBytes(combinedString);
            byte[] hashBytes = SHA256.HashData(combinedBytes);

                // Convert directly from byte array to hex string with a StringBuilder
            var sb = new StringBuilder(hashBytes.Length * 2);
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }

        public static string GenerateSalt(int length = 16)
        {
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Salt length must be positive.");
            }

            byte[] saltBytes = new byte[length];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            // Convert bytes to a base-64 encoded string for easier storage and use
            return Convert.ToBase64String(saltBytes);
        }
    }
}
