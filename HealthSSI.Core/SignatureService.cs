using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HealthSSI.Core
{
    /// <summary>
    /// Service for validating signatures of messages using public key
    /// </summary>
    public class SignatureService : ISignatureService
    {
        /// <summary>
        /// Convert a pem string into an public key (RSAParameters )
        /// </summary>
        /// <param name="pem"></param>
        /// <returns></returns>
        public RSAParameters GetPublicKeyFromPem(string pem)
        {
            return ImportPublicKey(pem).ExportParameters(false);
        }

        /// <summary>
        /// Get the RSACryptoServiceProvider from a pem string
        /// </summary>
        /// <param name="pem"></param>
        /// <returns></returns>
        public RSACryptoServiceProvider ImportPublicKey(string pem)
        {
            PemReader pr = new PemReader(new StringReader(pem));
            AsymmetricKeyParameter publicKey = (AsymmetricKeyParameter)pr.ReadObject();
            RSAParameters rsaParams = DotNetUtilities.ToRSAParameters((RsaKeyParameters)publicKey);

            RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
            csp.ImportParameters(rsaParams);
            return csp;
        }

        /// <summary>
        /// Check a signed message against the public key
        /// </summary>
        /// <param name="originalMessage"></param>
        /// <param name="signedMessage"></param>
        /// <param name="publicKey"></param>
        /// <returns>bool</returns>
        public bool VerifySignature(string originalMessage, string signedMessage, RSAParameters publicKey)
        {
            bool success = false;
            using (var rsa = new RSACryptoServiceProvider())
            {
                ASCIIEncoding byteConverter = new ASCIIEncoding();

                byte[] bytesToVerify = byteConverter.GetBytes(originalMessage);
                byte[] signedBytes = Convert.FromBase64String(signedMessage);

                try
                {
                    rsa.ImportParameters(publicKey);

                    success = rsa.VerifyData(bytesToVerify, CryptoConfig.MapNameToOID("SHA512"), signedBytes);
                }
                catch (CryptographicException e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
            return success;
        }
    }
}
