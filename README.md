# Cryptography Projects Overview

This repository contains a collection of projects dedicated to showcasing various cryptographic functionalities using C#. The projects demonstrate the creation of certificates, certificate chains, digital signatures, encryption/decryption, and hashing.
## 1. Cryptography.Certificate

Type: Console Project

Description:
This project demonstrates the creation of an X.509 certificate. It provides hands-on insights into how a single certificate can be generated and the properties it possesses.

Usage:
Run the project to generate and view the details of a newly created certificate.
## 2. Cryptography.CertificatesChain

Type: Console Project

Description:
This project delves into the creation of a certificate chain, consisting of three certificates:

    Root Certificate
    Intermediate Certificate
    End Certificate

The root signs the intermediate, and the intermediate signs the end certificate, thus forming a chain of trust.

Usage:
Execute the project to generate a chain of certificates and inspect their hierarchical trust relationship.
## 3. Cryptography.DigitalSignature

Type: Console Project

Description:
The focus of this project is on digital signatures. It demonstrates how to sign a SHA-256 hash of a string and then verify the signature to ensure data integrity and authenticity.

Usage:
Run the project, provide a string input, and witness the process of signing and verification using the generated digital signature.
## 4. Cryptography.DigitalSigning

Type: Console Project

Description:
This project showcases asymmetric encryption using RSA. It guides through the process of encrypting a string using a public RSA key and then decrypting it using the corresponding private RSA key.

Usage:
Initiate the project, input a string, and observe the encryption and subsequent decryption process, ensuring data confidentiality.
## 5. Cryptography.HashDemo

Type: Console Project

Description:
Hashing is fundamental in cryptography. This project offers a practical demonstration of hashing a string using two algorithms: SHA-256 and SHA-3. It emphasizes the fixed-length output and non-reversibility of hashes.

Usage:
Run the application, provide a string, and see the hashed outputs using both SHA-256 and SHA-3 algorithms.

Note: Before running any of the projects, ensure you've set up the necessary dependencies and configurations as required. Always ensure the safety and security of any cryptographic operation and key management.