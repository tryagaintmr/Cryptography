using System.Text;
using System.Security.Cryptography;



// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


string message = "Hello, World!";

using (var rsa = new RSACryptoServiceProvider(2048))
{

    RSAParameters publicKey = rsa.ExportParameters(false); // export public key
    RSAParameters privateKey = rsa.ExportParameters(true); // export private key

    // 2. Encrypt data using the public key
    string plainText = "Hello, encrypted World";
    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
    byte[] encryptedByte = rsa.Encrypt(plainBytes, false);
    Console.WriteLine($"Encrypted Text: {Convert.ToBase64String(encryptedByte)}");

    // 3. Decrypt data using the private key
    byte[] decryptedByte = rsa.Decrypt(encryptedByte, false);
    string decryptedText = Encoding.UTF8.GetString(decryptedByte);
    Console.WriteLine($"Decrypted Test:{decryptedText}");
}   

