using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

var currentDateTime = DateTimeOffset.Now;

var root = CreateCertificate("CN=Root CA", null, null, currentDateTime, true);
var intermediate = CreateCertificate("CN=Intermediate CA", root, root.GetRSAPrivateKey(), currentDateTime, true);
var endCertificate = CreateCertificate("CN=End Certificate", intermediate, intermediate.GetRSAPrivateKey(), currentDateTime); //my bug occur here

Console.WriteLine($"Root CA:\n{root}\n");
Console.WriteLine($"Intermediate CA:\n{intermediate}\n");
Console.WriteLine($"End Certificate:\n{endCertificate}");


static X509Certificate2 CreateCertificate(string subjectName, X509Certificate2 issuer, RSA issuerPrivateKey, DateTimeOffset currentDateTime, bool isCa = false)
{
    // Create a new RSA key pair for the current certificate.
    RSA rsaKeyPair = RSA.Create(2048);

    var request = new CertificateRequest(subjectName, rsaKeyPair, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

    if (isCa)
    {
        request.CertificateExtensions.Add(
            new X509BasicConstraintsExtension(true, false, 0, true)
            );
    }

    X509Certificate2 certificate;

    if (issuer == null) // Self-signed
    {
        certificate = request.CreateSelfSigned(currentDateTime, currentDateTime.AddYears(1));
        return new X509Certificate2(certificate.Export(X509ContentType.Pfx, "DevForEver"), "DevForEver", X509KeyStorageFlags.Exportable);
    }
    else // signed by another certificate
    {
        if (issuerPrivateKey == null)
            throw new ArgumentException("Issuer's private key is required");

        var signatureGenerator = X509SignatureGenerator.CreateForRSA(issuerPrivateKey, RSASignaturePadding.Pkcs1);
        certificate = request.Create(issuer.SubjectName, signatureGenerator, currentDateTime, currentDateTime.AddYears(1), Guid.NewGuid().ToByteArray());
    }

    // Associate the RSA key pair with the signed certificate.
    var certWithKey = certificate.CopyWithPrivateKey(rsaKeyPair);
    return new X509Certificate2(certWithKey.Export(X509ContentType.Pfx, "DevForEver"), "DevForEver", X509KeyStorageFlags.Exportable);
}
