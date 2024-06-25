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

    /// <summary>
    /// Encrypts a plain text using AES encryption.
    /// </summary>
    /// <param name="plainText">The plain text to encrypt.</param>
    /// <param name="encryptionKey">The encryption key (32 bytes).</param>
    /// <returns>The encrypted text in base-64 format.</returns>
    public static string AesEncrypt(string plainText, string encryptionKey)
    {
        byte[] encrypted;

        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aes.IV = new byte[16];

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using MemoryStream msEncrypt = new();
            using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (StreamWriter swEncrypt = new(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }
            encrypted = msEncrypt.ToArray();
        }

        return Convert.ToBase64String(encrypted);
    }

    /// <summary>
    /// Decrypts an AES encrypted text.
    /// </summary>
    /// <param name="cipherText">The encrypted text in base-64 format.</param>
    /// <param name="encryptionKey">The encryption key (32 bytes).</param>
    /// <returns>The decrypted plain text.</returns>
    public static string AesDecrypt(string cipherText, string encryptionKey)
    {
        string plaintext;
        byte[] cipherBytes = Convert.FromBase64String(cipherText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aes.IV = new byte[16];

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using MemoryStream msDecrypt = new(cipherBytes);
            using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new(csDecrypt);
            plaintext = srDecrypt.ReadToEnd();
        }

        return plaintext;
    }

    /// <summary>
    /// Converts a hexadecimal string to a byte array.
    /// </summary>
    /// <param name="hexString">The hexadecimal string to convert.</param>
    /// <returns>The byte array representation of the hexadecimal string.</returns>
    public static byte[] HexStringToByteArray(string hexString)
    {
        hexString = hexString.Replace(" ", "");
        byte[] buffer = new byte[hexString.Length / 2];
        for (int i = 0; i < hexString.Length; i += 2)
        {
            buffer[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
        }
        return buffer;
    }
}
