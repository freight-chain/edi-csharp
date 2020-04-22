using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using FreightTrust.EDI.Certificates;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using AlgorithmIdentifier = System.Security.Cryptography.Pkcs.AlgorithmIdentifier;
using BigInteger = Org.BouncyCastle.Math.BigInteger;
using ContentInfo = System.Security.Cryptography.Pkcs.ContentInfo;
using X509Certificate = Org.BouncyCastle.X509.X509Certificate;

namespace FreightTrust.EDI.AS2
{

    public class AS2Encryption
    {
       
        public static X509Certificate2 GenerateCertificate(string certName)
        {
            var keypairgen = new RsaKeyPairGenerator();
            keypairgen.Init(new KeyGenerationParameters(new SecureRandom(new CryptoApiRandomGenerator()), 1024));

            var keypair = keypairgen.GenerateKeyPair();
            
            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(keypair.Private);
            byte[] serializedPrivateBytes = privateKeyInfo.ToAsn1Object().GetEncoded();
            string serializedPrivate = Convert.ToBase64String(serializedPrivateBytes);
            
            SubjectPublicKeyInfo publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(keypair.Public);
            byte[] serializedPublicBytes = publicKeyInfo.ToAsn1Object().GetDerEncoded();
            string serializedPublic = Convert.ToBase64String(serializedPublicBytes);
            
            Console.WriteLine(serializedPrivate);
            var gen = new X509V3CertificateGenerator();

            var CN = new X509Name("CN=" + certName);
            var SN = BigInteger.ProbablePrime(120, new Random());

            gen.SetSerialNumber(SN);
            gen.SetSubjectDN(CN);
            gen.SetIssuerDN(CN);
            gen.SetNotAfter(DateTime.MaxValue);
            gen.SetNotBefore(DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)));
            gen.SetSignatureAlgorithm("MD5WithRSA");
            gen.SetPublicKey(keypair.Public);           
        
            var newCert = gen.Generate(keypair.Private);
            
            
            var result = new X509Certificate2(serializedPublicBytes);
            //result.PrivateKey = Crypto.DecodeRsaPrivateKey(serializedPrivateBytes);
            return result;
          //result.PrivateKey = new RSACryptoServiceProvider();
        }
        
        public static byte[] Encode(byte[] arMessage, object signerCert, string signerPassword)
        {
            X509Certificate2 cert = signerCert is X509Certificate2 ? (X509Certificate2)signerCert : new X509Certificate2((string)signerCert, signerPassword);
            ContentInfo contentInfo = new ContentInfo(arMessage);
 
            SignedCms signedCms = new SignedCms(contentInfo, true); // <- true detaches the signature
            CmsSigner cmsSigner = new CmsSigner(cert);
 
            signedCms.ComputeSignature(cmsSigner);
            byte[] signature = signedCms.Encode();
 
            return signature;
        }
 
        public static byte[] Encrypt(byte[] message, object recipientCert, string encryptionAlgorithm)
        {
            if (!string.Equals(encryptionAlgorithm, EncryptionAlgorithm.DES3) && !string.Equals(encryptionAlgorithm, EncryptionAlgorithm.RC2))
                throw new ArgumentException("encryptionAlgorithm argument must be 3DES or RC2 - value specified was:" + encryptionAlgorithm);
 
            X509Certificate2 cert = recipientCert is X509Certificate2 ? (X509Certificate2)recipientCert : new X509Certificate2((string)recipientCert);
 
            ContentInfo contentInfo = new ContentInfo(message);
 
            EnvelopedCms envelopedCms = new EnvelopedCms(contentInfo,
                new AlgorithmIdentifier(new System.Security.Cryptography.Oid(encryptionAlgorithm))); // should be 3DES or RC2
 
            CmsRecipient recipient = new CmsRecipient(SubjectIdentifierType.IssuerAndSerialNumber, cert);
 
            envelopedCms.Encrypt(recipient);
 
            byte[] encoded = envelopedCms.Encode();
 
            return encoded;
        }
 
        public static byte[] Decrypt(byte[] encodedEncryptedMessage, out string encryptionAlgorithmName, X509Certificate2 cert = null)
        {
            EnvelopedCms envelopedCms = new EnvelopedCms();
 
            // NB. the message will have been encrypted with your public key.
            // The corresponding private key must be installed in the Personal Certificates folder of the user
            // this process is running as. OR ADD it to the collection below in the X509Certificate2Collection
            envelopedCms.Decode(encodedEncryptedMessage);
 
            envelopedCms.Decrypt(new X509Certificate2Collection(cert));
            encryptionAlgorithmName = envelopedCms.ContentEncryptionAlgorithm.Oid.FriendlyName;
 
            return envelopedCms.Encode();
        }
 
    }
}