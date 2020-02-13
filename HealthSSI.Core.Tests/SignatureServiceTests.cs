using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using FluentAssertions;
using HealthSSI.Data.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace HealthSSI.Core.Tests
{
    [TestClass]
    public class SignatureServiceTests : BaseCoreTest
    {
        private ISignatureService GetSignatureService()
        {
            return new SignatureService();
        }



        [TestMethod]
        public void Checking_Message_That_Was_Signed_By_Hospital_For_Should_Return_Success()
        {
            // given
            var cryptoProvider = GetCryptoProvider();
            RSAParameters rsaPrivateKeyInfo = cryptoProvider.ExportParameters(true);
            Patient patient = new Patient("joe", "smith", "joe.smith@gmail.com");

            // simulate having an ID from the DB
            patient.Id = 123;
            Document doc = new Document(DateTime.Now, patient.Id.ToString());
            var message = doc.ToJson();
            string signedMessage = SignData(message, rsaPrivateKeyInfo);
            string publicKeyPem = ExportPublicKey(cryptoProvider);

            // when
            var signatureService = GetSignatureService();
            RSACryptoServiceProvider importedKey = signatureService.ImportPublicKey(publicKeyPem);
            var result = signatureService.VerifySignature(message, signedMessage, importedKey.ExportParameters(false));

            // then
            result.Should().BeTrue();
        }

        [TestMethod]
        public void Checking_Message_That_Was_Not_Signed_By_Hospital_Should_Not_Return_Success()
        {
            // given
            var cryptoProvider = GetCryptoProvider();
            RSAParameters rsaPrivateKeyInfo = cryptoProvider.ExportParameters(true);
            Patient patient = new Patient("jon", "smith", "jon.smith@gmail.com");

            // simulate having an ID from the DB
            patient.Id = 124;
            Document doc = new Document(DateTime.Now, patient.Id.ToString());
            var message = doc.ToJson();
            string signedMessage = SignData(message, rsaPrivateKeyInfo);

            // Keypair not signed with
            var unusedCryptoProvider = GetCryptoProvider();
            string unusedPublicKeyPem = ExportPublicKey(unusedCryptoProvider);

            // when
            var signatureService = GetSignatureService();
            RSACryptoServiceProvider importedKey = signatureService.ImportPublicKey(unusedPublicKeyPem);
            var result = signatureService.VerifySignature(message, signedMessage, importedKey.ExportParameters(false));

            // then
            result.Should().BeFalse();
        }
    }
}
