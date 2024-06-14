using System.Security.Cryptography;
using System.Text;

namespace Utility;

public static class HashUtility
{
    /// <summary>
    /// Computes the SHA256 hash of a string value using an empty string as the salt.
    /// </summary>
    /// <param name="plainText">The string to hash.</param>
    /// <returns>The SHA256 hash of the plain text in lowercase hexadecimal format.</returns>
    public static string ToSHA256(string plainText)
    {
        return ToSHA256(plainText, "");
    }

    /// <summary>
    /// Computes the SHA256 hash of a string value.
    /// </summary>
    /// <param name="plainText">The string to hash.</param>
    /// <param name="salt">An optional salt value to append to the plain text before hashing. Defaults to an empty string.</param>
    /// <returns>The SHA256 hash of the combined plain text and salt (if provided) in lowercase hexadecimal format.</returns>
    public static string ToSHA256(string plainText, string salt)
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

    /// <summary>
    /// Generates a random cryptographic salt with a default length of 16 bytes.
    /// </summary>
    /// <returns>A base-64 encoded string representing the generated salt.</returns>
    public static string GenerateSalt()
    {
        return GenerateSalt(16);
    }

    /// <summary>
    /// Generates a random cryptographic salt with a specified length.
    /// </summary>
    /// <param name="length">The desired length of the salt in bytes. Must be positive.</param>
    /// <returns>A base-64 encoded string representing the generated salt.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the provided length is less than or equal to zero.</exception>
    public static string GenerateSalt(int length)
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
