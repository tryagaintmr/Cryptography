using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

Console.WriteLine("Digital Signature Console App");
Console.WriteLine("1. Sign a Document");
Console.WriteLine("2. Verify a Document Signature");
Console.Write("Enter your choice (1 or 2): ");
string choice = Console.ReadLine();

switch (choice)
{
    case "1":
        SignDocument();
        break;
    case "2":
        VerifyDocument();
        break;
    default:
        Console.WriteLine("Invalid choice.");
        break;
}

static void SignDocument()
{
    try
    {
        Console.Write("Enter the path to the document to sign: ");
        string documentPath = Console.ReadLine();

        Console.Write("Enter the path to the PFX certificate file (or press Enter to generate a self-signed certificate): ");
        string certificatePath = Console.ReadLine();

        X509Certificate2 certificate;

        if (string.IsNullOrWhiteSpace(certificatePath))
        {
            // Generate a self-signed certificate if no certificate path is provided
            certificate = GenerateSelfSignedCertificate();
        }
        else
        {
            Console.Write("Enter the password for the certificate: ");
            string certificatePassword = Console.ReadLine();
            certificate = new X509Certificate2(certificatePath, certificatePassword);
        }

        // Sign the document
        SignDocumentWithRSA(documentPath, certificate);

        Console.WriteLine("Document signed successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error signing the document: " + ex.Message);
    }
}

static void VerifyDocument()
{
    try
    {
        Console.Write("Enter the path to the document to verify: ");
        string documentPath = Console.ReadLine();

        Console.Write("Enter the path to the PFX certificate file for verification: ");
        string certificatePath = Console.ReadLine();

        Console.Write("Enter the password for the certificate: ");
        string certificatePassword = Console.ReadLine();

        X509Certificate2 certificate = new X509Certificate2(certificatePath, certificatePassword);

        // Verify the document's signature
        bool isVerified = VerifyDocumentSignature(documentPath, certificate);

        Console.WriteLine("Signature Verification: " + (isVerified ? "Successful" : "Failed"));
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error verifying the signature: " + ex.Message);
    }
}

static X509Certificate2 GenerateSelfSignedCertificate()
{
    // Generate a self-signed certificate for testing purposes
    using (RSA rsa = RSA.Create(2048))
    {
        var request = new CertificateRequest("cn=SelfSignedCertificate", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        var certificate = request.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(1));

        // Export the certificate to a PFX file (password: "password")
        File.WriteAllBytes("SelfSignedCertificate.pfx", certificate.Export(X509ContentType.Pfx, "password"));

        return new X509Certificate2("SelfSignedCertificate.pfx", "password");
    }
}

static void SignDocumentWithRSA(string documentPath, X509Certificate2 certificate)
{
    try
    {
        // Load the document to sign
        string documentContents = File.ReadAllText(documentPath, Encoding.UTF8);
        byte[] documentBytes = Encoding.UTF8.GetBytes(documentContents);

        // Create a digital signature using the certificate's private key
        using (RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)certificate.PrivateKey)
        {
            byte[] signature = rsa.SignData(documentBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            // Append the signature to the document
            File.WriteAllBytes(documentPath + ".sig", signature);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error signing the document: " + ex.Message);
    }
}

static bool VerifyDocumentSignature(string documentPath, X509Certificate2 certificate)
{
    try
    {
        // Load the document and its signature
        string documentContents = File.ReadAllText(documentPath, Encoding.UTF8);
        byte[] documentBytes = Encoding.UTF8.GetBytes(documentContents);
        byte[] signature = File.ReadAllBytes(documentPath + ".sig");

        // Verify the signature using the certificate's public key
        using (RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)certificate.PublicKey.Key)
        {
            return rsa.VerifyData(documentBytes, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error verifying the signature: " + ex.Message);
        return false;
    }
}
