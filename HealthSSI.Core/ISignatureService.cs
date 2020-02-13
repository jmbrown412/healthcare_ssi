using System.Security.Cryptography;

namespace HealthSSI.Core
{
    public interface ISignatureService
    {
        public bool VerifySignature(string message, string signedMessage, RSAParameters publicKey);
        public RSAParameters GetPublicKeyFromPem(string pem);
        public RSACryptoServiceProvider ImportPublicKey(string pem);
    }
}
