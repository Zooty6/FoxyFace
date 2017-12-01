using System;
using System.Security.Cryptography;

public static class PasswordHasher
{
// 24 = 192 bits
    private const int SaltByteSize = 24;
    private const int HashByteSize = 24;
    private const int HasingIterationsCount = 10101;

    public static byte[] ComputeHash(string password, byte[] salt, int iterations=HasingIterationsCount, int hashByteSize=HashByteSize)
    {
        Rfc2898DeriveBytes hashGenerator = new Rfc2898DeriveBytes(password, salt);
        hashGenerator.IterationCount = iterations;
        return hashGenerator.GetBytes(hashByteSize);
    }

    public static byte[] GenerateSalt(int saltByteSize=SaltByteSize)
    {
        RNGCryptoServiceProvider saltGenerator = new RNGCryptoServiceProvider();
        byte[] salt = new byte[saltByteSize];
        saltGenerator.GetBytes(salt);
        return salt;
    }

    public static bool VerifyPassword(String password, byte[] passwordSalt, byte[] passwordHash)
    {
        byte[] computedHash = ComputeHash(password, passwordSalt);
        return AreHashesEqual(computedHash, passwordHash);
    }

//Length constant verification - prevents timing attack
    private static bool AreHashesEqual(byte[] firstHash, byte[] secondHash)
    { 
        int minHashLenght = firstHash.Length <= secondHash.Length ? firstHash.Length : secondHash.Length;
        var xor = firstHash.Length ^ secondHash.Length;
        for (int i = 0; i < minHashLenght; i++)
            xor |= firstHash[i] ^ secondHash[i];
        return 0 == xor;
    }
    
    public static void Encrypt(out string encodedPassword, out string generatedSalt, string password)
    {
        byte[] generatedSaltBytes = GenerateSalt();
        byte[] encodedPasswordBytes = ComputeHash(password, generatedSaltBytes);

        generatedSalt = Convert.ToBase64String(generatedSaltBytes);
        encodedPassword = Convert.ToBase64String(encodedPasswordBytes);
    }
    
    public static bool Compare(string goodPassword, string salt, string actualPassword)
    {
        byte[] generatedSaltBytes = Convert.FromBase64String(salt);
        byte[] actualEncodedPasswordBytes = ComputeHash(actualPassword, generatedSaltBytes);
        byte[] goodPasswordBytes = Convert.FromBase64String(goodPassword);

        return AreHashesEqual(goodPasswordBytes, actualEncodedPasswordBytes);
    }
}