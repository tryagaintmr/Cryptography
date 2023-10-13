using System.Text;
using System.Security.Cryptography;


string message = "Hello, World!";
byte[] messageBytes = Encoding.UTF8.GetBytes(message);

// 1. Generate RSA Key Pair
using (var rsa = new RSACryptoServiceProvider())
{
    // 2. Create a Hash
    byte[] hash;
    using (var sha256 = SHA256.Create())
    {
        hash = sha256.ComputeHash(messageBytes);
    }

    // 3. Sign the hash
    byte[] signature = rsa.SignHash(hash, CryptoConfig.MapNameToOID("SHA256"));

    // 4. Verify the Signature
    bool isVerified = rsa.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA256"), signature);

    Console.WriteLine($"Message: {message}");
    Console.WriteLine($"Signature: {Convert.ToBase64String(signature)}");
    Console.WriteLine($"Verification Result: {isVerified}");

}