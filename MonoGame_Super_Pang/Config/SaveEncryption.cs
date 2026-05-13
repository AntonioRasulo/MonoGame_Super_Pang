using System;
using System.Security.Cryptography;
using System.Text;

namespace MonoGame_Super_Pang.Config;

public static class SaveEncryption
{
    // Change this to any secret string — bake it into your build.
    // Not unbreakable if someone decompiles, but stops casual cheating.
    private const string MasterSecret = "MonoGame_SuperPang_$ecr3t_2025!";
    private const int KeySize    = 32; // AES-256
    private const int IvSize     = 16; // AES block size
    private const int SaltSize   = 16;
    private const int Iterations = 100_000;

    /// <summary>Encrypt a UTF-8 string. Returns Base64 of salt+iv+ciphertext.</summary>
    public static string Encrypt(string plainText)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] key  = DeriveKey(salt);

        using var aes = Aes.Create();
        aes.Key     = key;
        aes.GenerateIV();
        aes.Mode    = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var encryptor = aes.CreateEncryptor();
        byte[] plainBytes   = Encoding.UTF8.GetBytes(plainText);
        byte[] cipherBytes  = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        // Layout: [16 salt][16 iv][N ciphertext]
        byte[] result = new byte[SaltSize + IvSize + cipherBytes.Length];
        Buffer.BlockCopy(salt,        0, result, 0,                    SaltSize);
        Buffer.BlockCopy(aes.IV,      0, result, SaltSize,             IvSize);
        Buffer.BlockCopy(cipherBytes, 0, result, SaltSize + IvSize,    cipherBytes.Length);

        return Convert.ToBase64String(result);
    }

    /// <summary>Decrypt a Base64 string produced by Encrypt().</summary>
    public static string Decrypt(string cipherBase64)
    {
        byte[] data = Convert.FromBase64String(cipherBase64);

        if (data.Length < SaltSize + IvSize)
            throw new CryptographicException("Payload too short — file may be corrupt or tampered.");

        byte[] salt   = new byte[SaltSize];
        byte[] iv     = new byte[IvSize];
        int    cipherLen = data.Length - SaltSize - IvSize;
        byte[] cipherBytes = new byte[cipherLen];

        Buffer.BlockCopy(data, 0,                 salt,        0, SaltSize);
        Buffer.BlockCopy(data, SaltSize,           iv,          0, IvSize);
        Buffer.BlockCopy(data, SaltSize + IvSize,  cipherBytes, 0, cipherLen);

        byte[] key = DeriveKey(salt);

        using var aes = Aes.Create();
        aes.Key     = key;
        aes.IV      = iv;
        aes.Mode    = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var decryptor = aes.CreateDecryptor();
        byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
        return Encoding.UTF8.GetString(plainBytes);
    }

    private static byte[] DeriveKey(byte[] salt)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(
            MasterSecret,
            salt,
            Iterations,
            HashAlgorithmName.SHA256);
        return pbkdf2.GetBytes(KeySize);
    }
}