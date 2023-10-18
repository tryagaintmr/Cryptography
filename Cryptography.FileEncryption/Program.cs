using System;
using System.IO;
using System.Security.Cryptography;

Console.WriteLine("Welcome to File Encryption/Decryption Console App!");
Console.WriteLine("Choose an option:");
Console.WriteLine("1. Encrypt a file");
Console.WriteLine("2. Decrypt a file");
Console.Write("Enter your choice (1/2): ");

char choice = Console.ReadKey().KeyChar;
Console.WriteLine(); // Move to the next line

switch (choice)
{
    case '1':
        EncryptFile();
        break;
    case '2':
        DecryptFile();
        break;
    default:
        Console.WriteLine("Invalid choice. Please enter '1' to encrypt or '2' to decrypt.");
        break;
}

static void EncryptFile()
{
    Console.Write("Enter the path of the file to encrypt: ");
    string filePath = Console.ReadLine();

    using (Aes aesAlg = Aes.Create())
    {
        // Generate a random encryption key
        aesAlg.GenerateKey();
        byte[] encryptionKey = aesAlg.Key;

        // Display the encryption key (you may want to store it securely)
        Console.WriteLine("Encryption Key:");
        Console.WriteLine(BitConverter.ToString(encryptionKey).Replace("-", ""));

        // Encrypt the file
        byte[] encryptedBytes = Encrypt(filePath, aesAlg);

        // Save the encrypted data to a new file
        string encryptedFilePath = filePath + ".encrypted";
        File.WriteAllBytes(encryptedFilePath, encryptedBytes);
        Console.WriteLine($"File encrypted and saved as {encryptedFilePath}");
    }
}

static byte[] Encrypt(string filePath, SymmetricAlgorithm symmetricAlgorithm)
{
    byte[] encryptedBytes;

    using (FileStream fsInput = new FileStream(filePath, FileMode.Open))
    using (MemoryStream msOutput = new MemoryStream())
    using (CryptoStream cryptoStream = new CryptoStream(msOutput, symmetricAlgorithm.CreateEncryptor(), CryptoStreamMode.Write))
    {
        fsInput.CopyTo(cryptoStream);
        cryptoStream.FlushFinalBlock();
        encryptedBytes = msOutput.ToArray();
    }

    return encryptedBytes;
}

static void DecryptFile()
{
    Console.Write("Enter the path of the encrypted file: ");
    string encryptedFilePath = Console.ReadLine();

    Console.Write("Enter the encryption key (hexadecimal format): ");
    string keyHex = Console.ReadLine();

    if (!string.IsNullOrEmpty(keyHex) && keyHex.Length == 64)
    {
        byte[] encryptionKey = new byte[32];
        for (int i = 0; i < 32; i++)
        {
            encryptionKey[i] = Convert.ToByte(keyHex.Substring(i * 2, 2), 16);
        }

        Decrypt(encryptedFilePath, encryptionKey);
    }
    else
    {
        Console.WriteLine("Invalid key format. Please enter a 32-byte hexadecimal key.");
    }
}

static void Decrypt(string encryptedFilePath, byte[] encryptionKey)
{
    using (Aes aesAlg = Aes.Create())
    {
        aesAlg.Key = encryptionKey;
        byte[] decryptedBytes = DecryptF(encryptedFilePath, aesAlg);

        // Save the decrypted data to a new file
        string decryptedFilePath = encryptedFilePath + ".decrypted";
        File.WriteAllBytes(decryptedFilePath, decryptedBytes);
        Console.WriteLine($"File decrypted and saved as {decryptedFilePath}");
    }
}

 static byte[] DecryptF(string encryptedFilePath, SymmetricAlgorithm symmetricAlgorithm)
{
    byte[] decryptedBytes;

    using (FileStream fsInput = new FileStream(encryptedFilePath, FileMode.Open))
    using (MemoryStream msOutput = new MemoryStream())
    using (CryptoStream cryptoStream = new CryptoStream(msOutput, symmetricAlgorithm.CreateDecryptor(), CryptoStreamMode.Write))
    {
        fsInput.CopyTo(cryptoStream);
        cryptoStream.FlushFinalBlock();
        decryptedBytes = msOutput.ToArray();
    }

    return decryptedBytes;
}
