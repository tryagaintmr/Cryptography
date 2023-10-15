using System.Text;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto;

Console.WriteLine("Enter text to hash:");
var text = Console.ReadLine();

Console.WriteLine($"SHA256: {ComputeSHA256(text)}");
Console.WriteLine($"SHA3: {ComputeSHA3(text)}");


static string ComputeSHA256(string input)
{
    using(SHA256 sha256 = SHA256.Create())
    {
        byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        return BitConverter.ToString(bytes).Replace("-", "").ToLower();
    }
}

static string ComputeSHA3(string input)
{
    IDigest digest = new Sha3Digest(256);
    byte[] inputBytes = Encoding.UTF8.GetBytes(input);
    digest.BlockUpdate(inputBytes, 0, inputBytes.Length);
    byte[] result = new byte[digest.GetDigestSize()];
    digest.DoFinal(result, 0);
    return BitConverter.ToString(result).Replace("-", "").ToLower();
}