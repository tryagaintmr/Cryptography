using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

var certificate = CreateSelfSignedCertificate();
Console.WriteLine($"Certificate:\n{certificate}");

static X509Certificate2 CreateSelfSignedCertificate()
{
    var request = new CertificateRequest(
        "CN=test",
        RSA.Create(2048),
        HashAlgorithmName.SHA256,
        RSASignaturePadding.Pkcs1);
    var certificate = request.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(1));
    return new X509Certificate2(certificate.Export(X509ContentType.Pfx, "DevForEver"), "DevForEver", X509KeyStorageFlags.Exportable);
}